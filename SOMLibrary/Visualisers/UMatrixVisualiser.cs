using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using Util;
using MathLib.Matrices;


namespace SomLibrary
{/*
    [SOMPluginDetail("UMatrix Renderer", "UMatrix representation of feature map.")]
    public class UMatrixVisualiser : IVisualiser
    {
        private IDrawer m_Drawer;
        private String errMsg;

        public UMatrixVisualiser()
        {
        }

        public void VisualiseMap(ISOM algorithm)
        {
            if (m_Drawer == null)
                throw new InvalidOperationException("drawer has not been set");
            if (!CanVisualiseMap(algorithm))
                throw new InvalidAlgorithmException(errMsg);

            Matrix U;
            if (algorithm.Map is RectNeuronMap)
                U = CalcUMatrixRect((RectNeuronMap)algorithm.Map);
            else
                U = CalcUMatrixHex((HexNeuronMap)algorithm.Map);

            int[] UMatColour = new int[U.Rows * U.Columns];
            int i, j;

            // Create UMatColour values
            double max = double.MinValue;
            double min = double.MaxValue;
            for (i = 1; i <= U.Rows; i++)
                for (j = 1; j <= U.Columns; j++)
                {
                    if (U[i,j] > max)
                        max = U[i,j];
                    if (U[i,j] < min)
                        min = U[i,j];
                }
            byte c;
            double range = max - min;
            int idx = 0;
            for (i = 1; i <= U.Rows; i++)
                for (j = 1; j <= U.Columns; j++)
                {
                    c = (byte)(255 - (byte)(((U[i,j] - min) / range) * 255)) ;
                    UMatColour[idx] = 255 << 24 | c << 16 | c << 8 | c;
                    idx++;
                }

            m_Drawer.Clear();
            // render Hexagonal UMatrix
            if (algorithm.Map is HexNeuronMap)
            {
                float tmpW = 2f * (U.Columns + 1) * 0.866f;
                float tmpH = (3 * U.Rows + 1) / 2; 
                float h;    // length of hexagonal spoke
                float w;    // width of hexagonal cell
                float x, y; // center coords of hexagonal cell

                if (tmpW > tmpH)
                {

                    w = 2f / (2 * (U.Columns + 1));
                    h = w / 0.866f / 2;
                }
                else
                {
                    h = 2f / (3 * U.Rows + 1);
                    w = h * 2 * 0.866f;
                }
               
                idx = 0;
                Point[] cell = new Point[6];
                for (j = 1; j <= U.Rows; j++)
                {
                    y = ((3f / 2f) * j + 1) * h;
                    x = w / 2;
                    if ((j + 1) % 2 == 0)
                        x += w / 2;
                    else if ((j + 2) % 4 == 0)
                        x += w;
                    for (i = 0; i < U.Columns; i++)
                    {
                        cell[0].X = x - w / 2; cell[0].Y = y - (float)(0.5 * h);
                        cell[1].X = x - w / 2; cell[1].Y = y + (float)(0.5 * h);
                        cell[2].X = x; cell[2].Y = y + h;
                        cell[3].X = x + w / 2; cell[3].Y = y + (float)(0.5 * h);
                        cell[4].X = x + w / 2; cell[4].Y = y - (float)(0.5 * h);
                        cell[5].X = x; cell[5].Y = y - h;
                        m_Drawer.FillPolygon(cell, unchecked((int)UMatColour[idx]));
                        idx++;
                        x += w;
                    }
                }
            }
            else // render rectangular UMatrix
            {     
                float width;
                if (U.Columns > U.Rows)
                    width = 1f / U.Columns;
                else
                    width = 1f / U.Rows;
                
                idx = 0;
                for (j = 1; j <= U.Rows; j++)
                    for (i = 1; i <= U.Columns; i++)
                    {
                        m_Drawer.FillRectangle(new Point((float)(i * width), (float)(j * width)),
                            width, width, unchecked((int)UMatColour[idx]));
                        idx++;
                    }
            }
        }

        public bool CanVisualiseMap(ISOM algorithm)
        {
            RectNeuronMap map;
            if (algorithm.Map == null)
            {
                errMsg = "Algorithms map is not set.";
                return false;
            }

            if ((map = algorithm.Map as RectNeuronMap) == null)
            {
                errMsg = "Algorithms map must be either a HexNeuronMap or a RectNeuronMap";
                return false;
            }

            if (map.XDim < 2 || map.YDim < 2)
            {
                errMsg = "Algorithms map must 2 dimensional";
                return false;
            }

            return true;
        }

        public IDrawer Drawer
        {
            get { return m_Drawer; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                m_Drawer = value;
            }
        }

        private Matrix CalcUMatrixRect(RectNeuronMap map)
        {
            int ux = 2 * map.XDim - 1;
            int uy = 2 * map.YDim - 1;
            Matrix U = new Matrix(uy, ux);


            int i, j, x, y;
            for (y = 1; y <= map.YDim; y++)
            {
                i = 2 * y-1;
                for (x = 1; x <= map.XDim; x++)
                {
                    j = 2 * x-1;
                    // horizontal
                    if (x < map.XDim)
                        U[i, j + 1] = (map[x, y] - map[x + 1, y]).Norm;
                    // vertical
                    if (y < map.YDim)
                        U[i + 1, j] = (map[x, y] - map[x, y + 1]).Norm;
                    // diagonal
                    if (y < map.YDim && x < map.XDim)
                        U[i + 1, j + 1] = ((map[x, y] - map[x + 1, y + 1]).Norm +
                            (map[x + 1, y] - map[x, y + 1]).Norm) / 2;                }
            }

            for (i = 1; i <= uy; i += 2)
            {
                for (j = 1; j <= ux; j += 2)
                {
                    if (i > 1 && j > 1 && i < uy && j < ux) // middle of map
                        U[i, j] = (U[i, j - 1] + U[i, j + 1] + U[i - 1, j] + U[i + 1, j]) / 4;
                    else if (i == 1 && j > 1 && j < ux) // lower edge
                        U[i, j] = (U[i + 1, j] + U[i, j - 1] + U[i, j + 1]) / 3;
                    else if (i == uy && j > 1 && j < ux) // upper edge
                        U[i, j] = (U[i - 1, j] + U[i, j - 1] + U[i, j + 1]) / 3;
                    else if (j == 1 && i > 1 && i < uy) // left edge
                        U[i, j] = (U[i - 1, j] + U[i + 1, j] + U[i, j + 1]) / 3;
                    else if (j == ux && i > 1 && i < uy) // right edge
                        U[i, j] = (U[i - 1, j] + U[i + 1, j] + U[i, j - 1]) / 3;
                    else if (i == 1 && j == 1) // bottom left corner
                        U[i, j] = (U[i, j + 1] + U[i + 1, j]) / 2;
                    else if (i == 1 && j == ux) // bottom right corner
                        U[i, j] = (U[i, j - 1] + U[i + 1, j]) / 2;
                    else if (i == uy && j == 1) // top left corner
                        U[i, j] = (U[i - 1, j] + U[i, j + 1]) / 2;
                    else if (i == uy && j == ux) // top right corner
                        U[i, j] = (U[i - 1, j] + U[i, j - 1]) / 2;

                }
            }
            return U;

        }

        private Matrix CalcUMatrixHex(HexNeuronMap map)
        {
            int ux = 2 * map.XDim - 1;
            int uy = 2 * map.YDim - 1;
            Matrix U = new Matrix(uy, ux);


            int i, j, x, y;
            for (y = 0; y < map.YDim; y++)
            {
                i = 2 * y;
                for (x = 0; x < map.XDim; x++)
                {
                    j = 2 * x;
                    // horizontal
                    if (x < map.XDim - 1)
                        U[i, j + 1] = (map[x, y] - map[x + 1, y]).Norm;

                    // diagonals
                    if (y < map.YDim - 1)
                    {
                        if (y % 2 == 1 || x > 0)
                            U[i + 1, j - 1] = (map[x, y] - map[x - 1, y + 1]).Norm;
                        if (y % 2 == 0 || x < map.XDim - 1)
                            U[i + 1, j] = (map[x, y] - map[x, y + 1]).Norm;
                    }

                }
            }

            for (i = 0; i < uy; i += 2)
            {
                for (j = 0; j < ux; j += 2)
                {
                    if (i > 0 && j > 0 && i < ux - 1 && j < uy - 1) // middle of map
                    {
                        if (i % 4 == 0)
                            U[i, j] = (U[i, j + 1] + U[i - 1, j] + U[i - 1, j - 1] +
                                U[i, j - 1] + U[i + 1, j - 1] + U[i + 1, j]) / 6;
                        else
                        {
                            U[i, j] = (U[i, j + 1] + U[i - 1, j + 1] + U[i - 1, j] +
                                U[i, j - 1] + U[i + 1, j] + U[i + 1, j + 1]) / 6;
                        }
                    }
                    else if (i == 0 && j > 0 && j < ux - 1) // lower edge
                        U[i, j] = (U[i, j - 1] + U[i, j + 1] + U[i + 1, j - 1] + U[i + 1, j]) / 4;
                    else if (i == uy - 1 && j > 0 && j < ux - 1) // upper edge
                    {
                        if (i % 4 == 0)
                            U[i, j] = (U[i, j - 1] + U[i, j + 1] + U[i - 1, j - 1] + U[i - 1, j]) / 4;
                        else
                            U[i, j] = (U[i, j - 1] + U[i, j + 1] + U[i - 1, j] + U[i - 1, j + 1]) / 4;
                    }
                    else if (j == 0 && i > 0 && i < uy - 1) // left edge
                    {
                        if (i % 4 == 0)
                            U[i, j] = (U[i + 1, j] + U[i, j + 1] + U[i - 1, j]) / 3;
                        else
                            U[i, j] = (U[i + 1, j] + U[i + 1, j + 1] + U[i, j + 1] + U[i - 1, j + 1] + U[i - 1, j]) / 5;
                    }
                    else if (j == ux - 1 && i > 0 && i < uy - 1) // right edge
                    {
                        if (i % 4 == 0)
                            U[i, j] = (U[i - 1, j] + U[i - 1, j - 1] + U[i, j - 1] + U[i + 1, j - 1] + U[i + 1, j]) / 5;
                        else
                            U[i, j] = (U[i - 1, j] + U[i, j - 1] + U[i + 1, j]) / 3;
                    }
                    else if (i == 0 && j == 0) // bottom left corner
                        U[i, j] = (U[i + 1, j] + U[i, j + 1]) / 2;
                    else if (i == 0 && j == ux - 1) // bottom right corner
                        U[i, j] = (U[i, j - 1] + U[i + 1, j - 1] + U[i + 1, j]) / 3;
                    else if (i == uy - 1 && j == 0) // top left corner
                    {
                        if (i % 4 == 0)
                            U[i, j] = (U[i, j + 1] + U[i - 1, j]) / 2;
                        else
                            U[i, j] = (U[i, j + 1] + U[i - 1, j + 1] + U[i - 1, j]) / 3;
                    }
                    else if (i == uy - 1 && j == ux - 1) // top right corner
                    {
                        if (i % 4 == 0)
                            U[i, j] = (U[i - 1, j] + U[i - 1, j - 1] + U[i, j - 1]) / 3;
                        else
                            U[i, j] = (U[i - 1, j] + U[i, j - 1]) / 2;
                    }

                }
            }
            return U;            
        }

    }*/
}
