using System;
using System.Numerics;

namespace CLUNL.Scripting.SMS
{
    public class Number
    {
        Type TargetType;
        object Final;
        public Number(object obj, string Fallback, Type TargetType)
        {
            this.TargetType = TargetType; Final = obj;
            if (obj == null)
            {

                if (TargetType == typeof(int))
                {
                    Final = int.Parse(Fallback);
                }
                else
                if (TargetType == typeof(uint))
                {
                    Final = uint.Parse(Fallback);
                }
                else if (TargetType == typeof(byte))
                {
                    Final = byte.Parse(Fallback);
                }
                else if (TargetType == typeof(short))
                {
                    Final = short.Parse(Fallback);
                }
                else if (TargetType == typeof(ushort))
                {
                    Final = ushort.Parse(Fallback);
                }
                else if (TargetType == typeof(long))
                {
                    Final = long.Parse(Fallback);
                }
                else if (TargetType == typeof(ulong))
                {
                    Final = ulong.Parse(Fallback);
                }
                else if (TargetType == typeof(float))
                {
                    Final = float.Parse(Fallback);
                }
                else if (TargetType == typeof(double))
                {
                    Final = double.Parse(Fallback);
                }
                else if (TargetType == typeof(BigInteger))
                {
                    Final = BigInteger.Parse(Fallback);
                }
            }
        }
        public static object operator +(Number L, Number R)
        {
            var TargetT = L.TargetType;
            if (TargetT == typeof(int))
            {
                return Convert.ToInt32(L.Final) + Convert.ToInt32(R.Final);
            }
            else if (TargetT == typeof(uint))
            {
                return Convert.ToUInt32(L.Final) + Convert.ToUInt32(R.Final);
            }
            else if (TargetT == typeof(byte))
            {
                return Convert.ToByte(L.Final) + Convert.ToByte(R.Final);
            }
            else if (TargetT == typeof(short))
            {
                return Convert.ToInt16(L.Final) + Convert.ToInt16(R.Final);
            }
            else if (TargetT == typeof(ushort))
            {
                return Convert.ToUInt16(L.Final) + Convert.ToUInt16(R.Final);
            }
            else if (TargetT == typeof(long))
            {
                return Convert.ToInt64(L.Final) + Convert.ToInt64(R.Final);
            }
            else if (TargetT == typeof(ulong))
            {
                return Convert.ToUInt64(L.Final) + Convert.ToUInt64(R.Final);
            }
            else if (TargetT == typeof(float))
            {
                return Convert.ToSingle(L.Final) + Convert.ToSingle(R.Final);
            }
            else if (TargetT == typeof(double))
            {
                return Convert.ToDouble(L.Final) + Convert.ToDouble(R.Final);
            }
            else if (TargetT == typeof(BigInteger))
            {
                return (BigInteger)L.Final + (BigInteger)R.Final;
            }
            return null;
        }
        public static object operator -(Number L, Number R)
        {
            var TargetT = L.TargetType;
            if (TargetT == typeof(int))
            {
                return Convert.ToInt32(L.Final) - Convert.ToInt32(R.Final);
            }
            else if (TargetT == typeof(uint))
            {
                return Convert.ToUInt32(L.Final) - Convert.ToUInt32(R.Final);
            }
            else if (TargetT == typeof(byte))
            {
                return Convert.ToByte(L.Final) - Convert.ToByte(R.Final);
            }
            else if (TargetT == typeof(short))
            {
                return Convert.ToInt16(L.Final) - Convert.ToInt16(R.Final);
            }
            else if (TargetT == typeof(ushort))
            {
                return Convert.ToUInt16(L.Final) - Convert.ToUInt16(R.Final);
            }
            else if (TargetT == typeof(long))
            {
                return Convert.ToInt64(L.Final) - Convert.ToInt64(R.Final);
            }
            else if (TargetT == typeof(ulong))
            {
                return Convert.ToUInt64(L.Final) - Convert.ToUInt64(R.Final);
            }
            else if (TargetT == typeof(float))
            {
                return Convert.ToSingle(L.Final) - Convert.ToSingle(R.Final);
            }
            else if (TargetT == typeof(double))
            {
                return Convert.ToDouble(L.Final) - Convert.ToDouble(R.Final);
            }
            else if (TargetT == typeof(BigInteger))
            {
                return (BigInteger)L.Final - (BigInteger)R.Final;
            }
            return null;
        }
        public static object operator *(Number L, Number R)
        {
            var TargetT = L.TargetType;
            if (TargetT == typeof(int))
            {
                return Convert.ToInt32(L.Final) * Convert.ToInt32(R.Final);
            }
            else if (TargetT == typeof(uint))
            {
                return Convert.ToUInt32(L.Final) * Convert.ToUInt32(R.Final);
            }
            else if (TargetT == typeof(byte))
            {
                return Convert.ToByte(L.Final) * Convert.ToByte(R.Final);
            }
            else if (TargetT == typeof(short))
            {
                return Convert.ToInt16(L.Final) * Convert.ToInt16(R.Final);
            }
            else if (TargetT == typeof(ushort))
            {
                return Convert.ToUInt16(L.Final) * Convert.ToUInt16(R.Final);
            }
            else if (TargetT == typeof(long))
            {
                return Convert.ToInt64(L.Final) * Convert.ToInt64(R.Final);
            }
            else if (TargetT == typeof(ulong))
            {
                return Convert.ToUInt64(L.Final) * Convert.ToUInt64(R.Final);
            }
            else if (TargetT == typeof(float))
            {
                return Convert.ToSingle(L.Final) * Convert.ToSingle(R.Final);
            }
            else if (TargetT == typeof(double))
            {
                return Convert.ToDouble(L.Final) * Convert.ToDouble(R.Final);
            }
            else if (TargetT == typeof(BigInteger))
            {
                return (BigInteger)L.Final * (BigInteger)R.Final;
            }
            return null;
        }
        public static object operator /(Number L, Number R)
        {
            var TargetT = L.TargetType;
            if (TargetT == typeof(int))
            {
                return Convert.ToInt32(L.Final) / Convert.ToInt32(R.Final);
            }
            else if (TargetT == typeof(uint))
            {
                return Convert.ToUInt32(L.Final) / Convert.ToUInt32(R.Final);
            }
            else if (TargetT == typeof(byte))
            {
                return Convert.ToByte(L.Final) / Convert.ToByte(R.Final);
            }
            else if (TargetT == typeof(short))
            {
                return Convert.ToInt16(L.Final) / Convert.ToInt16(R.Final);
            }
            else if (TargetT == typeof(ushort))
            {
                return Convert.ToUInt16(L.Final) / Convert.ToUInt16(R.Final);
            }
            else if (TargetT == typeof(long))
            {
                return Convert.ToInt64(L.Final) / Convert.ToInt64(R.Final);
            }
            else if (TargetT == typeof(ulong))
            {
                return Convert.ToUInt64(L.Final) / Convert.ToUInt64(R.Final);
            }
            else if (TargetT == typeof(float))
            {
                return Convert.ToSingle(L.Final) / Convert.ToSingle(R.Final);
            }
            else if (TargetT == typeof(double))
            {
                return Convert.ToDouble(L.Final) / Convert.ToDouble(R.Final);
            }
            else if (TargetT == typeof(BigInteger))
            {
                return (BigInteger)L.Final / (BigInteger)R.Final;
            }
            return null;
        }
    }
}
