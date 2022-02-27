using System;

namespace CLUNL.Utilities
{
    /// <summary>
    /// A tool to generate random string.
    /// </summary>
    public static class RandomTool
    {
        /// <summary>
        /// Generate a random string in give length and range.
        /// </summary>
        /// <param name="Length">Target string length.</param>
        /// <param name="RandomRange">Target string range</param>
        /// <returns></returns>
        public static string GetRandomString(int Length, RandomStringRange RandomRange)
        {
            char[] data = new char[Length];
            bool OK = false;

            Random r = new Random();
            for (int i = 0; i < Length; i++)
            {
                OK = false;
                while (OK == false)
                {
                    var d = r.Next(char.MaxValue);
                    switch (RandomRange)
                    {
                        case RandomStringRange.R1:
                            if (d >= '0' && d <= '9')
                            {
                                data[i] = (char)d;
                                OK = true;
                            }
                            break;
                        case RandomStringRange.R2:

                            if ((d >= '0' && d <= '9') | (d >= 'a' && d <= 'z'))
                            {
                                data[i] = (char)d;
                                OK = true;
                            }
                            break;
                        case RandomStringRange.R3:
                            if ((d >= '0' && d <= '9') | (d >= 'a' && d <= 'z') | d >= 'A' && d <= 'Z')
                            {
                                data[i] = (char)d;
                                OK = true;
                            }
                            break;
                        case RandomStringRange.R4:
                            data[i] = (char)d;
                            OK = true;
                            break;
                        default:
                            throw new Exception();
                    }
                }
            }
            return new string(data);
        }
    }
    /// <summary>
    /// R1: 0~9; R2:0~9,a~z; R3: 0~9,a~z,A~Z,R4: ANY CHARACTER
    /// </summary>
    public enum RandomStringRange
    {
        /// <summary>
        /// From 0 to 9.
        /// </summary>
        R1,
        /// <summary>
        /// From 0 to 9 and a to z.
        /// </summary>
        R2,
        /// <summary>
        /// From 0 to 0 , a to z and A to Z.
        /// </summary>
        R3,
        /// <summary>
        /// Any character.
        /// </summary>
        R4
    }
}
