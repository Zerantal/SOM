using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.IO;
using System.Diagnostics.Contracts;

using MathLib.Matrices;

namespace SomLibrary
{
    // for static data sets
    [Serializable]
    public class BufferedFileInputLayer : IInputLayer
    {
        private string m_InputFile;

        [NonSerialized] private int m_LineCounter = 0;
        [NonSerialized] private int m_A = 0;    // attribute counter
        private int m_InputDimension = 0;
        private bool m_IsLabeled = false;
        private int m_BufferSize = 1048576;    // buffer size in number of DWORDs

        /// <summary>
        /// Read and parse data file. File format consists of input vectors on separate lines.
        /// Each vector can be delimited by a space, comma, or a tab character
        /// </summary>
        /// <param name="path"></param>        
        public BufferedFileInputLayer(string path)
        {
            if (path == null)
                throw new ArgumentNullException();
            if (!File.Exists(path))
                throw new ArgumentException("File does not exist: " + path);

            try
            {
                InputFile = path;
            }
            catch 
            {
                throw;
            }
        }

        public bool IsOnlineSource
        {
            get { return false; }
        }

        public string InputFile
        {
            get { return m_InputFile; }
            set 
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (!File.Exists(value))
                    throw new ArgumentException("File does not exist: " + value);

                m_InputFile = value;

                using (StreamReader sr = new StreamReader(m_InputFile))
                {
                    try
                    {
                        String line;
                        String[] values;
                        double refNum;

                        line = sr.ReadLine();
                        if (line == null)
                            throw new SOMFileException("Data file is empty."); 
                        values = line.Split(new char[] { ' ', '\t', ',', '|' }, // read first data line
                            StringSplitOptions.RemoveEmptyEntries);
                        if (values.Length < 1)  // this condn shouldn't be triggered?
                            throw new SOMFileException("Unable to parse first line of file.");  
                        // determine if data rows are labeled                
                        if (double.TryParse(values[0], out refNum) == false)
                        {
                            IsLabeled = true;
                            m_InputDimension = values.Length - 1;
                        }
                        else
                        {
                            IsLabeled = false;
                            m_InputDimension = values.Length;
                        }
                    }
                    catch (System.IO.IOException ex)
                    {
                        m_InputFile = null;
                        throw new IOException("Error reading file", ex);
                    }
                }                 
            }
        }

        public bool IsLabeled
        {
            get { return m_IsLabeled; }
            set { m_IsLabeled = value; }
        }

        public int InputDimension
        {
            get { return m_InputDimension; }            
        }

        public int BufferSize
        {
            get { return m_BufferSize; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Buffer size must be greater " +
                        "than or equal to zero.", (Exception) null);
                m_BufferSize = value;
            }
        }

        public virtual IEnumerator<Vector> SequentialEnumerator
        {
            get
            {
                // Contract.Ensures(// Contract.Result<IEnumerator<Vector>>() != null);

                Vector v;

                using (StreamReader sr = new StreamReader(new FileStream(m_InputFile, FileMode.Open),
                        Encoding.Default, true, m_BufferSize))
                {

                    string line;
                    string[] attributes;

                    m_LineCounter = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        v = new Vector(m_InputDimension);
                        attributes = line.Split(new char[] { ' ', '\t', ',', '|'}, StringSplitOptions.RemoveEmptyEntries);
                        if (attributes.Length != m_InputDimension + (IsLabeled ? 1 : 0))
                            throw new SOMFileException("Incorrect number of attributes on line", m_LineCounter, m_A);
                        if (IsLabeled)
                        {
                            for (m_A = 0; m_A < m_InputDimension; m_A++)
                                v[m_A] = double.Parse(attributes[m_A + 1]);
                        }
                        else
                        {
                            for (m_A = 0; m_A < m_InputDimension; m_A++)
                                v[m_A] = double.Parse(attributes[m_A]);
                        }

                        m_LineCounter++;
                        yield return v;
                    }                    
                }
            }
        }

        public IEnumerator<Vector> GetEnumerator()
        {
            try
            {
                return SequentialEnumerator;
            }
            catch (IOException ex)
            {
                throw new IOException("Error reading input file.", ex);
            }
            catch (Exception ex)
            {
                throw new SOMFileException("Error parsing file", 
                    m_LineCounter, m_A, ex);
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        [OnDeserialized]
        internal void CheckFileException(StreamingContext context)
        {
            if (m_InputFile != null)
                InputFile = m_InputFile;           
        }
    }
}
