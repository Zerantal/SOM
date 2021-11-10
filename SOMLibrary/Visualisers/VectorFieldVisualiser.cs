using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using Util;
using MathLib.Matrices;

namespace SomLibrary
{
    public class VectorFieldVisualiser : IVisualiser
    {       
        //private String errMsg;
        private double sigma = 2; //parameter

        public VectorFieldVisualiser()
        {
        }

        public void VisualiseMap(ISOM algorithm, IDrawer drawer)
        {
            /*
            if (m_Drawer == null)
                throw new InvalidOperationException("drawer has not been set");
            if (!CanVisualiseMap(algorithm))
                throw new InvalidAlgorithmException(errMsg);
                        

            RectNeuronMap map = algorithm.Map as RectNeuronMap;
            Vector [] vectorField = new Vector[map.MapSize];
            int j;
            Vector diss_minus;
            Vector diss_pos;
            Vector con_minus;
            Vector con_pos;
            Vector w_minus;
            Vector w_pos;
            Vector w;
            double alpha;
            Vector pi, pj;
            double h;

            /*
            for (int i = 0; i < map.MapSize; i++)
            {
                vectorField[i] = new Vector(2);

                diss_minus = new Vector(2);
                diss_pos = new Vector(2);
                con_minus = new Vector(2);
                con_pos = new Vector(2);
                w_minus = new Vector(2);
                w_pos = new Vector(2);
                w = new Vector(2);
                pi = map.NeuronPosition(i);

                for (j = 0; j < map.MapSize; j++)
                {                    
                    pj = map.NeuronPosition(j);
                    alpha = Vector.Angle(pi, pj);
                    h = neighbourhoodFn((pi - pj).Norm);
                    w[0] = Math.Cos(alpha) * h;
                    w[1] = Math.Sin(alpha) * h;
                    if (w[0] > 0)
                    {
                        con_pos[0] = ((map[i] - map[j]).Norm) * w[0];
                        con_minus[0] = 0;
                    }
                    else
                    {
                        con_pos[0] = 0;
                        con_minus[0] = (-(map[i] - map[j]).Norm) * w[0];
                    }
                    if (w[1] > 0)
                    {
                        con_pos[1] = ((map[i] - map[j]).Norm) * w[1];
                        con_minus[1] = 0;
                    }
                    else
                    {
                        con_pos[1] = 0;
                        con_minus[1] = (-(map[i] - map[j]).Norm) * w[1];
                    }
                    diss_pos[0] += con_pos[0];
                    diss_minus[0] += con_minus[0];
                    diss_pos[1] += con_pos[1];
                    diss_minus[1] += con_minus[1];
                }

                vectorField[i][0] = (diss_minus[0] * w_pos[0] - diss_pos[0] * w_minus[0])
                    / (diss_pos[0] + diss_minus[0]);
                vectorField[i][1] = (diss_minus[1] * w_pos[1] - diss_pos[1] * w_minus[1])
                    / (diss_pos[1] + diss_minus[1]);
            }
             * */

        }

        public bool CanVisualiseMap(ISOM algorithm)
        {
            INeuronMap map = algorithm.Map;
            if (map == null)
            {
//                errMsg = "Algorithms map is not set.";
                return false;
            }

            return true;
        }

        public double neighbourhoodFn(double dist)
        {
            return (Math.Exp(-(dist * dist) / (2 * sigma)));
        }
    }
}
