using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Diagnostics.Contracts;

namespace Util
{
    [Serializable]
    public class WinApiException : Exception, ISerializable
    {
        string _sourceDLL;
        string _invokedFunction;
        int _errorCode;

        public WinApiException() : base()
        {           
        }

        public WinApiException(string message) : base(message)
        {         
        }

        public WinApiException(string message, Exception inner) : base(message, inner)
        {          
        }

        // This constructor is needed for serialization.
        protected WinApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            // Contract.Requires(info != null);

            _sourceDLL = info.GetString("SourceDLL");
            _invokedFunction = info.GetString("InvokedFunction");            
        }

        public WinApiException(string message, string sourceDll, string invokedFunction, int errorCode) 
            : base(message)
        {
            _sourceDLL = sourceDll;
            _invokedFunction = invokedFunction;
            _errorCode = errorCode;
        }

        public string InvokedFunction
        {
            get { return _invokedFunction; }
            set { _invokedFunction = value; }
        }


        public string SourceDll
        {
            get { return _sourceDLL; }
            set { _sourceDLL = value; }
        }

        public int ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }

        #region ISerializable Members

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("SourceDLL", _sourceDLL);
            info.AddValue("InvokedFunction", _invokedFunction);
        }

        #endregion
    }
}
