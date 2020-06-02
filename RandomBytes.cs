using System;
using System.Collections;
using System.Linq;
using System.Numerics;

namespace rad
{


    
    public class RandomBytes : IEnumerable
    {

        public static IEnumerator _enumerator; 

        private string _bytes;

        public RandomBytes(string bytes)
        {
            _bytes = bytes;
            var _enum = (IEnumerator) GetEnumerator();
            _enumerator = _enum;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _enumerator;
        }

        public RandomBytesEnum GetEnumerator()
        {
            return new RandomBytesEnum(_bytes);
        }
    }

    public class RandomBytesEnum : IEnumerator
    {
        public string _bytes;
        
        int position = -1;

        public RandomBytesEnum(string bytes)
        {
            _bytes = bytes;
        }

        public bool MoveNext()
        {
            position += 90;
            return (position < _bytes.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }
        
        public string Current
        {
            get
            {
                try
                {
                    //Console.WriteLine(position + " / " + _bytes.Length);
                    string byteString = _bytes.Substring(position, 90);
                    return byteString;
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}

