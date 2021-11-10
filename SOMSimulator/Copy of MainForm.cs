using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using System.Diagnostics.Contracts;

using SomLibrary;
using Util;


namespace SOMSimulator
{
    internal partial class MainForm : Form
    {
        [Serializable]
        struct DisplayPanelDetails
        {
            [NonSerialized]
            public IVisualiser Visualiser;
            [NonSerialized]
            public IVisualiser AnimationVisualiser;
            [NonSerialized]
            public GifAnimator GifAnimation;
            [NonSerialized]
            public WinDrawer PanelDrawer;
            [NonSerialized]
            public WinDrawer AnimationDrawer;
            public bool GeneratingAnimation;
            public string AnimationFilename;
            public string VisualiserName;  // used for the deserializing of this data structure
            [NonSerialized]
            public PictureBox pb; // picture box associated with panel
        }

        #region Fields

        private const int animationWidth = 800;
        private const int animationHeight = 600;
        private ISOM _AlgInstance;
        private List<SOMPluginControl> _AlgorithmParameters;
        private List<SOMPluginControl> _NeuronMapParameters;
        //private List<ParameterDetails> m_VisualiserParameters;
        private PluginRegistry<ISOM> _AlgorithmRegistry;
        private PluginRegistry<IVisualiser> _VisualiserRegistry;
        private PluginRegistry<NeuronMap> _NeuronMapRegistry;
        private FileInputLayer _DataSource;
        private NeuronMap _CurrentMap;
        private DefaultVisualiser _DefaultVisualiser;
        private ExecutionManager _ExecManager;
        private DateTime _AlgStartTime;
        private TextBoxListener _TraceOutput;
        private DisplayPanelDetails[] _DisplayPanel;
        private int _SelectedPanel;
        private bool _CreateAlgInstance; // used to suspend alg creation when loading program state
        private bool _CreateMapInstance;

        private string _prevAlgSelection = ""; // used by method setting the algorithm listview tooltip
        #endregion Fields

        #region Constructors

        public MainForm()
        {
            InitializeComponent();

            // setup tracing and debugging
            _TraceOutput = new TextBoxListener(infoTB);
            Trace.Listeners.Add(_TraceOutput);
            // debugging output
            Debug.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(new FileStream("debugOutput.txt", FileMode.Create)));

            _AlgorithmRegistry = new PluginRegistry<ISOM>();
            _VisualiserRegistry = new PluginRegistry<IVisualiser>();
            _NeuronMapRegistry = new PluginRegistry<NeuronMap>();
            _DisplayPanel = new DisplayPanelDetails[4];
            for (int i = 0; i < _DisplayPanel.Length; i++)
            {
                _DisplayPanel[i].pb = (PictureBox)displayArea.GetPanelControl(i);
                _DisplayPanel[i].GifAnimation = new GifAnimator();
                _DisplayPanel[i].AnimationDrawer = new WinDrawer(animationWidth, animationHeight);
                _DisplayPanel[i].PanelDrawer = new WinDrawer(_DisplayPanel[i].pb.Size);
            }
            _CreateAlgInstance = true;
            _CreateMapInstance = true;

            // place default visualiser on first display panel            
            _DefaultVisualiser = new DefaultVisualiser();
            _DisplayPanel[0].Visualiser = _DefaultVisualiser;
            _DisplayPanel[0].Visualiser.Drawer = _DisplayPanel[0].PanelDrawer;

            // setup execution manager
            _ExecManager = new ExecutionManager();
            _ExecManager.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                OnAlgorithmStopped);
            _ExecManager.ProgressChanged += new ProgressChangedEventHandler
                (OnUpdateGraph);


            ScanAssemblies();

            SetFormNonExecutingState();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Update GUI controls after plugins have been read from files
        /// </summary>
        private void UpdateSOMControls()
        {
            algorithmLB.Items.Clear();
            neuronMapTypeCB.Items.Clear();
            visualiserLB.Items.Clear();

            // Populate algorithm listbox
            foreach (string name in _AlgorithmRegistry.NameEnum)
                algorithmLB.Items.Add(name);

            if (algorithmLB.Items.Count > 0)
            {
                if (trainToolStripMenuItem.Enabled == false)
                {
                    trainToolStripMenuItem.Enabled = true;
                    trainBtn.Enabled = true;
                }
                algorithmLB.SelectedIndex = 0;  // will update algorithm properties & sellect a neuron map
            }

            // fill visualiser listbox
            foreach (string name in _VisualiserRegistry.NameEnum)
                visualiserLB.Items.Add(name);

            if (visualiserLB.Items.Count > 0)
                visualiserLB.SelectedIndex = 0;
        }

        private TableLayoutPanel PropertyTable(List<SOMPluginControl> parameters, object Instance)
        {
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
            if (_AlgInstance != null)
            {
                if (_AlgInstance.Map != null && !_ExecManager.IsBusy)
                    foreach (DisplayPanelDetails d in _DisplayPanel)
                    {
                        if (d.Visualiser != null)
                        {
                            if (d.Visualiser.CanVisualiseMap(_AlgInstance))
                            {
                                d.Visualiser.VisualiseMap(_AlgInstance);
                                d.pb.Image = (Image)d.PanelDrawer.GetImage().Clone();
                            }
                        }
                        else
                            d.pb.Image = new Bitmap(d.PanelDrawer.Width, d.PanelDrawer.Height);

                    }
            }
            displayArea.Refresh();
        }

        /// <summary>
        /// Any application cleanup code to go in here
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
            foreach (FileInfo fi in assemblies)
                ScanFile(fi.Name);
            assemblies = di.GetFiles("*.exe");
            foreach (FileInfo fi in assemblies)
                ScanFile(fi.Name);
        }

        private bool ScanFile(string fn)
        {
            // This is a long function but trivial
            Assembly a;
            List<Type> algList = new List<Type>();
            List<Type> visList = new List<Type>();

            try
            {
                a = Assembly.LoadFrom(fn);
            }
            catch (FileLoadException)
            {
                return false;
            }
            Type[] tarray = a.GetExportedTypes();
            object[] pluginAttribute;
            foreach (Type t in tarray)
            {
                pluginAttribute = t.GetCustomAttributes(typeof(SOMPluginDetailAttribute), false);

                if (pluginAttribute.Length == 1)
                {
                    // read algorithms
                    if (null != t.GetInterface(typeof(ISOM).FullName) && !t.IsAbstract)
                    {
                        if (_AlgorithmRegistry.Add(t))
                            Trace.WriteLine(Path.GetFileName(fn) + ":\t" + t.Name + " algorithm found");
                    }
                    else // read visualisers
                        if (null != t.GetInterface(typeof(IVisualiser).FullName) && !t.IsAbstract)
                        {
                            if (_VisualiserRegistry.Add(t))
                                Trace.WriteLine(Path.GetFileName(fn) + ":\t" + t.Name + " visualiser found");
                        }
                        else // read neuron map types
                            if (t.IsSubclassOf(typeof(NeuronMap)) && !t.IsAbstract)
                            {
                                if (_NeuronMapRegistry.Add(t))
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
            saveProgramStateToolStripMenuItem.Enabled = false;
            saveMapMenuItem.Enabled = false;
            loadProgramStateToolStripMenuItem.Enabled = false;
            loadMapMenuItem.Enabled = false;
            writeMapVectorsToolStripMenuItem.Enabled = false;
            visualiserPropPanel.Enabled = false;
            algorithmLB.Enabled = false;
            algPropGB.Enabled = false;
            neuronMapPropGB.Enabled = false;
            resetBtn.Enabled = false;
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
            saveProgramStateToolStripMenuItem.Enabled = true;
            saveMapMenuItem.Enabled = true;
            loadProgramStateToolStripMenuItem.Enabled = true;
            loadMapMenuItem.Enabled = true;
            writeMapVectorsToolStripMenuItem.Enabled = true;
            visualiserPropPanel.Enabled = true;
            algorithmLB.Enabled = true;
            algPropGB.Enabled = true;
            neuronMapPropGB.Enabled = true;
            resetBtn.Enabled = true;
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
            if (_DisplayPanel[_SelectedPanel].Visualiser is IVisualiser)
            {
                animationControls.Enabled = true;
                visualiserLabel.Text = _DisplayPanel[_SelectedPanel].Visualiser.GetType().Name;
                animFilenameTB.Text = _DisplayPanel[_SelectedPanel].AnimationFilename;
                recordAnimCheckBox.Checked = _DisplayPanel[_SelectedPanel].GeneratingAnimation;
            }
            else
            {
                visualiserLabel.Text = "No visualiser active for selected display area.";
                animationControls.Enabled = false;
            }

            if (((PictureBox)displayArea.GetPanelControl(_SelectedPanel)).Image != null)
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

        private void OnAlgorithmLBSelectedIndexChanged(object sender, EventArgs e)
        {
            if (algorithmLB.SelectedIndex >= 0)
            {
                if (_CreateAlgInstance)                
                    _AlgInstance = _AlgorithmRegistry.CreatePluginInstance((string)algorithmLB.SelectedItem);                    

                _AlgorithmParameters = _AlgorithmRegistry.CompilePropertyControls((string)algorithmLB.SelectedItem, _AlgInstance);
                algPropPanel.Controls.Clear();
                algPropPanel.Controls.Add(PropertyTable(_AlgorithmParameters, _AlgInstance));
            }

            // Populate neuronMapTypeCB with neuron maps which are compatible with
            // the currently selected algorithm              
            Type neuronMapType;
            neuronMapTypeCB.Items.Clear();
            neuronMapTypeCB.ResetText();
            _CurrentMap = null;
            foreach (string name in _NeuronMapRegistry.NameEnum)
            {
                neuronMapType = _NeuronMapRegistry.Details(name).PluginType;
                if (_AlgorithmRegistry.Details((string)algorithmLB.SelectedItem).AcceptableMapType.IsAssignableFrom(neuronMapType))                
                    neuronMapTypeCB.Items.Add(name);              
            }            
            if (neuronMapTypeCB.Items.Count > 0)                
                neuronMapTypeCB.SelectedIndex = 0;
        }

        private void OnAlgorithmStopped(object sender, RunWorkerCompletedEventArgs e)
        {
            TimeSpan timer = DateTime.Now - _AlgStartTime;

            // check for exception occuring in algorithm execution
            if (e.Error != null)
            {
                Debug.WriteLine("Exception occurred when training algorithm:\n" +
                    e.Error.Message);
            }

            while (_ExecManager.IsBusy) ;

            SetFormNonExecutingState();
            // synchronize algorithm parameters with their input control.
            foreach (SOMPluginControl d in _AlgorithmParameters)
                d.UpdateParameter();

            Trace.WriteLine("Training finished in " + timer.Minutes + " Minutes, " + timer.Seconds +
                " Seconds.");

            for (int i = 0; i < _DisplayPanel.Length; i++)
            {
                if (_DisplayPanel[i].GeneratingAnimation)
                { // Save animation                       
                    try
                    {
                        _DisplayPanel[i].GifAnimation.SaveFile(Path.GetFileName(
                            _DisplayPanel[i].AnimationFilename));
                    }
                    catch
                    {
                        MessageBox.Show("Error writing animation file: " +
                            Path.GetFileName(_DisplayPanel[i].AnimationFilename),
                            "Error", MessageBoxButtons.OK);
                    }
                }
            }

            foreach (DisplayPanelDetails d in _DisplayPanel)
                if (d.Visualiser is DefaultVisualiser)
                    ((DefaultVisualiser)d.Visualiser).Stop();
        }

        private void OnCancelBtnClick(object sender, EventArgs e)
        {

            cancelBtn.Enabled = false;
            _ExecManager.CancelAsync();

        }

        private void OpenTrainingFile(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "Enter Training File...";
            openFile.Filter = "data files (*.dat;*.data;*.csv)|*.dat;*.data;*.csv|All files (*.*)|*.*";
            openFile.RestoreDirectory = true;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _DataSource = new FileInputLayer(openFile.FileName);

                    if (randomInput.Checked)
                        _DataSource.RandomizeInput = true;
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

        [TODO("Can this method be simplified a bit?")]
        private void OnDisplayPanelSizeChanged(object sender, EventArgs e)
        {
            Control panel = (Control)sender;
            int newWidth = panel.ClientSize.Width - panel.Margin.Left - panel.Margin.Right;
            int newHeight = panel.ClientSize.Height - panel.Margin.Top - panel.Margin.Bottom;
            int panelWidth, panelHeight;

            // ensure that newWidth and newHeight are always 
            if (newWidth < 1) newWidth = 1;
            if (newHeight < 1) newHeight = 1;

            if (newWidth < newHeight)
                displayArea.Size = new Size(newWidth, newWidth);
            else
                displayArea.Size = new Size(newHeight, newHeight);

            PictureBox pb;  // temporary reference to display panel picture boxes
            if (_ExecManager.IsBusy)
                lock (_VisualiserRegistry)
                {
                    for (int i = 0; i < _DisplayPanel.Length; i++)
                    {
                        pb = displayArea.GetPanelControl(i) as PictureBox;
                        panelWidth = pb.Width;
                        panelHeight = pb.Height;
                        if (panelWidth > 0 && panelHeight > 0)
                            _DisplayPanel[i].PanelDrawer.Size = new Size(
                                panelWidth, panelHeight);

                    }
                }
            else
            {
                for (int i = 0; i < _DisplayPanel.Length; i++)
                {
                    pb = displayArea.GetPanelControl(i) as PictureBox;
                    panelWidth = pb.Width;
                    panelHeight = pb.Height;
                    if (panelWidth > 0 && panelHeight > 0)
                        _DisplayPanel[i].PanelDrawer.Size = new Size(
                            panelWidth, panelHeight);
                }
            }

            VisualiseMaps();
        }

        private void OnQuitToolStripMenuItemClick(object sender, EventArgs e)
        {
            QuitApplication();
            this.Close();
        }

        private void OnRecordAnimCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            _DisplayPanel[_SelectedPanel].GeneratingAnimation = recordAnimCheckBox.Checked;

            if (recordAnimCheckBox.Checked == true)
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

        [TODO("Add option for frame speed")]
        private void OnUpdateGraph(object sender, ProgressChangedEventArgs e)
        {
            foreach (DisplayPanelDetails d in _DisplayPanel)
            {
                if (d.Visualiser != null)
                {
                    d.pb.Image = (Image)d.PanelDrawer.GetImage().Clone();
                    //d.pb.Refresh();
                }

                // re-render frame for animation
                if (d.GeneratingAnimation)
                    d.GifAnimation.AddFrame((Image)d.AnimationDrawer.GetImage().Clone(), 25);

            }
            _ExecManager.updateEvent.Set(); // resume worker thread
        }

        private void OnVisualDisplayPaint(object sender, PaintEventArgs e)
        {
            if (_ExecManager.Visualisers != null && !_ExecManager.IsBusy)
            {
                for (int i = 0; i < _DisplayPanel.Length; i++)
                    _DisplayPanel[i].pb.Image = (Image)_DisplayPanel[i].PanelDrawer.GetImage().Clone();
            }
        }

        [TODO("Rethink initialisation mechanism.")]
        private void TrainToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (_AlgInstance == null)
            {
                MessageBox.Show("Cannot initiate training, no algorithm has been selected!", "Error", MessageBoxButtons.OK);
                return;
            }

            if (_DataSource == null)
            {
                MessageBox.Show("Cannot initiate training, no data source loaded!", "Error",
                    MessageBoxButtons.OK);
                return;
            }
            if (_CurrentMap == null)
            {
                MessageBox.Show("Cannot initiate training, type of neuron map is not specified!", "Error", MessageBoxButtons.OK);
                return;
            }

            _AlgInstance.InputReader = _DataSource;
            _CurrentMap.InputDimension = _DataSource.InputDimension;
            MapInitialiser.Random(_CurrentMap); // initialise map weightings with random values
            _AlgInstance.Map = _CurrentMap;
            _AlgInstance.ProgressInterval = Convert.ToInt32(updateIntervalUpDown.Value);
            _ExecManager.Algorithm = _AlgInstance;

            // Setup default visualiser
            if (_DisplayPanel[0].Visualiser is DefaultVisualiser)
                ((DefaultVisualiser)_DisplayPanel[0].Visualiser).Start(
                    _AlgInstance.ProgressInterval);

            // Specify visualisers
            _ExecManager.Visualisers = new List<IVisualiser>();
            foreach (DisplayPanelDetails d in _DisplayPanel)
            {
                if (d.Visualiser != null)
                    _ExecManager.Visualisers.Add(d.Visualiser);
                if (d.GeneratingAnimation)
                {
                    d.GifAnimation.Clear();
                    _ExecManager.Visualisers.Add(d.AnimationVisualiser);
                }
            }

            _AlgStartTime = DateTime.Now;
            _ExecManager.RunWorkerAsync();

            SetFormExecutingState();
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            // force a SelectedIndexChanged event
            int tmp = algorithmLB.SelectedIndex;
            algorithmLB.SelectedIndex = -1;
            algorithmLB.SelectedIndex = tmp;

            // re-display plugin controls
            if (_AlgorithmParameters != null)
                foreach (SOMPluginControl p in _AlgorithmParameters)
                    p.RefreshControlDisplay();            
        }

        private void displayArea_SelectedPanelChanged(object sender, EventArgs e)
        {
            _SelectedPanel = ((MultiPanelSelectorControl)sender).SelectedPanel;
            if (_DisplayPanel[_SelectedPanel].Visualiser == null)
                detachVisualiserBtn.Enabled = false;
            else
                detachVisualiserBtn.Enabled = true;
            UpdateVisualiserProperties();
        }

        private void animFilenameTB_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            _DisplayPanel[_SelectedPanel].AnimationFilename =
                tb.Text;
        }

        [TODO("Extend this so that the visualDisplay can be saved as a vector file (needed for papers)")]
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

                    System.IO.FileStream fs;
                    if ((fs = (System.IO.FileStream)saveImgDlg.OpenFile()) != null)
                    {
                        PictureBox pb = displayArea.GetPanelControl(_SelectedPanel) as PictureBox;
                        switch (saveImgDlg.FilterIndex)
                        {
                            case 1:
                                pb.Image.Save(fs, ImageFormat.Png);
                                break;

                            case 2:
                                pb.Image.Save(fs, ImageFormat.Jpeg);
                                break;

                            case 3:
                                pb.Image.Save(fs, ImageFormat.Bmp);
                                break;

                            case 4:
                                pb.Image.Save(fs, ImageFormat.Gif);
                                break;
                            default:
                                break;
                        }
                        fs.Close();
                    }
                }
            }
        }

        private void saveProgramStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Contract.Requires(this._DisplayPanel != null);
            Contract.Requires(this._AlgInstance != null);

            if (!_AlgInstance.GetType().IsSerializable)
            {
                MessageBox.Show("Current algorithm doesn't support saving its" +
                    " state.", "Error", MessageBoxButtons.OK);
                return;
            }

            SaveFileDialog saveProgramDlg = new SaveFileDialog();

            saveProgramDlg.Filter = "SOM file|*.som";
            saveProgramDlg.RestoreDirectory = true;
            saveProgramDlg.Title = "Save Program State As ...";

            if (saveProgramDlg.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream fs;
                if ((fs = (System.IO.FileStream)saveProgramDlg.OpenFile()) != null)
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    // Serialise the algorithm state if possible
                    formatter.Serialize(fs, _AlgInstance);
                    formatter.Serialize(fs, _DisplayPanel);

                    fs.Close();
                }
            }
        }

        private void loadProgramStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog programStateOpenDlg = new OpenFileDialog();

            programStateOpenDlg.Filter = "SOM files (*.som)|*.som|All files (*.*)|*.*";
            programStateOpenDlg.RestoreDirectory = true;
            programStateOpenDlg.Title = "Open Program State...";

            if (programStateOpenDlg.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream fs = (System.IO.FileStream)programStateOpenDlg.OpenFile();
                BinaryFormatter formatter = new BinaryFormatter();

                _AlgInstance = (ISOM)formatter.Deserialize(fs);
                _DisplayPanel = (DisplayPanelDetails[])formatter.Deserialize(fs);

                // Reconnect global variables
                _DataSource = (FileInputLayer)_AlgInstance.InputReader;                
                for (int i = 0; i < _DisplayPanel.Length; i++)
                {
                    _DisplayPanel[i].pb = displayArea.GetPanelControl(i) as PictureBox;
                    _DisplayPanel[i].GifAnimation = new GifAnimator();
                    _DisplayPanel[i].AnimationDrawer = new WinDrawer(animationWidth, animationHeight);
                    _DisplayPanel[i].PanelDrawer = new WinDrawer(_DisplayPanel[i].pb.Size);

                    // setup visualisers
                    if (_DisplayPanel[i].VisualiserName != null)
                    {
                        _DisplayPanel[i].Visualiser = _VisualiserRegistry.CreatePluginInstance(_DisplayPanel[i].VisualiserName);
                        _DisplayPanel[i].Visualiser.Drawer = _DisplayPanel[i].PanelDrawer;
                        _DisplayPanel[i].AnimationVisualiser = _VisualiserRegistry.CreatePluginInstance(_DisplayPanel[i].VisualiserName);
                        _DisplayPanel[i].AnimationVisualiser.Drawer = _DisplayPanel[i].AnimationDrawer;

                        // draw maps
                        _DisplayPanel[i].Visualiser.VisualiseMap(_AlgInstance);
                        _DisplayPanel[i].pb.Image = (Image)_DisplayPanel[i].PanelDrawer.GetImage().Clone();

                    }
                }

                displayArea.Refresh();

                _CreateAlgInstance = false;
                algorithmLB.SelectedIndex = -1;
                algorithmLB.SelectedItem = _AlgInstance.GetType().Name;
                neuronMapTypeCB.SelectedIndex = -1;
                neuronMapTypeCB.SelectedItem = _AlgInstance.Map.GetType().Name;
                _CreateAlgInstance = true;
            }
        }

        private void neuronMapTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_CreateMapInstance)
            {
                if (neuronMapTypeCB.Items.Count > 0)
                    _CurrentMap = _NeuronMapRegistry.CreatePluginInstance((string)neuronMapTypeCB.SelectedItem);
            }
            if (_DataSource != null)
                _CurrentMap.InputDimension = _DataSource.InputDimension;

            _NeuronMapParameters = _NeuronMapRegistry.CompilePropertyControls((string)neuronMapTypeCB.SelectedItem, _CurrentMap);
            neuronMapPropPanel.Controls.Clear();
            neuronMapPropPanel.Controls.Add(PropertyTable(_NeuronMapParameters, _CurrentMap));

        }

        private void writeMapVectorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((_AlgInstance == null) || (_AlgInstance.Map == null))
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
                StreamWriter sw = new StreamWriter((System.IO.FileStream)saveProgramDlg.OpenFile());

                for (int i = 1; i <= _AlgInstance.Map.MapSize; i++)
                    sw.WriteLine(_AlgInstance.Map[i]);

                sw.Close();
            }
        }

        private void saveMapMenuItem_Click(object sender, EventArgs e)
        {
            Contract.Requires(_AlgInstance.Map != null);

            if (!_CurrentMap.GetType().IsSerializable)
            {
                MessageBox.Show("Current map doesn't support saving its" +
                    " state.", "Error", MessageBoxButtons.OK);
                return;
            }

            SaveFileDialog saveMapDlg = new SaveFileDialog();

            saveMapDlg.Filter = "SOM NeuronMap file|*.nmap";
            saveMapDlg.RestoreDirectory = true;
            saveMapDlg.Title = "Save map state As ...";

            if (saveMapDlg.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveMapDlg.OpenFile();
                BinaryFormatter formatter = new BinaryFormatter();

                // Serialise the map state if possible
                formatter.Serialize(fs, _AlgInstance.Map);

                fs.Close();
            }
        }

        private void loadMapMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog mapStateOpenDlg = new OpenFileDialog();

            mapStateOpenDlg.Filter = "SOM neural map files (*.nmap)|*.nmap|All files (*.*)|*.*";
            mapStateOpenDlg.RestoreDirectory = true;
            mapStateOpenDlg.Title = "Open Map State...";

            if (mapStateOpenDlg.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream fs = (System.IO.FileStream)mapStateOpenDlg.OpenFile();
                BinaryFormatter formatter = new BinaryFormatter();

                _CurrentMap = (NeuronMap)formatter.Deserialize(fs);

                _CreateMapInstance = false;
                neuronMapTypeCB.SelectedIndex = -1;
                neuronMapTypeCB.SelectedItem = _CurrentMap.GetType().Name;
                _CreateMapInstance = true;

                VisualiseMaps();
            }

        }

        private void randomInput_CheckedChanged(object sender, EventArgs e)
        {
            if (randomInput.Checked)
                _DataSource.RandomizeInput = true;
            else
                _DataSource.RandomizeInput = false;
        }

        private void attachVisBtn_Click(object sender, EventArgs e)
        {
            if (visualiserLB.SelectedIndex == -1)
                return;

            _DisplayPanel[_SelectedPanel].VisualiserName = (string)visualiserLB.SelectedItem;

            _DisplayPanel[_SelectedPanel].Visualiser =
                _VisualiserRegistry.CreatePluginInstance((string)visualiserLB.SelectedItem);
            _DisplayPanel[_SelectedPanel].Visualiser.Drawer =
                _DisplayPanel[_SelectedPanel].PanelDrawer;
            _DisplayPanel[_SelectedPanel].AnimationVisualiser =
                _VisualiserRegistry.CreatePluginInstance((string)visualiserLB.SelectedItem);
            _DisplayPanel[_SelectedPanel].AnimationVisualiser.Drawer =
                _DisplayPanel[_SelectedPanel].AnimationDrawer;

            UpdateVisualiserProperties();

            VisualiseMaps();

        }

        private void detachVisualiserBtn_Click(object sender, EventArgs e)
        {
            _DisplayPanel[_SelectedPanel].Visualiser = null;
            _DisplayPanel[_SelectedPanel].AnimationVisualiser = null;

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
                strTip = _AlgorithmRegistry.Details(algorithmLB.Items[nIdx].ToString()).Description;

                if (algorithmLB.Items[nIdx].ToString() != _prevAlgSelection) // prevent tooltip from flickering
                {
                    toolTip1.SetToolTip(algorithmLB, strTip);
                    _prevAlgSelection = algorithmLB.Items[nIdx].ToString();
                }
            }

        }

        #endregion
    }
}
