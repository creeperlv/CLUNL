using CLUNL.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Massives
{
    public class HoldableObject : IHoldable
    {
        internal int Handle=0;

        public void OnHold(int Handle)
        {
            if (Handle == -1)
            {
                throw new IllegalHandleException(Handle);
            }
            this.Handle = Handle;
        }

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

        public bool CheckHold()
        {
            if (this.Handle == -1)
                return false;
            else
                return true;
        }
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
