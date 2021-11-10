using System;
using System.ComponentModel;
using System.Threading;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SomLibrary;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Collections.ObjectModel;

using Util;

namespace SOMSimulator
{
    class ExecutionManager : BackgroundWorker
    {      
        private ISOM algorithm;
        private ReadOnlyCollection<DisplayInfo> _displayAreas;

        public AutoResetEvent updateEvent;
        
        public ExecutionManager(IList<DisplayInfo> displayDetails)
        {
            // Contract.Requires<ArgumentNullException>(displayDetails != null);
            // Contract.Requires<ArgumentException>(// Contract.ForAll<DisplayInfo>(displayDetails, d => d != null));

            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
            updateEvent = new AutoResetEvent(false);
            this._displayAreas = new ReadOnlyCollection<DisplayInfo>(displayDetails);            
        }

        public ISOM Algorithm
        {
            get { return algorithm; }
            set
            {
                // Contract.Requires<ArgumentNullException>(value != null);
                
                if (this.IsBusy)
                    throw new InvalidOperationException("Can't change algorithm in the " +
                        "middle of training");
                
                algorithm = value;
                // Setup event handler                
                algorithm.ProgressUpdate += new EventHandler(DrawVisual);
            }
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            // Contract.Assume(// Contract.ForAll<DisplayInfo>(_displayAreas, d => d != null));

            if (algorithm == null)
                throw new InvalidOperationException("A valid algorithm must be specified");

            //train network if algorithm is in a trainable state
            if (algorithm.InputReader != null && algorithm.Map != null &&
                algorithm.InputReader.InputDimension == algorithm.Map.InputDimension)
                algorithm.Train();            

            base.OnDoWork(e);

            // Contract.Assume(e != null);
            e.Result = true;
            if (CancellationPending)
            {
                e.Cancel = true;
            }
        }
       
        // executes on the worker thread (Called by algorithm periodically)
        [ToDo("calculate percentage for progress report")]
        private void DrawVisual(object sender, EventArgs e)
        {
            if (CancellationPending)
            {
                if (algorithm != null) // should always be the case!
                    algorithm.CancelTraining();
                //return;
            }

            // Visualiser needs to be synchronized with main thread.
            // Lock contention will probably only happen when algorithm
            // is executing and application gets resized.
            foreach (DisplayInfo d in _displayAreas)
            {
                // Contract.Assume(d != null);
                // Contract.Assume(algorithm != null);
                d.DrawMapVisual(algorithm);                
            }
            
            this.ReportProgress(0);
            updateEvent.WaitOne(); // suspend so that UI can update itself
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            // Contract.Invariant(_displayAreas != null);
            // Contract.Invariant(updateEvent != null);
        }
    }
}
