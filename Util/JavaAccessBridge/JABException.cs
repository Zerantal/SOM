using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Util.JavaAccessBridge
{
    [Serializable]
    public class JABException : Exception, ISerializable
    {
        public JABException() : base()
        {        
        }

        public JABException(string message)
            : base(message)
        {           
        }

        public JABException(string message, Exception inner)
            : base(message, inner)
        {
        }

        // This constructor is needed for serialization.
        protected JABException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
