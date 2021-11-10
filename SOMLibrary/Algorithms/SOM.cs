using System;
using System.Diagnostics.Contracts;

using MathLib;
using MathLib.Matrices;
using SomLibrary.NeuronMaps;
using Util;

namespace SomLibrary.Algorithms
{
	[SOMPluginDetail("SOM", "The Original self-organising map")]    
	[Serializable]
	public partial class SOM : ISOM
	{

		#region Fields (6) 

	    [NonSerialized] private IInputLayer _input;
		private bool _isTraining;
		private INeuronMap _mapping;       
		private int _updateInterval = 100;

		#endregion Fields 

		#region Constructors (1) 

		public SOM()
		{
			// initialise parameters to something reasonably sensible
			Alpha = 0.4;
			AlphaDecayFactor = 0.9995;
			MaxEpoch = 5;
			Sigma = 20;
			SigmaDecayFactor = 0.999;
			_mapping = new RectNeuronMap(2, 20, 20);        
		}

		#endregion Constructors 

		#region Properties (3) 

		public IInputLayer InputReader
		{
			set { _input = value; }
			get { return _input; }
		}

		public INeuronMap Map
		{
			get { return _mapping; }
			set { _mapping = value; }
		}

		public int ProgressInterval
		{
			get { return _updateInterval; }
			set { _updateInterval = value; }
		}

		#endregion Properties 

		#region Delegates and Events (1) 


		// Events (1) 

		[field: NonSerialized] public event EventHandler ProgressUpdate;


		#endregion Delegates and Events 

		#region Methods (9) 


		// Public Methods (5) 

		public void CancelTraining()
		{
			_isTraining = false;
		}
	    
		public int Simulate(Vector x, out double error)
		{
			int c = 0;
		    double minDist = int.MaxValue;

		    for (int i = 0; i < _mapping.MapSize; i++)
		    {
		        double dist = (_mapping[i] - x).Norm;
		        if (dist >= minDist) continue;
		        c = i;
		        minDist = dist;
		    }

		    error = minDist;
			return c;
		}

		/// <summary>
		/// Train SOM
		/// </summary>        
		public void Train()
		{
			int epoch;
		    int updateOnIter = _updateInterval;
			int currentIter = 1;

		    _isTraining = true;

			for (epoch = 0; epoch < m_MaxEpoch; epoch++)
			{
				foreach (Vector x in _input)
				{
					if (currentIter == updateOnIter)  // update event (for gui)
					{
						OnProgressUpdate(new EventArgs());
						updateOnIter += _updateInterval;
						//System.Threading.Thread.Sleep(1000);
					}

					if (!_isTraining)    // abort algorithm if isTraining is set to false;
						break;

                    // Contract.Assume(x != null);
					// find winning neuron                   
				    double error;
				    int c = Simulate(x, out error);  // index of winning node                       
					
					UpdateWeights(x, c);        //****** 

					AdjustParameters();  // adjust parameters

					currentIter++;
				}
				if (!_isTraining)
					break;

			}
		}



		// Private Methods (4) 

		[ToDo] // this should probably use some precalculated lookup table (but i'm feeling lazy)        
		private double NeighbourhoodFn(double distance)
		{
			return (Math.Exp(-distance * distance / m_SigmaSquared));                                
		}

		private void OnProgressUpdate(EventArgs e)
		{
			if (ProgressUpdate != null)
				ProgressUpdate(this, e);
		}

		private void AdjustParameters()
		{
			m_SigmaSquared *= m_RSigmaSquared;
			m_Alpha *= m_Ralpha;        
		}

		/// <summary>      
		/// Neighbourhood of the winning neuron is bounded by a 2n sided regular polytope (where n 
		/// is the map dimensionality)
		/// </summary>
		/// <param name="x"></param>
		/// <param name="winner"></param>                
		protected virtual void UpdateWeights(Vector x, int winner)
		{
			int kernel = 0; // nieghbourhood size
			double h = 1; // result of neighbourhood function
			int [] neighIndices;

			while (h > Constants.Epsilon)
			{
				neighIndices = _mapping.Neighbours(winner, kernel);
				if (kernel == _mapping.MapSize || neighIndices.Length == 0) // fail safe check
					break;

				h = NeighbourhoodFn(kernel);				
				foreach (int n in neighIndices)
					_mapping[n] += (m_Alpha * h * (x - _mapping[n]));
				kernel++;

			} 
		}       

		#endregion Methods 

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            // Contract.Invariant(_mapping != null);
        }

	}
}
