using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.Contracts;

using MathLib.Matrices;
using Util;

namespace SomLibrary.Algorithms
{
	//[SOMPluginDetail("CPSOM", "Cellular probabilistic SOM: developed by Tommy Chow.")]
	public partial class CPSOM //: ISOM
	{/*
		#region Fields (9) 

		protected const int DefaultDimension = 8;
		protected double m_Beta;
		private double m_Gamma;
		protected IInputLayer m_Input;
		protected bool m_IsTraining;
		protected NeuronMap m_Mapping;
		protected CPSOMNodeData[] m_NodeData;
		private int m_UpdateInterval = 100;
		protected const int MaxKernelSize = 3;

		#endregion Fields 

		#region Properties (3) 

		public IInputLayer InputReader
		{
			set
			{
				m_Input = value;
			}

			get { return m_Input; }
		}

		public NeuronMap Map
		{
			get { return m_Mapping; }
			set
			{
				m_Mapping = value;
			}
		}

		public int ProgressInterval
		{
			get { return m_UpdateInterval; }
			set
			{
				m_UpdateInterval = value;
			}
		}

		#endregion Properties 

		#region Delegates and Events (1) 


		// Events (1) 

		public event EventHandler ProgressUpdate;


		#endregion Delegates and Events 

		#region Methods (10) 


		// Public Methods (5) 

		public void CancelTraining()
		{
			m_IsTraining = false;
		}

		public int Simulate(Vector x, out double error)
		{


			Vector assignmentProps = this.EStep(x);
			
			// find neuron with greatest assignment probability
			double maxProb = -1;
			int winner = 1;

			for (int i = 1; i <= Map.MapSize; i++)                
			{
				if (m_NodeData[i].Probability > maxProb)
				{
					maxProb = m_NodeData[i].Probability;
					winner = i;
				}                
			}
			error = (m_Mapping[winner] - x).Norm;
			return winner;
		}

		/// <summary>
		/// Train CPSOM
		/// </summary>
		public void Train()
		{
			int epoch;

			int iter = 1;
			int updateOnIter = 1;
			m_Beta = BetaStart;
			m_Gamma = GammaStart;

			// check whether algorithm is in valid state
			if (m_Input == null)
				throw new InvalidOperationException("No input source set");
			if (m_Mapping == null)
				throw new InvalidOperationException("No map set");
			if (m_Input.InputDimension != m_Mapping.InputDimension)
				throw new InvalidOperationException("Mismatch between input vector length " +
					"and map weight vector lengths");

			// allocate m_NodeData
			m_NodeData = new CPSOMNodeData[m_Mapping.MapSize];
			for (int i = 0; i < m_Mapping.MapSize; i++)
				m_NodeData[i] = new CPSOMNodeData();

			m_IsTraining = true;            

			CalcNeighbourhood();
			for (epoch = 0; epoch < m_MaxEpoch; epoch++)
			{
				foreach (Vector x in m_Input)
				{
					if (iter == updateOnIter)
					{
						OnProgressUpdate(new EventArgs());
						updateOnIter += m_UpdateInterval;                        
					}

					if (!m_IsTraining)
						break;

					EStep(x);

					MStep(x);                    

					PostProcessing(iter);

					iter++;
				}
				if (!m_IsTraining)
					break;
			}          
		}



		// Protected Methods (5) 
        /*
		[ToDo("Normalisation isn't quite correct. produces contracted maps")]
		protected virtual void CalcNeighbourhood()
		{
			double [] h = new double [MaxKernelSize+1];
			double normFactor = 0;
			int kernel;
			
			for (kernel = 0; kernel <= MaxKernelSize; kernel++)            
				h[kernel] = Math.Exp(-Math.Pow(kernel, 2) / (2*m_Sigma*m_Sigma));                
					   
			double[] tmpH;
			int [] tmpNeighbours;
			for (int i = 1; i <= m_Mapping.MapSize; i++)
			//for (int nodeID = 0; nodeID < nodeData.Length; nodeID++)
			{
				normFactor = h[0];
				for (int s = 1; s <= MaxKernelSize; s++)
				{
					tmpNeighbours = m_Mapping.Neighbours(i, s);
					normFactor += (tmpNeighbours.Length * h[s]);
				}
				tmpH = new double[h.Length];
				for (int j = 0; j < tmpH.Length; j++)
					tmpH[j] = h[j] / normFactor;
				
				m_NodeData[i-1].SetNeighbourhoodFn(tmpH);				
			}                        
		}
       
		protected Vector EStep(Vector x)
		{
			// Contract.Requires(x != null);
			// Contract.Requires(x.Length == Map.InputDimension);
            // Contract.Requires(x.Orientation == VectorType.RowVector);
            // Contract.Ensures(// Contract.Result<Vector>().Length == m_Mapping.InputDimension);

			double normFactor = double.Epsilon;         // make sure this is nonzero
			double tmp;
			int kernel;   // neighbourhood kernel size;   
			Vector assignmentProbs = new Vector(m_Mapping.MapSize);
			Vector D = new Vector(m_Mapping.MapSize);

            for (int i = 0; i < Map.MapSize; i++)
                D[i] = (x - Map[i]).NormSquared / 2;			
				
			for (int i = 1; i <= m_Mapping.MapSize; i++)                
			{                
				tmp = 0;
				for (kernel = 0; kernel <= MaxKernelSize; kernel++)
					foreach (int j in Map.Neighbours(i,kernel))
						tmp += m_NodeData[i-1].H(kernel) * D[i];
				
				assignmentProbs[i] = Math.Exp(-m_Beta * tmp / 2);

				normFactor += assignmentProbs[i];
			}
			
			// normalise probability assignment
			for (int i = 1; i <= Map.MapSize; i++)
				assignmentProbs[i] /= normFactor;

			return assignmentProbs;
		}

		protected virtual void MStep(Vector x)
		{
			// Contract.Requires(x != null);
			// Contract.Requires(x.Length == Map.InputDimension);

			double tmp;           

			for (int i = 1; i <= Map.MapSize; i++)
			{
				tmp = 0;
				for (int size = 0; size <= MaxKernelSize; size++)                   
					foreach (int n in m_Mapping.Neighbours(i,size))
						tmp += m_NodeData[n-1].H(size) * m_NodeData[n-1].Probability;
								   
				m_NodeData[i-1].B += tmp;

				Map[i] += (tmp * (x-Map[i]) / m_NodeData[i-1].B);  
			}                        
		}

		protected virtual void OnProgressUpdate(System.EventArgs e)
		{
			if (ProgressUpdate != null)
				ProgressUpdate(this, e);
		}

		protected virtual void PostProcessing(int iter)
		{
			if (iter % m_Lambda1 == 0)
				if (m_Beta < m_BetaFinal)
					m_Beta += m_Delta1;
				else
					m_Beta = m_BetaFinal;

			if (iter % m_Lambda2 == 0)
			{
				if (m_Gamma > m_GammaFinal)
					m_Gamma -= m_Delta2;
				else
					m_Gamma = m_GammaFinal;

				for (int i = 1; i <= Map.MapSize; i++)                
					m_NodeData[i-1].B /= m_Gamma;                
			}

			if (iter % m_Lambda3 == 0) // forgetting effect
			{
				m_Gamma = m_GammaStart;                
			}
		}


		#endregion Methods 

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            // Contract.Invariant(m_NodeData != null);
            // Contract.Invariant(m_Mapping != null);
            // Contract.Invariant(// Contract.ForAll<CPSOMNodeData>(m_NodeData, node => node != null));

        }
      * */
    }
}
