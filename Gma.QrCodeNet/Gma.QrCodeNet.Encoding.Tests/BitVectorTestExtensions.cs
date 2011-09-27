using System;
using System.Collections.Generic;
using System.Text;

namespace Gma.QrCodeNet.Encoding.Tests
{
    public static class BitVectorTestExtensions
    {
        public const char TrueChar = '1';
        public const char FalseChar = '0';

        public static string To01String(this IEnumerable<bool> bits)
        {
            StringBuilder result = new StringBuilder();
            foreach (bool bit in bits)
            {
                char ch = bit ? TrueChar : FalseChar;
                result.Append(ch);
            }
            return result.ToString();
        }

        internal static IEnumerable<bool> From01String(IEnumerable<char> bitsString)
        {
            foreach (char ch in bitsString)
            {
                switch (ch)
                {
                    case TrueChar:
                        yield return true;
                        break;
                    case FalseChar:
                        yield return false;
                        break;
                    default:
                        throw new ArgumentException("String is expected to contain only 0 and 1.", "bitsString");
                }
            }
        }
    }
}
