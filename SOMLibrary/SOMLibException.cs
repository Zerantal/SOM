using System;
using System.Runtime.Serialization;

namespace SomLibrary
{
    public class SOMLibException : Exception
    {       
        public SOMLibException()
        {}

        public SOMLibException(string message)
            : base(message)
        {}

        public SOMLibException(string message, Exception inner)
            : base(message, inner)
        {}

        protected SOMLibException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {}        
    }
}
