using System;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Massives
{
    public interface IHoldable
    {
        void OnHold(int Handle);
        void Release(int Handle);
        bool CheckAllowOperateWhenHold(int Handle);
        bool CheckHold();
    }
}
