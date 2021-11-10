using System;
using System.Runtime.Serialization;

namespace SomLibrary
{
    public class InvalidAlgorithmException : Exception
    {
        private readonly Type _expectedType;
        private readonly Type _passedType;

        public InvalidAlgorithmException()
        {            
        }

        public InvalidAlgorithmException(string message)
            : base(message)
        {          
        }

        public InvalidAlgorithmException(string message, Exception inner)
            : base(message, inner)
        {        
        }

        protected InvalidAlgorithmException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {         
        }

        public InvalidAlgorithmException(string message, Type expected, Type passed)
            : base(message)
        {
            _expectedType = expected;
            _passedType = passed;
        }

        public InvalidAlgorithmException(string message, Type expected, Type passed, Exception inner)
            : base(message, inner)
        {
            _expectedType = expected;
            _passedType = passed;
        }

        public Type ExpectedType
        {
            get { return _expectedType; }
        }

        public Type PassedType
        {
            get { return _passedType; }
        }
    }
}