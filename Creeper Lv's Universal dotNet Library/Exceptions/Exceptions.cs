using System;

namespace CLUNL.Exceptions
{

    /// <summary>
    /// Throw when the handle is illegal. (When handle equals -1 or other that depends on specific implementation of IHodlable)
    /// </summary>
    [Serializable]
    public class IllegalHandleException : Exception
    {
        /// <summary>
        /// Throws when the handle is illegal. (When handle equals -1 or other that depends on specific implementation of IHodlable)
        /// </summary>
        public IllegalHandleException() : base("Illegal Handle:NaN") { }
        /// <summary>
        /// Throws when the handle is illegal. (When handle equals -1 or other that depends on specific implementation of IHodlable)
        /// </summary>
        public IllegalHandleException(int Handle) : base("Illegal Handle:" + Handle) { }
        /// <summary>
        /// Throws when the handle is illegal. (When handle equals -1 or other that depends on specific implementation of IHodlable)
        /// </summary>
        public IllegalHandleException(int Handle, Exception inner) : base("Illegal Handle:" + Handle, inner) { }
    }
    /// <summary>
    /// Throw when using handle mismatch the handle that holds the target object.
    /// </summary>
    [Serializable]
    public class HandleMismatchException : Exception
    {
        /// <summary>
        /// Throw when using handle mismatch the handle that holds the target object.
        /// </summary>
        public HandleMismatchException() { }
        /// <summary>
        /// Throw when using handle mismatch the handle that holds the target object.
        /// </summary>
        public HandleMismatchException(int Handle1, int Handle2) : base($"Given handle({Handle1}) does not match original handle({Handle2}).") { }
        /// <summary>
        /// Throw when using handle mismatch the handle that holds the target object.
        /// </summary>
        public HandleMismatchException(int Handle1, int Handle2, Exception inner) : base($"Given handle({Handle1}) does not match original handle({Handle2}).", inner) { }
    }
    /// <summary>
    /// Throw when trying using the object that is on hold.
    /// </summary>
    [Serializable]
    public class ObjectIsOnHoldException : Exception
    {
        /// <summary>
        /// Throw when trying using the object that is on hold.
        /// </summary>
        public ObjectIsOnHoldException() : base("The object is on hold by other handle.") { }
    }
}
