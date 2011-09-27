using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gma.QrCodeNet.Encoding.Tests
{
    public static class BitMatrixTestExtensions
    {
        public const char TrueChar = '1';
        public const char FalseChar = '0';

        public static string ToBase64(this BitMatrix matrix)
        {
            byte[] bytes = matrix.EncodeToBytes();
            string base64String = Convert.ToBase64String(bytes);
            return base64String;
        }

        private static BitMatrix FromBytes(byte[] bytes)
        {
            int length = BitConverter.ToInt32(bytes, 0);
            byte[] data = new byte[(bytes.Length - sizeof(int)) * sizeof(byte)];
            Array.Copy(bytes, sizeof(int), data, 0, data.Length);

            int width = (int)Math.Sqrt(length);
            if (width * width != length)
            {
                throw new ArgumentException(string.Format("The header containsinvalid length value. Length must be a square of width.{0}", length), "bytes");
            }

            BitArray bitArray = new BitArray(data);
            SimpleBitMatrix matrix = new SimpleBitMatrix(width);
            for (int index = 0; index < length; index++)
            {
                int i = index % width;
                int j = index / width;

                matrix.Set(i, j, bitArray[index]);
            }
            return matrix;
        }

        private static byte[] EncodeToBytes(this BitMatrix matrix)
        {
            BitArray bitArray = new BitArray(matrix.GetBits().ToArray());
            byte[] headerBytes = BitConverter.GetBytes(bitArray.Length);
            byte[] bytes = new byte[sizeof(int) + bitArray.Length / 8 + 1];
            headerBytes.CopyTo(bytes, 0);
            bitArray.CopyTo(bytes, headerBytes.Length);
            return bytes;
        }

        public static BitMatrix FromBase64(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            BitMatrix matrix = FromBytes(bytes);
            return matrix;
        }

        public static IEnumerable<bool> GetBits(this BitMatrix matrix)
        {
            for (int i = 0; i < matrix.Width; i++)
                for (int j = 0; j < matrix.Width; j++)
                {
                    yield return matrix[j, i];
                }
        }

        public static bool IsEquivalentTo(this BitMatrix first, BitMatrix second)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            if (first.Width != second.Width)
            {
                return false;
            }

            for (int i = 0; i < first.Width; i++)
                for (int j = 0; j < first.Width; j++)
                {
                    if (first[i, j] != second[i, j])
                    {
                        return false;
                    }
                }
            return true;
        }
    }
}
