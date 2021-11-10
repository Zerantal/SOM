using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MathLib
{
    /// <summary>
    /// Represent a general MathLib exception.
    /// </summary>   
    [Serializable]
    public class MathLibException : Exception
    {
        /// <summary>
        /// Initialise a new instance of the <see cref="MathLibException"/> class.
        /// </summary>
        public MathLibException()
            : base()
        { }

        /// <summary>
        /// Initialise a new instance of the <see cref="MathLibException"/> class with a
        /// specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason
        /// for this exception.</param>
        public MathLibException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initialise a new instance of the <see cref="MathLibException"/> class with a
        /// specified error message and the exception that is the cause
        /// of the current exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason
        /// for this exception.</param>
        /// <param name="inner">The exception which is the cause of the 
        /// current exception  or a null reference (Nothing in Visual Basic)
        /// if no inner exception is specified. </param>
        public MathLibException(string message, Exception inner)
            : base(message, inner)
        { }

        /// <summary>
        /// Initialise a new instance of the <see cref="MathLibException"/> class 
        /// with serialized data. 
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.
        /// </param>
        /// <param name="context">An object that describes the source or destination
        /// of the serialized data.</param>
        protected MathLibException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
