namespace CLUNL.Massives
{
    /// <summary>
    /// Interface for holdable objects.
    /// </summary>
    public interface IHoldable
    {
        /// <summary>
        /// Hold object. Implementation should check handle.
        /// </summary>
        /// <param name="Handle"></param>
        void OnHold(int Handle);
        /// <summary>
        /// Release object. When the object is not hold, implementation should not throw exception.
        /// </summary>
        /// <param name="Handle"></param>
        void Release(int Handle);
        /// <summary>
        /// Check if the object is operatable with given handle.
        /// </summary>
        /// <param name="Handle"></param>
        /// <returns></returns>
        bool CheckAllowOperateWhenHold(int Handle);
        /// <summary>
        /// Check if the object is on hold.
        /// </summary>
        /// <returns></returns>
        bool CheckHold();
    }
}
