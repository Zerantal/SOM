using System;
using System.Collections.Generic;
using System.Text;
using Util;
using MathLib.Matrices;

namespace SomLibrary.Algorithms
{
    public partial class GCPSOM //: ISOM
    {
        /*
		#region Fields (11) 

        private const int GammaAdjustmentInterval = 100;
        private double m_Gamma;
        private double m_GT;
        private double[] m_H;
        private double m_Herr = 0;
        protected IInputLayer m_Input;
        protected bool m_IsTraining;
        private GrowingRectNeuronMap m_Map;
        protected NeuronMap m_Mapping;
        protected CPSOMNodeData[] m_NodeData;
        private int m_UpdateInterval = 100;
        protected const int MaxKernelSize = 3;
        private double m_GammaDecrement;       

		#endregion Fields 

		#region Constructors (1) 

        public GCPSOM() : base()
        {
            ResetParameters();

            m_Map = new GrowingRectNeuronMap(2, 20, 20);
         }

		#endregion Constructors 

		#region Properties (3) 

        public IInputLayer InputReader
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                m_Input = value;
            }

            get { return m_Input; }
        }

        public NeuronMap Map
        {
            get { return m_Mapping; }
            set
            {
                if (value == null)
                    throw new ArgumentException("Invalid map set");
                m_Mapping = value;
            }
        }

        public int ProgressInterval
        {
            get { return m_UpdateInterval; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", "value has to be greater than or equal to one");

                m_UpdateInterval = value;

            }
        }

		#endregion Properties 

		#region Delegates and Events (1) 


		// Events (1) 

        public event EventHandler ProgressUpdate;


		#endregion Delegates and Events 

		#region Methods (15) 


		// Public Methods (7) 

        public void CancelTraining()
        {
            m_IsTraining = false;
        }

        public CPSOMNodeData GetData(Neuron n)
        {
            return m_NodeData[n.Index];
        }

        public void GrowNodes()
        {
            double maxProbAssignment = -1;
            Neuron c = null;
            List<Neuron> newNodes;

            // find "winning" neuron
            foreach (Neuron i in Map)
            {
                if (m_NodeData[i.Index].Probability > maxProbAssignment)
                {
                    maxProbAssignment = m_NodeData[i.Index].Probability;
                    c = i;
                }
            }


            // Grow new nodes if needed
            GRectNodeData d = m_Map.GetData(c);
            d.HitCount++;
            //if (nodeData[c.Index].B > 15)
            {

                d.Error += m_NodeData[c.Index].D;
                if (d.Error > m_Herr)
                {
                    m_Herr = d.Error;
                    //if (d.Error > GT && mapping.MapSize <= 100)
                    if (m_Herr > m_GT)
                    {
                        if (m_Map.IsBoundary(c) && m_Map.Neighbours(c,1).Count >= 2 && m_Mapping.MapSize <= 256)
                        {
                            newNodes = m_Map.GrowNodes(c);
                            PopulateNewNodeData(c, newNodes);
                            d.HitCount = 0;
                            d.Error = 0;
                        }
                        else
                        {
                            m_Map.RedistributeError(c);
                            m_Herr /= FindMaxError();
                            //d.HitCount = 0;
                        }
                    }
                }
            }

        }

        public void InitialiseMap()
        {/*
            m_Mapping = new GrowingRectNeuronMap(m_Input.InputDimension);
            m_Map = (GrowingRectNeuronMap) m_Mapping;
            m_NodeData = new CPSOMNodeData[m_Map.MaxMapSize];

            // Initialise weights
            Init.Random(m_Mapping);

            foreach (Neuron n in m_Mapping)
                m_NodeData[n.Index] = new CPSOMNodeData();
           *
        }

        public void ResetParameters()
        {           
            m_Lambda = 2000;
            m_GammaStart = 10;          

            m_Sigma = 0.5;

            //m_BetaStart = 100;
            m_Beta = 1000;
            m_SF = 0.01;
            m_FD = 0.3;
            if (m_Input != null)
                m_GT = -m_Input.InputDimension * Math.Log(m_SF);

            m_GammaDecrement = (m_GammaStart - 1) / 2 / m_Lambda * 3 * GammaAdjustmentInterval;
            
        }

        public virtual Neuron Simulate(Vector x, out double squaredError)
        {
            this.EStep(x);

            // find neuron with greatest assignment probability
            Neuron winner = null;
            double maxProb = -1;

            foreach (Neuron n in Map)
            {
                if (m_NodeData[n.Index].Probability > maxProb)
                {
                    maxProb = m_NodeData[n.Index].Probability;
                    winner = n;
                }
            }

            squaredError = m_NodeData[winner.Index].D;
            return winner;
        }

        /// <summary>
        /// Train CPSOM
        /// </summary>
        public void Train()
        {
            int epoch, updateOnIter;

            int iter = 1;            
            updateOnIter = 1;            
            m_Gamma = m_GammaStart;           

            // check whether algorithm is in valid state
            if (m_Input == null)
                throw new InvalidOperationException("No input source set");
            if (m_Mapping == null)
                throw new InvalidOperationException("No map set");
            if (m_Input.InputDimension != m_Mapping.WeightDimension)
                throw new InvalidOperationException("Mismatch between input vector length " +
                    "and map weight vector lengths");
            
            m_IsTraining = true;
            m_GT = -m_Input.InputDimension * Math.Log(m_SF);
            m_Herr = 0;
            
            CalcNeighbourhood();
            for (epoch = 0; epoch < m_MaxEpoch; epoch++)
            {
                foreach (Vector x in m_Input)
                {
                    if (iter == updateOnIter)
                    {
                        OnProgressUpdate(new EventArgs());
                        updateOnIter += ProgressInterval;                        
                    }

                    if (!m_IsTraining)    // abort algorithm if isTraining is set to false;
                        break;

                    EStep(x);

                    MStep(x);

                   GrowNodes();        // new addition to algorithm

                    PostProcessing(iter);

                    iter++;
                }
                if (!m_IsTraining)
                    break;
            }            
        }



		// Protected Methods (6) 

        protected void CalcNeighbourhood()
        {
            m_H = new double[MaxKernelSize + 1];
            double [] normalisedH;
            double normFactor = 0;
            int size;

            for (size = 0; size <= MaxKernelSize; size++)            
                m_H[size] = Math.Exp(-size * size / (2 * m_Sigma * m_Sigma));                                            

            // load neighbourhood funciton profile into starting node data and normalise
            normalisedH = (double[])m_H.Clone();
            foreach (Neuron n in m_Mapping)
            { // this assumes four starting neurons arranged in a square
                normFactor = normalisedH[0] + 3 * normalisedH[1];
                for (size = 0; size <= MaxKernelSize; size++)
                    normalisedH[size] /= normFactor;

                m_NodeData[n.Index].SetNeighbourhoodFn((double [])normalisedH.Clone());

            }
        }

        protected virtual void EStep(Vector x)
        {
            double normFactor = double.Epsilon;         // make sure this is nonzero
            double tmp;
            int kernel;   // neighbourhood kernel size;            

            foreach (Neuron n in m_Mapping)
            {
                m_NodeData[n.Index].D = (x - n.W).SquaredNorm / 2;
            }

            foreach (Neuron i in Map)
            {
                tmp = 0;
                for (kernel = 0; kernel <= MaxKernelSize; kernel++)
                    foreach (Neuron j in m_Mapping.Neighbours(i, kernel))
                        tmp += m_NodeData[i.Index].H(kernel) * m_NodeData[j.Index].D;

                m_NodeData[i.Index].Probability = Math.Exp(-m_Beta * tmp / 2);

                normFactor += m_NodeData[i.Index].Probability;
            }

            // normalise probability assignment
            foreach (Neuron n in Map)
            {
                m_NodeData[n.Index].Probability /= normFactor;
            }
        }

        protected virtual void MStep(Vector x)
        {
            double tmp;

            foreach (Neuron i in Map)
            {
                tmp = 0;
                for (int size = 0; size <= MaxKernelSize; size++)
                    foreach (Neuron n in m_Mapping.Neighbours(i, size))
                        tmp += m_NodeData[n.Index].H(size) * m_NodeData[n.Index].Probability;

                m_NodeData[i.Index].B += tmp;

                if (m_NodeData[i.Index].B == 0)
                    m_NodeData[i.Index].B = Double.Epsilon;  //fail safe

                i.W += (tmp * (x - i.W) / m_NodeData[i.Index].B);
            }
        }

        protected virtual void OnProgressUpdate(System.EventArgs e)
        {
            if (ProgressUpdate != null)
                ProgressUpdate(this, e);
        }

        protected virtual void PopulateNewNodeData(Neuron parentNode, List<Neuron> newNodes)
        {
            double normFactor;
            int kernel;
            List<Neuron> neighbours;

            foreach (Neuron n in newNodes)
            {
                normFactor = m_H[0];
                for (kernel = 1; kernel <= MaxKernelSize; kernel++)
                {
                    neighbours = m_Mapping.Neighbours(n, kernel);
                    normFactor += (neighbours.Count * m_H[kernel]);
                    foreach (Neuron n2 in neighbours)
                    {
                        if (m_NodeData[n2.Index] != null)
                        {
                            m_NodeData[n2.Index].NormFactor += m_H[kernel];
                            // The following line is is a hack
                            m_NodeData[n2.Index].Renormalise();                           
                        }                        
                    }
                }
                m_NodeData[n.Index] = new CPSOMNodeData();
                m_NodeData[n.Index].SetNeighbourhoodFn((double [])m_H.Clone());
                m_NodeData[n.Index].NormFactor = normFactor;
                m_NodeData[n.Index].Renormalise();

                //nodeData[n.Index].B = 0;

                //nodeData[parentNode.Index].B = 0;             
                m_NodeData[n.Index].B = m_NodeData[parentNode.Index].B;
            }
            
            // see how this goes                
            //for (size = 1; size <= 2; size++)
              //  foreach (Neuron i in mapping.Neighbours(parentNode, size))
                //    nodeData[i.Index].B /= (8 * nodeData[parentNode.Index].H(size) );
            /*
            nodeData[parentNode.Index].B = 0;
            beta /= 2;*
            //m_NodeData[parentNode.Index].B *= 9;
        }

        private double FindMaxError()
        {
            GRectNodeData nodeData;
            double maxError = 0;

            foreach (Neuron n in m_Mapping)
            {
                nodeData = m_Map.GetData(n);
                if (nodeData.Error > maxError)
                    maxError = nodeData.Error;
            }

            return maxError;
        }

        [TODO]
        protected void PostProcessing(int iter)
        {
            if (iter % GammaAdjustmentInterval == 0)
            {
                m_Gamma -= m_GammaDecrement;                
                if (m_Gamma < 1)
                    m_Gamma = 1;

                foreach (Neuron n in Map)
                    m_NodeData[n.Index].B /= m_Gamma;

/*                if (m_Beta >= 1000)
                    m_Beta = 100;
                else
                    m_Beta += 67.5;*
            }            
             
            if (iter % m_Lambda == 0) // forgetting effect
            {
                m_Gamma = m_GammaStart;
                //GrowNodes();
            }

            /*
            if (iter % lambda4 == 0)
            {                
                List<int> removalIndices = new List<int>();
                double upperThreshold = 0;       // == x * std (say, x = 2 initially)
                double lowerThreshold = 0;
                double[] vals = new double[mapping.MapSize];
                double[] neigh = new double[mapping.MapSize];
                int i = 0;
                Neuron[] neighbours;

                foreach (Neuron n in _map)
                {
                    vals[i] = MeanNeighbourDistance(n);
                    neigh[i++] = _map.Neighbours(n, 1).Length;

                }

                double stdDev = MathExtra.StdDev(vals);
                double av = MathExtra.Average(vals);
                upperThreshold = av + 2 * stdDev;
                lowerThreshold = av - 2 * stdDev;

                i = 0;
                foreach (Neuron n in _map)
                {
                 
                    if ((vals[i] < lowerThreshold /4*neigh[i] || vals[i] > upperThreshold*4/neigh[i]) && _map.IsBoundary(n))
                        removalIndices.Add(n.Index);
                    i++;                                        
                }

                foreach (int idx in removalIndices)
                    _map.RemoveNeuron(idx);
            }*/
            
            /*
            if (iter % m_Lambda4 == 0) // prune nodes
            {
                GRectNodeData nodeData;
                List<int> removalIndices = new List<int>();
                List<Neuron> neighbours;

                foreach (Neuron n in m_Map)
                {
                    nodeData = m_Map.GetData(n);
                    neighbours = m_Mapping.Neighbours(n, 1);
                    if (neighbours.Count <= 1 || nodeData.HitCount == 0)   // delete neuron
                        removalIndices.Add(n.Index);
                    else nodeData.HitCount = 0;
                }
                // remove nodes
                foreach (int idx in removalIndices)
                    m_Map.RemoveNeuron(idx);

                //growingSuspended = true;
            }*

            //if (iter % lambda5 == 0)
              //  growingSuspended = false;
        }



		// Private Methods (2) 

        private double MaxNeighbourDistance(Neuron n)
        {
            double maxDist = 0;
            List<Neuron> neighbours;

            neighbours = m_Mapping.Neighbours(n, 1);
            double dist;
            foreach (Neuron neigh in neighbours)
            {
                dist = (n.W - neigh.W).Norm;
                if (dist > maxDist)
                    maxDist = dist;
            }

            return maxDist;
        }

        private double MeanNeighbourDistance(Neuron n)
        {
            double meanDist = 0;
            List<Neuron> neighbours;

            neighbours = m_Mapping.Neighbours(n, 1);
            foreach (Neuron neigh in neighbours)
                meanDist += (n.W - neigh.W).Norm;

            meanDist /= neighbours.Count;

            return meanDist;
        }


		#endregion Methods 
    */
    }
}
