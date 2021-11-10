#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using SomLibrary;
using Util;
using Util.CustomControls;

#endregion

namespace SOMSimulator
{
    internal partial class MainForm : Form
    {
        #region Fields

        //private List<ParameterDetails> m_VisualiserParameters;
        private readonly bool _createAlgInstance; // used to suspend alg creation when loading program state
        private readonly bool _createMapInstance;
        private readonly ExecutionManager _execManager;
        private readonly TextBoxListener _traceOutput;
        private readonly PluginRegistry<ISOM> _algorithmRegistry;
        private readonly PluginRegistry<INeuronMap> _neuronMapRegistry;
        private readonly PluginRegistry<IVisualiser> _visualiserRegistry;
        private ISOM _algInstance;
        private DateTime _algStartTime;
        private List<SOMPluginControl> _algorithmParameters;
        private INeuronMap _currentMap;
        private FileInputLayer _dataSource;
        private List<DisplayInfo> _displayDetails;
        private List<SOMPluginControl> _neuronMapParameters;

        private string _prevAlgSelection = ""; // used by method setting the algorithm listview tooltip

        #endregion Fields

        #region Constructors

        public MainForm()
        {
            InitializeComponent();

            // setup tracing and debugging
            _traceOutput = new TextBoxListener(infoTB);
            Trace.Listeners.Add(_traceOutput);
            // debugging output
            Debug.Listeners.Add(new TextWriterTraceListener(new FileStream("debugOutput.txt", FileMode.Create)));

            _algorithmRegistry = new PluginRegistry<ISOM>();
            _visualiserRegistry = new PluginRegistry<IVisualiser>();
            _neuronMapRegistry = new PluginRegistry<INeuronMap>();

            SetupDisplayPanel();

            _createAlgInstance = true;
            _createMapInstance = true;

            // setup execution manager
            _execManager = new ExecutionManager(_displayDetails);
            _execManager.RunWorkerCompleted += OnAlgorithmStopped;
            _execManager.ProgressChanged += DisplayMapVisual;


            ScanAssemblies();

            SetFormNonExecutingState();
        }

        private void SetupDisplayPanel()
        {
            _displayDetails = new List<DisplayInfo>();
            bool firstDisplay = true;

            foreach (Control ctrl in displayTable.Controls)
            {
                if (ctrl is PictureBox)
                {
                    TableLayoutPanelCellPosition cell = displayTable.GetCellPosition(ctrl);

                    // construct display detail record
                    DisplayInfo details;
                    if (firstDisplay)
                    {
                        details = new DisplayInfo(ctrl as PictureBox, cell, new DefaultVisualiser());
                        firstDisplay = false;
                    }
                    else
                        details = new DisplayInfo(ctrl as PictureBox, cell);

                    _displayDetails.Add(details);
                }
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///   Update GUI controls after plugins have been read from files
        /// </summary>
        private void UpdateSOMControls()
        {
            algorithmLB.Items.Clear();
            neuronMapTypeCB.Items.Clear();
            visualiserLB.Items.Clear();

            // Populate algorithm listbox
            foreach (string name in _algorithmRegistry.NameEnum)
                algorithmLB.Items.Add(name);

            if (algorithmLB.Items.Count > 0)
            {
                if (trainToolStripMenuItem.Enabled == false)
                {
                    trainToolStripMenuItem.Enabled = true;
                    trainBtn.Enabled = true;
                }
                algorithmLB.SelectedIndex = 0; // will update algorithm properties & sellect a neuron map
            }

            // fill visualiser listbox
            foreach (string name in _visualiserRegistry.NameEnum)
                visualiserLB.Items.Add(name);

            if (visualiserLB.Items.Count > 0)
                visualiserLB.SelectedIndex = 0;
        }

        private static TableLayoutPanel PropertyTable(List<SOMPluginControl> parameters)
        {
            // Contract.Requires(parameters != null);
            // Contract.Requires(// Contract.ForAll(parameters, p => p != null));

            Label l;
            Control inputCtrl;
            TableLayoutPanel propTable = new TableLayoutPanel();

            propTable.ColumnCount = 2;
            propTable.Dock = DockStyle.Fill;
            propTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            propTable.AutoScroll = true;

            if (parameters.Count != 0)
            {
                foreach (SOMPluginControl p in parameters)
                {
                    // Contract.Assume(p != null);
                    inputCtrl = p.ParameterControl;
                    if (inputCtrl != null)
                    {
                        l = new Label();
                        l.Text = p.Name;
                        l.Dock = DockStyle.Fill;
                        l.TextAlign = ContentAlignment.MiddleLeft;

                        propTable.Controls.Add(l);
                        propTable.Controls.Add(inputCtrl);
                    }
                }
            }
            propTable.Controls.Add(new Label()); // dummy control to add extra row                 
            return propTable;
        }

        // visualise and draw maps
        private void VisualiseMaps()
        {
            // Re-visualise map at new resolution
            if (_algInstance != null)
            {
                if (_algInstance.Map != null && !_execManager.IsBusy)
                    foreach (DisplayInfo d in _displayDetails)
                    {
                        // Contract.Assume(d != null);

                        d.DrawMapVisual(_algInstance);
                        d.PaintPictureBox();
                    }
            }
            displayTable.Refresh();
        }

        /// <summary>
        ///   Any application cleanup code to go in here
        /// </summary>
        private void QuitApplication()
        {
            /*
            if (m_ExecManager.IsBusy)
                m_ExecManager.CancelAsync();
            
            this.Enabled = false; // prevent any more UI events from occuring

            // this is probably not necessary. What does it matter
            // if the worker thread is aborted!
            while (m_ExecManager.IsBusy)
            {
                // this is a bit of a hack
                // Needed to ensure that execManager.RunWorkerCompleted event is marshalled
                // so that execution thread can terminate
                Application.DoEvents();
            }*/

            Debug.Close();
            Trace.Close();
        }

        private void ScanAssemblies()
        {
            Trace.WriteLine("Searching for plugins ...");
            // Read dll assemblies in current directories
            DirectoryInfo di = new DirectoryInfo(".\\");

            FileInfo[] assemblies = di.GetFiles("*.dll");
            // Contract.Assume(// Contract.ForAll(assemblies, a => a != null));
            foreach (FileInfo fi in assemblies)
                ScanFile(fi.Name);

            assemblies = di.GetFiles("*.exe");
            // Contract.Assume(// Contract.ForAll(assemblies, a => a != null));
            foreach (FileInfo fi in assemblies)
                ScanFile(fi.Name);
        }

        private bool ScanFile(string fn)
        {
            // This is a long function but trivial
            Assembly a;

            try
            {
                a = Assembly.LoadFrom(fn);
            }
            catch (FileLoadException)
            {
                return false;
            }
            catch (BadImageFormatException)
            {
                return false;
            }

            // Contract.Assume(a != null);
            Type[] tarray = a.GetExportedTypes();
            object[] pluginAttribute;
            foreach (Type t in tarray)
            {
                // Contract.Assume(t != null);
                pluginAttribute = t.GetCustomAttributes(typeof (SOMPluginDetailAttribute), false);

                if (pluginAttribute.Length == 1)
                {
                    // read algorithms
                    if (null != t.GetInterface(typeof (ISOM).FullName) && !t.IsAbstract)
                    {
                        if (_algorithmRegistry.Add(t))
                            Trace.WriteLine(Path.GetFileName(fn) + ":\t" + t.Name + " algorithm found");
                    }
                    else // read visualisers
                        if (null != t.GetInterface(typeof (IVisualiser).FullName) && !t.IsAbstract)
                        {
                            if (_visualiserRegistry.Add(t))
                                Trace.WriteLine(Path.GetFileName(fn) + ":\t" + t.Name + " visualiser found");
                        }
                        else // read neuron map types
                            if ((t.GetInterface(typeof (INeuronMap).FullName) != null) && !t.IsAbstract)
                            {
                                if (_neuronMapRegistry.Add(t))
                                    Trace.WriteLine(Path.GetFileName(fn) + ":\t" + "Type of Neuron Map found: " + t.Name);
                            }
                }
            }

            UpdateSOMControls();

            return true;
        }

        private void SelectInputSource(object sender, EventArgs e)
        {
            if (sender == openFileToolStripMenuItem)
            {
                openFileToolStripMenuItem.Checked = true;
                OpenTrainingFile(sender, e);
            }
        }

        private void SetFormExecutingState()
        {
            simulateToolStripMenuItem.Enabled = false;
            inputSourceToolStripMenuItem.Enabled = false;
            scanAssemblyToolStripMenuItem.Enabled = false;
            saveMapMenuItem.Enabled = false;
            loadMapMenuItem.Enabled = false;
            writeMapVectorsToolStripMenuItem.Enabled = false;
            visualiserPropPanel.Enabled = false;
            algorithmLB.Enabled = false;
            algPropGB.Enabled = false;
            neuronMapPropGB.Enabled = false;
            trainBtn.Enabled = false;
            updateIntervalUpDown.Enabled = false;
            cancelBtn.Enabled = true;

            animationControls.Enabled = false;
            attachVisBtn.Enabled = false;
            detachVisualiserBtn.Enabled = false;
            saveImageBtn.Enabled = false;
        }

        private void SetFormNonExecutingState()
        {
            simulateToolStripMenuItem.Enabled = true;
            inputSourceToolStripMenuItem.Enabled = true;
            scanAssemblyToolStripMenuItem.Enabled = true;
            saveMapMenuItem.Enabled = true;
            loadMapMenuItem.Enabled = true;
            writeMapVectorsToolStripMenuItem.Enabled = true;
            visualiserPropPanel.Enabled = true;
            algorithmLB.Enabled = true;
            algPropGB.Enabled = true;
            neuronMapPropGB.Enabled = true;
            trainBtn.Enabled = true;
            updateIntervalUpDown.Enabled = true;
            cancelBtn.Enabled = false;

            animationControls.Enabled = true;
            attachVisBtn.Enabled = true;
            detachVisualiserBtn.Enabled = true;
            saveImageBtn.Enabled = true;
        }

        private void UpdateVisualiserProperties()
        {

            DisplayInfo info = CurrentCellInfo;

            if (info.Visualiser == null)
                return;

            animationControls.Enabled = true;
            visualiserLabel.Text = info.Visualiser.GetType().Name;
            animFilenameTB.Text = info.AnimationFilename;
            recordAnimCheckBox.Checked = info.GeneratingAnimation;

            if (info.Image != null)
                saveImageBtn.Enabled = true;
            else
                saveImageBtn.Enabled = false;
        }

        #endregion Methods

        #region Event Handlers

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            QuitApplication();
        }

        private void OnAlgorithmLbSelectedIndexChanged(object sender, EventArgs e)
        {
            if (algorithmLB.SelectedIndex >= 0)
            {
                if (_createAlgInstance)
                    _algInstance = _algorithmRegistry.CreatePluginInstance((string) algorithmLB.SelectedItem);

                _algorithmParameters = _algorithmRegistry.CompilePropertyControls((string) algorithmLB.SelectedItem,
                                                                                  _algInstance);
                algPropPanel.Controls.Clear();
                algPropPanel.Controls.Add(PropertyTable(_algorithmParameters));
            }

            // Populate neuronMapTypeCB with neuron maps which are compatible with
            // the currently selected algorithm              
            Type neuronMapType;
            neuronMapTypeCB.Items.Clear();
            neuronMapTypeCB.ResetText();
            _currentMap = null;
            foreach (string name in _neuronMapRegistry.NameEnum)
            {
                neuronMapType = _neuronMapRegistry.Details(name).PluginType;
                if (
                    _algorithmRegistry.Details((string) algorithmLB.SelectedItem).AcceptableMapType.IsAssignableFrom(
                        neuronMapType))
                    neuronMapTypeCB.Items.Add(name);
            }
            if (neuronMapTypeCB.Items.Count > 0)
                neuronMapTypeCB.SelectedIndex = 0;
        }

        private void OnAlgorithmStopped(object sender, RunWorkerCompletedEventArgs e)
        {
            TimeSpan timer = DateTime.Now - _algStartTime;

            // check for exception occuring in algorithm execution
            if (e.Error != null)
            {
                Debug.WriteLine("Exception occurred when training algorithm:\n" +
                                e.Error.Message);
            }

            while (_execManager.IsBusy)
            {
            }

            SetFormNonExecutingState();

            Trace.WriteLine("Training finished in " + timer.Minutes + " Minutes, " + timer.Seconds +
                            " Seconds.");

            foreach (DisplayInfo d in _displayDetails)
            {
                d.WriteAnimation();
            }

            foreach (DisplayInfo d in _displayDetails)
                if (d.Visualiser is DefaultVisualiser)
                    ((DefaultVisualiser) d.Visualiser).Stop();
        }

        private void DisplayMapVisual(object sender, ProgressChangedEventArgs e)
        {
            foreach (DisplayInfo info in _displayDetails)
                info.PaintPictureBox();

            _execManager.updateEvent.Set();
        }

        private void OnCancelBtnClick(object sender, EventArgs e)
        {
            cancelBtn.Enabled = false;
            _execManager.CancelAsync();
        }

        private void OpenTrainingFile(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog
                                          {
                                              Title = "Enter Training File...",
                                              Filter =
                                                  "data files (*.dat;*.data;*.csv)|*.dat;*.data;*.csv|All files (*.*)|*.*",
                                              RestoreDirectory = true
                                          };
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _dataSource = new FileInputLayer(openFile.FileName);

                    if (randomInput.Checked)
                        _dataSource.RandomizeInput = true;
                }
                catch (IOException)
                {
                    MessageBox.Show("Can't open file", "Error", MessageBoxButtons.OK);
                }
                catch (SOMFileException ex)
                {
                    MessageBox.Show("Error parsing file at line: " + ex.Line +
                                    ", attribute: " + ex.Attribute, "Error", MessageBoxButtons.OK);
                }
            }
        }

        private void OnQuitToolStripMenuItemClick(object sender, EventArgs e)
        {
            QuitApplication();
            Close();
        }

        private void OnRecordAnimCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            DisplayInfo info = CurrentCellInfo;

            info.GeneratingAnimation = recordAnimCheckBox.Checked;

            if (recordAnimCheckBox.Checked)
            {
                animFilenameTB.Enabled = true;
                SelectAnimFileBtn.Enabled = true;
            }
            else
            {
                animFilenameTB.Enabled = false;
                SelectAnimFileBtn.Enabled = false;
            }
        }

        private void OnScanAssemblyToolStripMenuItemClick(object sender, EventArgs e)
        {
            OpenFileDialog assOpenDlg = new OpenFileDialog();

            assOpenDlg.Filter = "assemblies (*.exe, *.dll)|*.dll; *.exe|All files (*.*)|*.*";
            assOpenDlg.RestoreDirectory = true;

            if (assOpenDlg.ShowDialog() == DialogResult.OK)
            {
                Trace.WriteLine("Scanning " + Path.GetFileName(assOpenDlg.FileName) + "...");
                if (!ScanFile(assOpenDlg.FileName))
                {
                    Trace.WriteLine("  Invalid assembly file");
                    MessageBox.Show("File specified is not a .NET assembly", "Error", MessageBoxButtons.OK);
                }
                else Trace.WriteLine("  Done.");
            }
        }

        private void OnSelectAnimFileBtnClick(object sender, EventArgs e)
        {
            SaveFileDialog saveDlg = new SaveFileDialog();

            saveDlg.Filter = "gif file| *.gif";
            saveDlg.RestoreDirectory = true;

            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                animFilenameTB.Text = saveDlg.FileName;
            }
        }

        [ToDo("Rethink initialisation mechanism.")]
        private void TrainToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (_dataSource == null)
            {
                MessageBox.Show("Cannot initiate training, no data source loaded!", "Error",
                                MessageBoxButtons.OK);
                return;
            }
            if (_currentMap == null)
            {
                MessageBox.Show("Cannot initiate training, type of neuron map is not specified!", "Error",
                                MessageBoxButtons.OK);
                return;
            }

            if (InitialiseAlgorithm())
            {
                _execManager.Algorithm = _algInstance;

                // Setup default visualiser
                if (_displayDetails[0].Visualiser is DefaultVisualiser)
                    ((DefaultVisualiser) _displayDetails[0].Visualiser).Start(
                        _algInstance.ProgressInterval);

                _algStartTime = DateTime.Now;
                _execManager.RunWorkerAsync();

                SetFormExecutingState();
            }
        }

        private bool InitialiseAlgorithm()
        {
            _algInstance = _algorithmRegistry.CreatePluginInstance((string) algorithmLB.SelectedItem);
            if (_algInstance == null)
            {
                MessageBox.Show("Unable to initialise algorithm", "Error", MessageBoxButtons.OK);
                return false;
            }
            foreach (SOMPluginControl ctrl in _algorithmParameters)
                ctrl.UpdateParameter(_algInstance);
            foreach (SOMPluginControl ctrl in _neuronMapParameters)
                ctrl.UpdateParameter(_currentMap);

            _algInstance.InputReader = _dataSource;
            _currentMap.InputDimension = _dataSource.InputDimension;
            MapInitialiser.Random(_currentMap); // initialise map weightings with random values
            _algInstance.Map = _currentMap;

            _algInstance.ProgressInterval = Convert.ToInt32(updateIntervalUpDown.Value);

            return true;
        }

        private void animFilenameTB_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            DisplayInfo info = CurrentCellInfo;
            info.AnimationFilename = tb.Text;
        }

        [ToDo("Extend this so that the visualDisplay can be saved as a vector file (needed for papers)")]
        private void saveImageBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveImgDlg = new SaveFileDialog();

            saveImgDlg.Filter = "PNG Image|*.png|JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            saveImgDlg.RestoreDirectory = true;
            saveImgDlg.Title = "Save Image As ...";

            if (saveImgDlg.ShowDialog() == DialogResult.OK)
            {
                // If the file name is not an empty string open it for saving.
                if (saveImgDlg.FileName != "")
                {
                    FileStream fs;
                    if ((fs = (FileStream) saveImgDlg.OpenFile()) != null)
                    {
                        DisplayInfo info = CurrentCellInfo;
                        Image im = info.Image;
                        switch (saveImgDlg.FilterIndex)
                        {
                            case 1:
                                im.Save(fs, ImageFormat.Png);
                                break;

                            case 2:
                                im.Save(fs, ImageFormat.Jpeg);
                                break;

                            case 3:
                                im.Save(fs, ImageFormat.Bmp);
                                break;

                            case 4:
                                im.Save(fs, ImageFormat.Gif);
                                break;
                            default:
                                break;
                        }
                        fs.Close();
                    }
                }
            }
        }

        private void neuronMapTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_createMapInstance)
            {
                if (neuronMapTypeCB.Items.Count > 0)
                    _currentMap = _neuronMapRegistry.CreatePluginInstance((string) neuronMapTypeCB.SelectedItem);
            }
            if (_dataSource != null)
                _currentMap.InputDimension = _dataSource.InputDimension;

            _neuronMapParameters = _neuronMapRegistry.CompilePropertyControls((string) neuronMapTypeCB.SelectedItem,
                                                                              _currentMap);
            neuronMapPropPanel.Controls.Clear();
            neuronMapPropPanel.Controls.Add(PropertyTable(_neuronMapParameters));
        }

        private void writeMapVectorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((_algInstance == null) || (_algInstance.Map == null))
            {
                MessageBox.Show("No map to write!!!");
                return;
            }

            SaveFileDialog saveProgramDlg = new SaveFileDialog();

            saveProgramDlg.Filter = "Neuron map file|*.map";
            saveProgramDlg.RestoreDirectory = true;
            saveProgramDlg.Title = "Save neuron map as ...";

            if (saveProgramDlg.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveProgramDlg.OpenFile());

                for (int i = 0; i < _algInstance.Map.MapSize; i++)
                    sw.WriteLine(_algInstance.Map[i]);

                sw.Close();
            }
        }

        private void saveAlgStateMenuItem_Click(object sender, EventArgs e)
        {
            // Contract.Requires(_algInstance != null);

            if (!_algInstance.GetType().IsSerializable)
            {
                MessageBox.Show("Current algorithm doesn't support saving its" +
                                " state.", "Error", MessageBoxButtons.OK);
                return;
            }

            SaveFileDialog saveMapDlg = new SaveFileDialog();

            saveMapDlg.Filter = "SOMSimulator Algorithm file|*.somalg";
            saveMapDlg.RestoreDirectory = true;
            saveMapDlg.Title = "Save algorithm state As ...";


            if (saveMapDlg.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = (FileStream) saveMapDlg.OpenFile();
                BinaryFormatter formatter = new BinaryFormatter();

                try
                {
                    // Serialise the map state if possible
                    formatter.Serialize(fs, _algInstance);
                }
                catch (SerializationException ex)
                {
                    Trace.WriteLine("Unable to save algorithm state: " + ex.Message);
                }
                finally
                {
                    fs.Close();
                }
            }
        }

        private void loadAlgStateMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog mapStateOpenDlg = new OpenFileDialog();

            mapStateOpenDlg.Filter = "SOMSimulator map files (*.somalg)|*.somalg|All files (*.*)|*.*";
            mapStateOpenDlg.RestoreDirectory = true;
            mapStateOpenDlg.Title = "Open Map...";

            if (mapStateOpenDlg.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = (FileStream) mapStateOpenDlg.OpenFile();
                BinaryFormatter formatter = new BinaryFormatter();

                try
                {
                    _algInstance = (ISOM) formatter.Deserialize(fs);
                }
                catch (SerializationException ex)
                {
                    Trace.WriteLine("Unable to load map state: " + ex.Message);
                }
                catch (InvalidCastException)
                {
                    Trace.WriteLine("Invalid map!");
                }
                finally
                {
                    fs.Close();
                }

                VisualiseMaps();
            }
        }

        private void randomInput_CheckedChanged(object sender, EventArgs e)
        {
            if (randomInput.Checked)
                _dataSource.RandomizeInput = true;
            else
                _dataSource.RandomizeInput = false;
        }

        private void attachVisBtn_Click(object sender, EventArgs e)
        {
            if (visualiserLB.SelectedIndex == -1)
                return;

            DisplayInfo info = CurrentCellInfo;

            info.VisualiserName = (string) visualiserLB.SelectedItem;

            info.Visualiser =
                _visualiserRegistry.CreatePluginInstance((string) visualiserLB.SelectedItem);

            UpdateVisualiserProperties();

            VisualiseMaps();
        }

        private void detachVisualiserBtn_Click(object sender, EventArgs e)
        {
            DisplayInfo info = CurrentCellInfo;

            info.Visualiser = null;

            detachVisualiserBtn.Enabled = false;
            UpdateVisualiserProperties();

            VisualiseMaps();
        }

        private void algorithmLB_MouseMove(object sender, MouseEventArgs e)
        {
            string strTip = "";

            //Get the item
            int nIdx = algorithmLB.IndexFromPoint(e.Location);


            if ((nIdx >= 0) && (nIdx < algorithmLB.Items.Count))
            {
                strTip = _algorithmRegistry.Details(algorithmLB.Items[nIdx].ToString()).Description;

                if (algorithmLB.Items[nIdx].ToString() != _prevAlgSelection) // prevent tooltip from flickering
                {
                    toolTip1.SetToolTip(algorithmLB, strTip);
                    _prevAlgSelection = algorithmLB.Items[nIdx].ToString();
                }
            }
        }

        #endregion

        private DisplayInfo CurrentCellInfo
        {
            get { return _displayDetails.Find(info => info.DisplayCell == displayTable.SelectedCell); }
        }

        private void displayTable_SelectedPanelChanged(object sender, PanelChangedEventArgs e)
        {
            DisplayInfo info = CurrentCellInfo;

            if (info.Visualiser == null)
                detachVisualiserBtn.Enabled = false;
            else
                detachVisualiserBtn.Enabled = true;

            UpdateVisualiserProperties();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            foreach (DisplayInfo info in _displayDetails)
            {
                info.DrawMapVisual(_algInstance);
                info.PaintPictureBox();
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            // Contract.Invariant(displayTable != null);
            // Contract.Invariant(algorithmLB != null);
            // Contract.Invariant(neuronMapTypeCB != null);
            // Contract.Invariant(visualiserLB != null);
            // Contract.Invariant(_algorithmRegistry != null);
            // Contract.Invariant(trainToolStripMenuItem != null);
            // Contract.Invariant(trainBtn != null);
            // Contract.Invariant(_visualiserRegistry != null);
            // Contract.Invariant(_neuronMapRegistry != null);
            // Contract.Invariant(simulateToolStripMenuItem != null);
            // Contract.Invariant(inputSourceToolStripMenuItem != null);
            // Contract.Invariant(scanAssemblyToolStripMenuItem != null);
            // Contract.Invariant(saveMapMenuItem != null);
            // Contract.Invariant(loadMapMenuItem != null);
            // Contract.Invariant(writeMapVectorsToolStripMenuItem != null);
            // Contract.Invariant(visualiserPropPanel != null);
            // Contract.Invariant(algPropGB != null);
            // Contract.Invariant(neuronMapPropGB != null);
            // Contract.Invariant(updateIntervalUpDown != null);
            // Contract.Invariant(cancelBtn != null);
            // Contract.Invariant(animationControls != null);
            // Contract.Invariant(attachVisBtn != null);
            // Contract.Invariant(detachVisualiserBtn != null);
            // Contract.Invariant(saveImageBtn != null);
        }
    }
}