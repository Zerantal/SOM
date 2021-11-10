using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.IO;
using System.Diagnostics.Contracts;

using MathLib.Matrices;

namespace SomLibrary
{
    // for static data sets
    public class FileInputLayer : IInputLayer
    {
        private string _inputFile;

        [NonSerialized]
        private int _lineCounter;
        [NonSerialized]
        private int _attrCounter;    // attribute counter
        private int _inputDimension;
        private Matrix _data;
        private List<String> _labels;
        private bool _randomizeInput;

        /// <summary>
        /// Read and parse data file. File format consists of input vectors on separate lines.
        /// Each vector can be delimited by a space, comma, or a tab character
        /// </summary>
        /// <param name="path"></param>        
        public FileInputLayer(string path)
        {
            if (path == null)
                throw new ArgumentNullException();
            if (!File.Exists(path))
                throw new ArgumentException("File does not exist: " + path);

            Exception e = ParseFile(path);
            if (e != null)
                throw e;            
        }

        public bool IsOnlineSource
        {
            get { return false; }
        }

        public string InputFile
        {
            get { return _inputFile; }
            set
            {
                // Contract.Requires(value != null);

                Exception e = ParseFile(value);
                if (e != null)
                    throw e;
            }
 
        }

        private Exception ParseFile(string file)
        {
            // Contract.Requires(file != null);            
            
            _inputFile = file;

            using (StreamReader sr = new StreamReader(_inputFile))
            {
                try
                {
                    double refNum;
                    List<String> fileData = new List<String>();

                    string line = sr.ReadLine();
                    fileData.Add(line);
                    string[] values = line.Split(new[] { ' ', '\t', ',' }, // read first data line
                                                 StringSplitOptions.RemoveEmptyEntries);
                    // determine if data rows are labeled                
                    if (double.TryParse(values[0], out refNum) == false)
                    {
                        IsLabeled = true;
                        _inputDimension = values.Length - 1;
                    }
                    else
                    {
                        IsLabeled = false;
                        _inputDimension = values.Length;
                    }


                    // read data into list of strings
                    while ((line = sr.ReadLine()) != null)
                        fileData.Add(line);

                    _lineCounter = 0;
                    _data = new Matrix(fileData.Count, _inputDimension);
                    if (IsLabeled)
                        _labels = new List<String>(fileData.Count);
                    // Read data into Matrix       
                    try
                    {
                        for (int i = 0; i < _data.Rows; i++)
                        {
                            values = fileData[i].Split(new[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (values.Length != _inputDimension + (IsLabeled ? 1 : 0))
                                throw new SOMFileException("Incorrect number of attributes on line", _lineCounter, _attrCounter);
                            if (IsLabeled)
                            {
                                for (_attrCounter = 0; _attrCounter < _inputDimension; _attrCounter++)
                                    _data[_lineCounter, _attrCounter] = double.Parse(values[_attrCounter+1]);
                                _labels.Add(values[0]);
                            }
                            else
                            {
                                for (_attrCounter = 0; _attrCounter < _inputDimension; _attrCounter++)
                                    _data[_lineCounter, _attrCounter] = double.Parse(values[_attrCounter]);
                            }

                            _lineCounter++;
                        }
                    }
                    catch (FormatException ex)
                    {
                        return new SOMFileException("Error parsing file", _lineCounter, _attrCounter, ex);
                    }
                }
                catch (IOException ex)
                {
                    _inputFile = null;
                    return new IOException("Error reading file", ex);
                }                
            }
            return null;
        }

        public bool IsLabeled { get; set; }

        public int InputDimension
        {
            get { return _inputDimension; }
        }

        public bool RandomizeInput
        {
            get { return _randomizeInput; }
            set { _randomizeInput = value; }
        }

        public virtual IEnumerator<Vector> SequentialEnumerator
        {
            get {
                return _data.RowEnumerator.GetEnumerator();
            }
        }

        public virtual IEnumerator<Vector> RandomEnumerator
        {
            get
            {
                // Contract.Ensures(// Contract.Result<IEnumerator<Vector>>() != null);

                List<int> randomIndices = new List<int>(_data.Rows);

                for (int i = 0; i < _data.Rows; i++)
                    randomIndices.Add(i);

                Random r = new Random();
                int numData = _data.Rows;
                for (int i = numData-1; i >= 0; i--)
                {
                    int k = r.Next(i);
                    int tmp = randomIndices[i];
                    randomIndices[i] = randomIndices[k];
                    randomIndices[k] = tmp;
                }

                return randomIndices.Select(idx => _data.GetRow(idx)).GetEnumerator();
            }
        }

        public IEnumerator<Vector> GetEnumerator()
        {
            if (_data != null)
            {
                return _randomizeInput ? RandomEnumerator : SequentialEnumerator;
            }
            throw new InvalidOperationException("No file has been loaded yet.");
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        [OnDeserialized]
        internal void CheckFileException(StreamingContext context)
        {
            if (_inputFile != null)
                InputFile = _inputFile;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            // Contract.Invariant(_data != null);
        }
    }
}
