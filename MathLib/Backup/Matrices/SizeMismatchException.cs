using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Text;

namespace MathLib.Matrices
{
    /// <summary>
    /// The exception that is thrown when the dimensionality of a Vector or Matric
    /// object are incompatible for a requested operation.
    /// </summary>
    [Serializable]
    public class SizeMismatchException : Exception
    {
        /// <summary>
        /// Initialise a new instance of the <see cref="SizeMismatchException"/> class.
        /// </summary>
        public SizeMismatchException()
            : base()
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="SizeMismatchException"/> class with a
        /// specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason
        /// for this exception.</param>
        public SizeMismatchException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="SizeMismatchException"/> class with a
        /// specified error message and the exception that is the cause
        /// of the current exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason
        /// for this exception.</param>
        /// <param name="inner">The exception which is the cause of the 
        /// current exception  or a null reference (Nothing in Visual Basic)
        /// if no inner exception is specified. </param>
        public SizeMismatchException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="SizeMismatchException"/> class 
        /// with serialized data. 
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.
        /// </param>
        /// <param name="context">An object that describes the source or destination
        /// of the serialized data.</param>
        protected SizeMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
