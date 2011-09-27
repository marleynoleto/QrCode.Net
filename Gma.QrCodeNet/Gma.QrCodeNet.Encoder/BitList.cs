using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gma.QrCodeNet.Encoding
{
    public sealed class BitList : IList<bool>
    {
        private readonly BitArray m_BitArray;

        public BitList()
        {
            m_BitArray = new BitArray(0);
        }

        public IEnumerator<bool> GetEnumerator()
        {
            return m_BitArray.Cast<bool>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(bool item)
        {
            m_BitArray.Length = m_BitArray.Count + 1;
            m_BitArray[m_BitArray.Length - 1] = item;
        }

        public void Add(int value, int bitCount)
        {
            for (int i = 0; i < bitCount; i++)
            {
                int mask = 1 << i;    
                this.Add((value & mask) != 0);
            }
        }

        public void Clear()
        {
            m_BitArray.Length = 0;
        }

        public bool Contains(bool item)
        {
            return ((IEnumerable<bool>)this).Contains(item);
        }

        public void CopyTo(bool[] array, int arrayIndex)
        {
            int startIndex = arrayIndex;
            foreach (bool element in this)
            {
                array[startIndex] = element;
                startIndex++;
            }
        }

        public bool Remove(bool item)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return m_BitArray.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(bool item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, bool item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public bool this[int index]
        {
            get { return m_BitArray[index]; }
            set { m_BitArray[index]=value; }
        }
    }
}
