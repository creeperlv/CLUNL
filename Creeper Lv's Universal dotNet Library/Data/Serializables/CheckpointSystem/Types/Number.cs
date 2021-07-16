using System;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Data.Serializables.CheckpointSystem.Types
{
    /// <summary>
    /// Int Number, helpful when using Checkpoint.
    /// </summary>
    public class IntNumber
    {
        /// <summary>
        /// Real data.
        /// </summary>
        public int Data;

        /// <summary>
        /// Int Number, helpful when using Checkpoint.
        /// </summary>
        public IntNumber(int data)
        {
            Data = data;
        }

        /// <summary>
        /// Int Number, helpful when using Checkpoint.
        /// </summary>
        public IntNumber()
        {

        }
        /// <summary>
        /// Convert from long to IntNumber;
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator IntNumber(long value)
        {
            return new IntNumber((int)value);
        }
        /// <summary>
        /// Convert from long to IntNumber;
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator IntNumber(int value)
        {
            return new IntNumber(value);
        }
        /// <summary>
        /// To int.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator int(IntNumber value)
        {
            return value.Data;
        }
        /// <summary>
        /// To long
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator long(IntNumber value)
        {
            return (long)value.Data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        /// <returns></returns>
        public static bool operator ==(IntNumber L, IntNumber R)
        {
            return L.Data == R.Data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        /// <returns></returns>
        public static bool operator !=(IntNumber L, IntNumber R)
        {
            return L.Data != R.Data;
        }
        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Data.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is IntNumber N)
            {
                return N.Data == Data;
            }
            else if (obj is int I)
            {
                return I == Data;
            }
            return base.Equals(obj);
        }
    }
}
