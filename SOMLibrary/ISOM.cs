using System;
using System.Collections.Generic;
using System.Text;
using MathLib.Matrices;
using Util;
using System.Diagnostics.Contracts;

namespace SomLibrary
{

    [ToDo("perhaps there should be some mechanism to load recommended values")]
    [ContractClass(typeof(ISOMContract))]
    public interface ISOM
    {
        // for deserialization purposes, this event should be prefixed with
        // [field: NonSerialized]
        event EventHandler ProgressUpdate; // periodic progress report event                         

        /// <summary>
        /// Train Network. Generally this method requires that the 
        /// map has been initialised (and InputReader set).
        /// </summary>        
        void Train();                              

        /// <summary>
        /// Simulate network with a single Vector
        /// </summary>
        /// <param name="x"></param>
        /// <param name="error"></param>
        /// <returns>Index of the neuron that input vector maps to</returns>
        int Simulate(Vector x, out double error);

        /// <summary>
        /// Gets or sets the Neuron Map. Has to be initialised before training.
        /// </summary>
        INeuronMap Map { get; set;}

        int ProgressInterval { get; set;}
              
        /// <summary>
        /// Assign an inputsource to algorithm.
        /// </summary>
        IInputLayer InputReader { set; get; }

        // This method is intended to be useful for signalling to the training algorithm to stop.
        // (note: the execution of the algorithm needn't be terminated after returning from this method)
        // This should be threadsafe method.
        void CancelTraining();
    }
}
