using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartSystem.Interface.Entities
{
    public class InterfaceException : Exception
    {
        public int InterfaceErrorCode { get; set; }
        public InterfaceException() { }
        public InterfaceException(int errorCode) { InterfaceErrorCode = errorCode; }
        public InterfaceException(string message) : base(message) { }
        public InterfaceException(int errorCode, string message) : base(message) { InterfaceErrorCode = errorCode; }
        public InterfaceException(string message, Exception inner) : base(message, inner) { }
        public InterfaceException(int errorCode, string message, Exception inner) : base(message, inner) { InterfaceErrorCode = errorCode; }
        public InterfaceException(int errorCode, Exception inner) : this(errorCode, inner.Message, inner) { }
    }
}
