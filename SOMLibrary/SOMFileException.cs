using System;
using System.Runtime.Serialization;

namespace SomLibrary
{
    public class SOMFileException : Exception
    {
        private readonly int _line;
        private readonly int _attr;

        public SOMFileException()
        {
            _line = 0;
            _attr = 0;
        }

        public SOMFileException(string message) : base (message)
        {
            _line = 0;
            _attr = 0;
        }

        public SOMFileException(string message, Exception inner) : base (message, inner)
        {
            _line = 0;
            _attr = 0;
        }

        protected SOMFileException(SerializationInfo info, StreamingContext context) : base (info, context)
        {
            _line = 0;
            _attr = 0;
        }

        public SOMFileException(string message, int l, int a) : base (message)
        {            
            _line = l;
            _attr = a;
        }

        public SOMFileException(string message, int l, int a, Exception inner) : base(message, inner)            
        {
            _line = l;
            _attr = a;            
        }

        public int Line
        {
            get { return _line; }
        }

        public int Attribute
        {
            get { return _attr; }
        }
    }
}
