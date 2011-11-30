using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gma.QrCodeNet.Encoding
{
    public sealed class BitList : IEnumerable<bool>
    {
        private readonly BitArray m_BitArray;

        public BitList()
        {
            m_BitArray = new BitArray(0);
        }

        public BitList(bool[] values)
        {
           m_BitArray = new BitArray(values);
        }
        
        public IEnumerator<bool> GetEnumerator()
        {
            return m_BitArray.Cast<bool>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public BitList Add(bool item)
        {
            this.Resize(this.Count + 1);
            m_BitArray[this.Count - 1] = item;
            return this;
        }

        public BitList Add(bool[] values)
        {
            return this.Add(values, values.Length);
        }

        public BitList Add(int value, int bitCount)
        {
            IEnumerable<bool> bits = GetBits(value, bitCount);
            return this.Add(bits, bitCount);
        }

        public BitList Add(IEnumerable<bool> values)
        {
            return this.Add(values, values.Count());
        }

     
        public BitList Add(IEnumerable<bool> values, int bitCount)
        {
            int initialCount = this.Count;
            Resize(this.Count + bitCount);
            int i = 0;
            foreach (var value in values)
            {
                this[initialCount + i] = value;
                i++;
            }
            return this;
        }

        private void Resize(int newLength)
        {
            if (m_BitArray.Length<newLength)
            {
                m_BitArray.Length = newLength;
            }
        }

        private static IEnumerable<bool> GetBits(int value, int bitCount)
        {
            if (bitCount < 0 || bitCount > 32)
            {
                throw new ArgumentOutOfRangeException("bitCount", bitCount, "Number of bits must be between 0 and 32");
            }

            for (int shift = bitCount - 1; shift >= 0; shift--)
            {
                bool bit = ((value >> shift) & 1) != 0;
                yield return bit;
            }
        }

        public void Clear()
        {
            m_BitArray.Length = 0;
        }
      
        public int Count
        {
            get { return m_BitArray.Count; }
        }

        public bool this[int index]
        {
            get { return m_BitArray[index]; }
            set { m_BitArray[index]=value; }
        }
    }
}
