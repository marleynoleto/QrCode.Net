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
            Resize(initialCount + bitCount);
            initialCount = AddIEnumerable(values, initialCount);
            if(initialCount == this.Count)
            	return this;
            else
            	throw new ArgumentException(string.Format("InitialCount:{0}, ThisCount:{1}", initialCount, this.Count));
        }
        
        private int AddIEnumerable(IEnumerable<bool> values, int initialCount)
        {
        	int i = 0;
        	foreach(bool value in values)
        	{
        		this[initialCount + i] = value;
        		i++;
        	}
        	return initialCount + i;
        }
        
        
        public BitList Add(byte[] values, int numBytes)
        {
        	int initialCount = this.Count;
        	Resize(initialCount + (numBytes << 3));
        	foreach(byte value in values)
        	{
        		IEnumerable<bool> bits = GetBits((int)value, 8);
        		initialCount = AddIEnumerable(bits, initialCount);
        	}
        	
        	if(initialCount == this.Count)
        		return this;
        	else
        		throw new ArgumentException(string.Format("InitialCount:{0}, ThisCount:{1}", initialCount, this.Count));
        }
        
        /// <summary>
        /// Method for Terminator class. 
        /// Add padding codeword at end of BitList.
        /// </summary>
        /// <param name="byteCount">Number of padding</param>
        internal BitList AddPadding(int byteCount)
        {
        	if(byteCount < 0)
				throw new ArgumentException("Num of pade codewords less than Zero");
        	int initialCount = this.Count;
        	Resize(initialCount + (byteCount << 3));
			for(int numOfP = 1; numOfP <= byteCount; numOfP++)
			{
				if(numOfP % 2 == 1)
					initialCount = this.AddBoolArray(QRCodeConstantVariable.PadeOdd, initialCount);
				else
					initialCount = this.AddBoolArray(QRCodeConstantVariable.PadeEven, initialCount);
			}
			
			if(initialCount != this.Count)
				throw new ArgumentException(string.Format("InitialCount:{0}, ThisCount:{1}", initialCount, this.Count));
			else
				return this;
        }
        
        /// <summary>
        /// Sub method for AddPadding. It will use initialCount from AddPadding as actually data size. 
        /// </summary>
        /// <param name="pad">Pad codewords from QRCodeConstantVariable, or bool array. </param>
        private int AddBoolArray(bool[] pad, int initialCount)
        {
        	int i = 0;
        	foreach(bool value in pad)
        	{
        		this[initialCount + i] = value;
        		i++;
        	}
        	return initialCount + i;
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
