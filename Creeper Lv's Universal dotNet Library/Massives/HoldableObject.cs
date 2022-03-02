using CLUNL.Exceptions;

namespace CLUNL.Massives
{
    /// <summary>
    /// An implementation of IHoldable.
    /// </summary>
    public class HoldableObject : IHoldable
    {
        internal int Handle = 0;
        /// <summary>
        /// Hold the object.
        /// </summary>
        /// <param name="Handle"></param>

        public void OnHold(int Handle)
        {
            if (Handle == -1)
            {
                throw new IllegalHandleException(Handle);
            }
            this.Handle = Handle;
        }
        /// <summary>
        /// Release the object.
        /// </summary>
        /// <param name="Handle"></param>
        public void Release(int Handle)
        {
            if (this.Handle == Handle)
            {
                this.Handle = -1;
            }
            else
            {
                throw new HandleMismatchException(Handle, this.Handle);
            }
        }
        /// <summary>
        /// Check whether the object is on hold.
        /// </summary>
        /// <returns></returns>
        public bool CheckHold()
        {
            if (this.Handle == -1)
                return false;
            else
                return true;
        }
        /// <summary>
        /// Check if given handle matches the handle that holds the object and throws an exception says that the object is on hold when ThrowExceptionWhenHold flag is on.
        /// </summary>
        /// <param name="Handle"></param>
        /// <returns></returns>
        public bool HitHandle(int Handle)
        {
            if (CheckAllowOperateWhenHold(Handle) == false)
            {
                if (LibraryInfo.GetFlag(FeatureFlags.ThrowExceptionWhenHold) == 0)
                {
                    throw new ObjectIsOnHoldException();
                }
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// Check if given handle matches the handle that holds the object.
        /// </summary>
        /// <param name="Handle"></param>
        /// <returns></returns>
        public bool CheckAllowOperateWhenHold(int Handle)
        {
            if (this.Handle == Handle)
            {
                return true;
            }
            else
            {
                if (this.Handle == -1)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
