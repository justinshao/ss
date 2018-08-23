using System;
using System.IO;
using System.Text;
[Serializable]
public class MyException : Exception {
    public int MyErrorCode { get; set; }
    public MyException() { }
    public MyException(int errorCode) { MyErrorCode = errorCode; }
    public MyException(string message) : base(message) { }
    public MyException(int errorCode, string message) : base(message) { MyErrorCode = errorCode; }
    public MyException(string message, Exception inner) : base(message, inner) { }
    public MyException(int errorCode, string message, Exception inner) : base(message, inner) { MyErrorCode = errorCode; }
    public MyException(int errorCode, Exception inner) : this(errorCode, inner.Message, inner) { }
}