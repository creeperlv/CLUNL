using System;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Exceptions
{

    [Serializable]
    public class IllegalHandleException : Exception
    {
        public IllegalHandleException() : base("Illegal Handle:NaN") { }
        public IllegalHandleException(int Handle) : base("Illegal Handle:" + Handle) { }
        public IllegalHandleException(int Handle, Exception inner) : base("Illegal Handle:" + Handle, inner) { }
        protected IllegalHandleException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class HandleMismatchException : Exception
    {
        public HandleMismatchException() { }
        public HandleMismatchException(int Handle1, int Handle2) : base($"Given handle({Handle1}) does not match original handle({Handle2}).") { }
        public HandleMismatchException(int Handle1, int Handle2, Exception inner) : base($"Given handle({Handle1}) does not match original handle({Handle2}).", inner) { }
        protected HandleMismatchException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class ObjectIsOnHoldException : Exception
    {
        public ObjectIsOnHoldException():base("The object is on hold by other handle.") { }
        public ObjectIsOnHoldException(string message) : base(message) { }
        public ObjectIsOnHoldException(string message, Exception inner) : base(message, inner) { }
        protected ObjectIsOnHoldException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
