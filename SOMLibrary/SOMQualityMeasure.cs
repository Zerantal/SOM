using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Diagnostics.Contracts;

using MathLib.Matrices;

namespace SomLibrary
{
    public class SOMQualityMeasure
    {
        #region Public methods

        public static double QuantisationError(ISOM alg, FileInputLayer input, RectRegion region,
            int numTrialVectors = 5000)
        {
            double error = 0;
            IEnumerator<Vector> inputEnum = input.RandomEnumerator;
            int vecCount = 0;

            for (int i = 0; i < numTrialVectors; i++)
            {
                if (!inputEnum.MoveNext())
                {                   
                    break;
                }

                Vector v = inputEnum.Current;

                if (v[0] < region.XMin || v[0] > region.XMax || v[1] < region.YMin || v[1] > region.YMax) continue;
                double tmp;
                alg.Simulate(v, out tmp);
                error += tmp;
                vecCount++;
            }
            error /= vecCount;

            return error;
        }

        public static double QuantisationError(ISOM alg, FileInputLayer input, int numTrialVectors = 5000)
        {
            return QuantisationError(alg, input, new RectRegion(), numTrialVectors);
        }

        public static double TopologicalError(INeuronMap neuronMap, FileInputLayer input, RectRegion region, int numTrialVectors = 5000)
        {
            List<Vector> trialVectors = new List<Vector>();
            IEnumerator<Vector> iter = input.RandomEnumerator;
            double topError = 0;
            int sbmu = 0;       // second best matching unit            

            for (int i = 0; i < numTrialVectors; i++)
            {
                if (iter.MoveNext())
                {
                    Vector tmp = iter.Current;
                    if (tmp[0] >= region.XMin && tmp[0] <= region.XMax && 
                        tmp[1] >= region.YMin && tmp[1] <= region.YMax)
                    {
                        trialVectors.Add(tmp);
                    }
                }
                else
                    break;
            }

            int[] bmuNeighbours;
            foreach (Vector v in trialVectors)
            {
                double minError = double.MaxValue;
                int bmu = -1;        // best matching unit
                // calculate bmu and sbmu
                double e;
                for (int n = 0; n < neuronMap.MapSize; n++)
                {
                    e = (v - neuronMap[n]).NormSquared;
                    if (e >= minError) continue;
                    minError = e;
                    sbmu = bmu;
                    bmu = n;
                }

                // find sbmu explicitly if bmu found first time round (i.e., sbmu == -1)
                minError = double.MaxValue;
                if (sbmu == -1)
                {
                    for (int n = 0; n < neuronMap.MapSize; n++)
                    {
                        e = (v - neuronMap[n]).NormSquared;
                        if (e < minError && n != bmu)
                        {
                            minError = e;
                            sbmu = n;
                        }
                    }
                }

                // determine if bmu and sbmu are adjacent
                bmuNeighbours = neuronMap.Neighbours(bmu, 1);
                if (!bmuNeighbours.Contains(sbmu))
                    topError += 1;
            }
            topError /= trialVectors.Count();

            return topError;
        }

        public static double TopologicalError(INeuronMap neuronMap, FileInputLayer input, int numTrialVectors = 5000)
        {
            return TopologicalError(neuronMap, input, new RectRegion(), numTrialVectors);
        }

        public static double Psi(INeuronMap neuronMap, RectRegion region, FileInputLayer input = null)
        {
            SparseMatrix<int> c;

            if (input == null)
                c = ConnectivityMatrix(neuronMap);      // from voronoi regions
            else
                c = MaskedConnectivityMatrix(input, neuronMap); // from input data

            double error = 0;
            SparseVector<int> w;

            List<int> mapNodesInCalc = new List<int>();   // register of map nodes to use in calculation
            for (int j = 0; j < neuronMap.MapSize; j++)
            {
                Vector v = neuronMap[j];
                if (v[0] >= region.XMin && v[0] <= region.XMax && 
                    v[1] >= region.YMin && v[1] <= region.YMax)
                    mapNodesInCalc.Add(j);
            }

            foreach (int j in mapNodesInCalc)
            {
                w = c.GetRow(j);
// ReSharper disable AccessToModifiedClosure
                double tmp = w.ValueEnumerator.Sum(element => (neuronMap.NeuronPosition(j) - (neuronMap.NeuronPosition(element.Item1))).OneNorm);
// ReSharper restore AccessToModifiedClosure
                if (w.NumberOfNonzeroElements != 0)
                    error += (tmp / w.NumberOfNonzeroElements);
            }
            error /= mapNodesInCalc.Count();

            return error;
        }

        public static double Psi(INeuronMap neuronMap, FileInputLayer input = null)
        {
            return Psi(neuronMap, new RectRegion(), input);
        }

        public static double PhiBar(INeuronMap neuronMap, FileInputLayer input, RectRegion region)
        {
            List<Tuple<int, double>> phi = Phi(neuronMap, input, region, -1, 1);

            double result = phi.Find(                      // find phi(1)
                t => t.Item1 == 1
                ).Item2;

            result = result - phi.Find(             // subtract phi(-1)
                                  t => t.Item1 == -1).Item2;

            return result;
        }

        public static double PhiBar(INeuronMap neuronMap, FileInputLayer input)
        {
            return PhiBar(neuronMap, input, new RectRegion());
        }

        public static List<Tuple<int, double>> Phi(INeuronMap neuronMap, FileInputLayer input, RectRegion region, 
            int kMin = 0, int kMax = 0)
        {
            // Contract.Requires(kMin <= 0 && kMax >= 0);

            List<Tuple<int, double>> result = new List<Tuple<int, double>>();

            PhiData data = new PhiData(neuronMap, input, region, -kMin);    // pre-calc data to be used in subsequent calculations      

            if (kMin < 0)
                result.AddRange(PhiNeg(data, kMin, -1));

            result.Add(new Tuple<int, double>(0, PhiZero(data)));

            if (kMax > 0)
                result.AddRange(PhiPos(data, 1, kMax));

            return result;
        }

        public static List<Tuple<int, double>> Phi(INeuronMap neuronMap, FileInputLayer input,
            int kMin = 0, int kMax = 0)
        {
            return Phi(neuronMap, input, new RectRegion(), kMin, kMax);
        }

        public static double TopographicProduct(INeuronMap map, RectRegion region)
        {
            double result = 0;

            // inputNeughbours[i,j] indicates the index of the jth nearest neighbour to i
            // as measured on the input space
            Matrix<int> inputNeighbours = new Matrix<int>(map.MapSize, map.MapSize);

            // outputNeughbours[i,j] indicates the index of the jth nearest neighbour to i
            // as measured on the output lattice
            Matrix<int> outputNeighbours = new Matrix<int>(map.MapSize, map.MapSize);

            List<KeyValuePair<double, int>> inputSpaceDistances = new List<KeyValuePair<double, int>>(map.MapSize);
            List<KeyValuePair<double, int>> outputSpaceDistances = new List<KeyValuePair<double, int>>(map.MapSize);
            List<int> mapNodesInCalc = new List<int>();

            IOrderedEnumerable<KeyValuePair<double, int>> inSpaceDist;
            IOrderedEnumerable<KeyValuePair<double, int>> outSpaceDist;

            // rank distances on input space and output space between all pairs of neurons.
            // i.e., calculate inputNeighbours and outputNeighbours
            for (int i = 0; i < map.MapSize; i++)
            {
                Vector v = map[i];
                if (v[0] < region.XMin || v[0] > region.XMax || v[1] < region.YMin || v[1] > region.YMax) continue;
                mapNodesInCalc.Add(i);

                inputSpaceDistances.Clear();
                outputSpaceDistances.Clear();
                for (int j = 0; j < map.MapSize; j++)
                {
                    inputSpaceDistances.Add(new KeyValuePair<double, int>(
                                                (map[i] - map[j]).NormSquared, j));
                    outputSpaceDistances.Add(new KeyValuePair<double, int>(
                                                 (map.NeuronPosition(i) - map.NeuronPosition(j)).NormSquared, j));
                }
                inSpaceDist = inputSpaceDistances.OrderBy(kvp => kvp.Key);
                outSpaceDist = outputSpaceDistances.OrderBy(kvp => kvp.Key);
                inputNeighbours.SetRow(i, new Vector<int>(
                                              inSpaceDist.Select(kvp => kvp.Value).ToArray()));
                outputNeighbours.SetRow(i, new Vector<int>(
                                               outSpaceDist.Select(kvp => kvp.Value).ToArray()));
            }

            // calculate Topographic Product
            foreach (int j in mapNodesInCalc)
            {
                for (int k = 1; k <= map.MapSize - 1; k++)
                {
                    double tmp = 1;
                    for (int l = 1; l <= k; l++)
                    {
                        int outNeigh = outputNeighbours[j, l];   // represents l^th neighbout as measured on output space            
                        int inNeigh = inputNeighbours[j,l];    // represents l^th neighbour as measured on input space
                        tmp *= (((map[j] - map[outNeigh]).Norm) * (map.NeuronPosition(j) - map.NeuronPosition(outNeigh)).Norm) /
                            ((map[j] - map[inNeigh]).Norm * (map.NeuronPosition(j) - map.NeuronPosition(inNeigh)).Norm);
                    }
                    result += Math.Log10(Math.Pow(tmp, (1/(2*(double)k))));
                }
            }

            return (result / (mapNodesInCalc.Count * (map.MapSize - 1)));
        }

        public static double TopographicProduct(INeuronMap map)
        {
            return TopographicProduct(map, new RectRegion());
        }

        private static SparseMatrix<int> MaskedConnectivityMatrix(FileInputLayer input, INeuronMap map)
        {
            SparseMatrix<int> c = new SparseMatrix<int>(map.MapSize, map.MapSize);

            // Calculate connectivity matrix      
            IEnumerator<Vector> iter = input.RandomEnumerator;
            int numTrialVectors = 500 * map.MapSize;

            for (int i = 0; i < numTrialVectors; i++)
            {
                int bmu = 0;        // best matching unit
                int sbmu = 0;       // second best matching unit
                Vector v;
                if (iter.MoveNext())
                    v = iter.Current;
                else
                    break;

                double minError = double.MaxValue;
                // calculate bmu
                for (int n = 0; n < map.MapSize; n++)
                {
                    double e = (v - map[n]).NormSquared;
                    if (e >= minError) continue;
                    minError = e;
                    sbmu = bmu;
                    bmu = n;
                }
                if (sbmu == 0) continue;
                c[bmu, sbmu] = 1;
                c[sbmu, bmu] = 1;
            }

            return c;
        }

        #endregion

        #region Helper methods
        internal class PhiData
        {
            private readonly Matrix _delaunayDistances;
            private readonly Matrix _maxNorms;
            private readonly INeuronMap _map;

            private readonly SparseMatrix<int> _c;
            public List<int> MapNodesInCalc;

            // pre-calculate all distances
            internal PhiData(INeuronMap map, FileInputLayer input, RectRegion region, int kMax)
            {
                // Contract.Requires<ArgumentNullException>(map != null);
                // Contract.Requires<ArgumentNullException>(input != null);
                // Contract.Requires<ArgumentOutOfRangeException>(kMax >= 0);

                _map = map;

                MapNodesInCalc = new List<int>();
                for (int i = 0; i < map.MapSize; i++)
                {
                    Vector v = map[i];
                    if (v[0] >= region.XMin && v[0] <= region.XMax &&
                        v[1] >= region.YMin && v[1] <= region.YMax)
                        MapNodesInCalc.Add(i);
                }

                _maxNorms = new Matrix(_map.MapSize, _map.MapSize);
                _delaunayDistances = new Matrix(_map.MapSize, _map.MapSize);
                
                _c = MaskedConnectivityMatrix(input, map);

                for (int i = 0; i < _map.MapSize; i++)
                {
                    for (int j = 0; j < _map.MapSize; j++)
                    {
                        _maxNorms[i, j] = (_map.NeuronPosition(i) - _map.NeuronPosition(j)).InfinityNorm;                        
                        _delaunayDistances[i, j] = DelaunayDistance(i, j, kMax);
                    }
                }                   
            }

            // determine the graph path length between two nodes in the connectivity graph.
            // return Int.MaxValue if traversal distance is greater than kMax
            private double DelaunayDistance(int i, int j, int kMax)
            {
                int v = i;
                Queue<int> q = new Queue<int>();
                HashSet<int> visitedNodes = new HashSet<int>();
                int traversalDepth = 0;
                int nodesLeftInCurrentLevel = 1;
                int nodesInNextLevel = 0;
                bool destinationFound = false;

                q.Enqueue(i);
                while (v != j && q.Count > 0)
                {
                    v = q.Dequeue();
                    visitedNodes.Add(v);

                    foreach (Tuple<int, int> val in _c.GetRow(v).ValueEnumerator)
                    {
                        if (val.Item1 == j)
                        {
                            traversalDepth++;   // found distance
                            destinationFound = true;
                            break;
                        }
                        if (!visitedNodes.Contains(val.Item1))
                        {
                            q.Enqueue(val.Item1);
                            nodesInNextLevel++;
                        }
                    }

                    if (destinationFound)
                        break;
                    nodesLeftInCurrentLevel--;

                    if (nodesLeftInCurrentLevel == 0)
                    {
                        nodesLeftInCurrentLevel = nodesInNextLevel;
                        nodesInNextLevel = 0;
                        traversalDepth++;
                    }

                    if (traversalDepth > kMax)
                        return int.MaxValue;
                }

                return traversalDepth;                 
            }
           
            public INeuronMap Map { get { return _map; } }
            public Matrix MaxNorms { get { return _maxNorms; } }
            public Matrix DelaunayDistances { get { return _delaunayDistances; } }

            [ContractInvariantMethod]
// ReSharper disable UnusedMember.Local
            private void ObjectInvariant()
// ReSharper restore UnusedMember.Local
            {
                // Contract.Invariant(_map != null);
            }
        }

        private static IEnumerable<Tuple<int, double>> PhiNeg(PhiData data, int kMin, int kMax)
        {
            // Contract.Requires(kMin <= kMax);
            // Contract.Requires(kMin < 0 && kMax < 0);

            Tuple<int, double>[] result = new Tuple<int, double>[kMax-kMin+1];
            int kmax = -kMin;   // invert range for algorithm
            int kmin = -kMax;

            for (int k = kmax, kIdx = 0; k >= kmin; k--, kIdx++)
            {
                double count = 0;
                foreach (int i in data.MapNodesInCalc)
                {
                    for (int j = 0; j < data.Map.MapSize; j++)
                    {
                        if ((data.Map.NeuronPosition(i) - data.Map.NeuronPosition(j)).Norm <= 1.1)                            
                            if (data.DelaunayDistances[i, j] > k)
                                count++;
                    }
                }
                result[kIdx] = new Tuple<int, double>(-k, count / data.MapNodesInCalc.Count());
            }                  

            return result;
        }

        private static double PhiZero(PhiData data)
        {
            // Contract.Requires(data != null);

            double count = 0;            

            foreach (int i in data.MapNodesInCalc)
                for (int j = 0; j < data.Map.MapSize; j++)
                {                    
                    if (data.MaxNorms[i,j] > 1 && data.DelaunayDistances[i, j] == 1)
                        count++;
                    if ((data.Map.NeuronPosition(i) - data.Map.NeuronPosition(j)).Norm <= 1.1)
                        if (data.DelaunayDistances[i, j] > 1)
                            count++;
                }
            double result = count / data.MapNodesInCalc.Count();

            return result;                   
        }

        private static IEnumerable<Tuple<int, double>> PhiPos(PhiData data, int kMin, int kMax)
        {
            // Contract.Requires(data != null);
            // Contract.Requires(kMin <= kMax);
            // Contract.Requires(kMin > 0 && kMax > 0);

            Tuple<int, double>[] result = new Tuple<int, double>[kMax - kMin + 1];

            for (int k = kMin, kIdx = 0; k <= kMax; k++, kIdx++)
            {
                double count = 0;
                foreach (int i in data.MapNodesInCalc)
                    for (int j = 0; j < data.Map.MapSize; j++)
                    {
                        if (data.MaxNorms[i, j] > k && data.DelaunayDistances[i, j] == 1)
                            count++;
                    }
                result[kIdx] = new Tuple<int, double>(k, count / data.MapNodesInCalc.Count());
            }
            return result;         
        }



        // Uses explicitly calculated voronoi region of mapping
        private static SparseMatrix<int> ConnectivityMatrix(INeuronMap neuronMap)
        {
            StringBuilder qdelaunayInput = new StringBuilder();
            Process qdelaunayProcess = new Process();
            SparseMatrix<int> c = new SparseMatrix<int>(neuronMap.MapSize, neuronMap.MapSize);     

            // Construct standard input stream for qdelaunay
            qdelaunayInput.AppendLine(neuronMap.InputDimension.ToString());
            qdelaunayInput.AppendLine(neuronMap.MapSize.ToString());
            for (int i = 0; i < neuronMap.MapSize; i++)
                qdelaunayInput.AppendLine(neuronMap[i].ToString(false, " "));            

            // Redirect the output stream of qdelaunay
            qdelaunayProcess.StartInfo.UseShellExecute = false;
            qdelaunayProcess.StartInfo.RedirectStandardOutput = true;
            qdelaunayProcess.StartInfo.FileName = "qdelaunay.exe";
            qdelaunayProcess.StartInfo.Arguments = "Fv";
            qdelaunayProcess.StartInfo.RedirectStandardInput = true;
            qdelaunayProcess.StartInfo.RedirectStandardError = true;
            qdelaunayProcess.Start();
            StreamWriter sw = qdelaunayProcess.StandardInput;
            sw.Write(qdelaunayInput);
            sw.Flush();
            sw.Close();

            string output = qdelaunayProcess.StandardOutput.ReadToEnd();
            qdelaunayProcess.WaitForExit();

            // parse output and construct connectivity matrix
            StringReader sr = new StringReader(output);
            int numRegions = Convert.ToInt32(sr.ReadLine());
            for (int i = 0; i < numRegions; i++)
            {
                string[] numbers = sr.ReadLine().Split();
                int numSites = Convert.ToInt32(numbers[0]);
                int s1;
                int s2;
                for (int j = 1; j < numSites; j++)
                {
                    s1 = Convert.ToInt32(numbers[j]);
                    s2 = Convert.ToInt32(numbers[j + 1]);
                    c[s1, s2] = 1;
                    c[s2, s1] = 1;
                }
                s1 = Convert.ToInt32(numbers[1]);
                s2 = Convert.ToInt32(numbers[numSites - 1]);
                c[s1, s2] = 1;
                c[s2, s1] = 1;
            }

            return c;
        }
        #endregion
    }
}
