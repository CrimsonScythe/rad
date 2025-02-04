using System.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rad{
    class Algorithms
    {
        
        public static RandomBytes randomBytes = new RandomBytes(System.IO.File.ReadAllText("randombytes.txt"));
        public static IEnumerator randomBytesEnum = randomBytes.GetEnumerator();
        
        public static byte[] GetBytes(string bitString) {
    return Enumerable.Range(0, bitString.Length/8).
        Select(pos => Convert.ToByte(
            bitString.Substring(pos*8, 8),
            2)
        ).ToArray();
}

        /// <summary>
         ///   extracts 4 random bytes each of length 89 from the random byte string initialized in Program.cs (variable name:bytes) 
         /// </summary>
         /// <param name="index">
         ///  Takes int number as input (index to be used as a starting point to extract the 4 bytes)
         /// </param>
         /// <returns>
         ///   tuple of the 4 bytes
         /// </returns>
         ///

        /*
        public static (BigInteger, BigInteger, BigInteger, BigInteger) randomBytes(int index) {
            // make sure index argument increases in 4s like so: 0, 4, 8..
            // we get a reference to the string. This is so we don't have to read the .txt everytime
            var bytes = Program.bytes;
            int[] indices = {index, index+1, index+2, index+3};
            // we simply extract 4 bytes each of length 89.
            // we rely on the fact that index is of the size 0,4,8.. so that everytime this is called
            // we get 4 different substrings
            var splitted = bytes.Substring(90*indices[0],90);
            var splitted2 = bytes.Substring(90*indices[1], 90);
            var splitted3 = bytes.Substring(90*indices[2], 90);
            var splitted4 = bytes.Substring(90*indices[3], 90);

            
            // convert strings to byte arrays and then create bigints from those
            // converting to decimal values caused problems with uint64 being too small
            // this probably does not make sense but appending {0x0} to the end
            // makes the bigint unsigned otherwise there were problems with overflow
            // when returing y from alg2, as y is UInt64 and the result was -1.
        
            var byte0 = new BigInteger(GetBytes(splitted).Concat(new byte[]{0}).ToArray());
            var byte1 = new BigInteger(GetBytes(splitted2).Concat(new byte[]{0}).ToArray());
            var byte2 = new BigInteger(GetBytes(splitted3).Concat(new byte[]{0}).ToArray());
            var byte3 = new BigInteger(GetBytes(splitted4).Concat(new byte[]{0}).ToArray());
            
            // Console.WriteLine(byte0);
            // Console.WriteLine(byte1);
            // Console.WriteLine(byte2);
            // Console.WriteLine(byte3);

            // var byte0 = new BigInteger(Encoding.ASCII.GetBytes(splitted));
            // var byte1 = new BigInteger(Encoding.ASCII.GetBytes(splitted2));
            // var byte2 = new BigInteger(Encoding.ASCII.GetBytes(splitted3));
            // var byte3 = new BigInteger(Encoding.ASCII.GetBytes(splitted4));
           
            return (byte0, byte1, byte2, byte3);
        }
        */
        
        public static string BinToDec(string value)
        {
            // BigInteger can be found in the System.Numerics dll
            BigInteger res = 0;

            // I'm totally skipping error handling here
            foreach(char c in value)
            {
                res <<= 1;
                res += c == '1' ? 1 : 0;
            }

            return res.ToString();
        }
        
         /// <summary>
         ///   Algorithm 1 / g(x) as described in the "Second moment estimation" notes, p. 6. 
         /// </summary>
         /// <param name="x">
         ///  Takes UInt64 number as input (key to be hashed)
         /// </param>
         /// <returns>
         ///   Returns hashed value
         /// </returns>
         ///
        public static BigInteger Algorithm1(UInt64 x, List<BigInteger> a) {
            /*
                To get the a's which must be random in [p], we use random.org/bytes
                a need to be 89 bits long each, so we generate a 12 byte number and throw away the first 7 bits to get 89 bits
                10111110 01000110 01000011 10011110 10011000 01000111 11100001 11111100 01010110 11101101 10000011 0
                10101110 01000001 10100001 00101111 00001111 11010111 10000100 01011010 11010001 10110111 01111100 1
                01011100 00010110 11101001 00111000 10101010 00001100 01000011 00100001 11001011 00010111 11101110 0
                01010000 00100110 01100111 01101000 11111001 10111001 10000000 10010100 11101111 10100111 00101111 1
            */
            
            // UInt64 p = (UInt64)(Math.Pow(2, 89)-1);
            BigInteger p = new BigInteger(Math.Pow(2, 89)) - 1;
            
            int b = 89;

            // we get random bytes correspding to the current index.
            // the idea is that the index will change on each successive call to 
            // Algorithm1 so we get completely *new* random bytes everytime
            // the bytes are actually in an array so we want to start the index at a new spot everytime.
            // thus the index needs to be incremented by 4 everytime Algorithm1 is called
            //var tuple = randomBytes(index);
            // then we append them to the array
            /*
            a.Add(tuple.Item1);
            a.Add(tuple.Item2);
            a.Add(tuple.Item3);
            a.Add(tuple.Item4);
            */
            /*
            a.Add(BigInteger.Parse("193790846148879967259151967"));	  
            a.Add(BigInteger.Parse("193790846148879967259151967"));
            a.Add(BigInteger.Parse("193790846148879967259151967"));       
            a.Add(BigInteger.Parse("193790846148879967259151967"));	  	      
            */
            
            int q = a.Capacity;
            // -1 because we're 0 indexed
            BigInteger y = a[q-1];
            for(int i = q-2; i >= 0; i--) {
                y = y*x + a[i];         
                y = ( y & p ) + ( y >> b );
            }
            // Console.WriteLine(y);
            // Console.WriteLine((Int64)p);
            if (y >= p){
                // Console.WriteLine("yes");
                y = y - p;
            }
            
            // Console.WriteLine(p);
            // Console.WriteLine(y);
            return y;
        }
        
        /// <summary>
        /// Algorithm 2, page 6, "Second moment estimation" (Thorup)
        /// </summary>
        /// <param name="x">
        ///             Key x in [u]. 
        /// </param>
        /// <param name="g">
        ///            4-universal hash function to use (e.g. algorithm 1), 
        ///            which itself takes a UInt64 key and returns hash value
        /// </param>
        public static (Int64, Int64) Algorithm2(UInt64 x, Func<UInt64, List<BigInteger>, BigInteger> g, List<BigInteger> a, int t)
        {


            BigInteger m = BigInteger.Pow(2, t);
            
            // b is set to 89 bits according to p. 5 of "Implementeringsprojekt.pdf"
            // as 2^(89)-1 creates a Mersenne prime number. 
            int b = 89;

            BigInteger gx = g(x, a);
            // Console.WriteLine("gx:" + gx);
            BigInteger hx = gx&(m-1);
            // Console.WriteLine("hx:" + hx);
            BigInteger bx = gx >> (b-1);
            // Console.WriteLine("bx:" + bx);
            BigInteger sx = 1 - 2*bx;
            // Console.WriteLine("sx:" + sx);
            // Console.WriteLine();

            return((Int64) hx, (Int64) sx);
        }

        /// <summary>
        /// "Basic Count Sketch for Second Moment",
        /// from 2moment-lect.pdf, p. 16, sl. 5.
        /// </summary>
        /// <param name="stream">Stream of keys generated by Generator.CreateStream.</param>
        /// <param name="epsilon">error factor (according to web)</param>
        /// <param name="index">this ensures that the random bytes used are unique in each call to CountSketch.
        /// This should start from 0 and be incremented by 4 everytime. i.e: 0,4,8</param>
        public static UInt64 CountSketch(IEnumerable<Tuple<ulong , int>> stream, int t) {
            // compute t
            // 2^t = 8/epsilon^2
            // t = log_{2}(8/epsilon^2)
            

            // //// BCS-INITIALIZE part ////
            
            //Int64 k = (Int64) Math.Ceiling(8/Math.Pow(epsilon, 2));
            
            List<BigInteger> a = new List<BigInteger>();
            randomBytesEnum.MoveNext();
            string bitsString = (string) randomBytesEnum.Current;
            a.Add(BigInteger.Parse(BinToDec(bitsString)));
            
            randomBytesEnum.MoveNext();
            bitsString = (string) randomBytesEnum.Current;
            a.Add(BigInteger.Parse(BinToDec(bitsString)));
            
            randomBytesEnum.MoveNext();
            bitsString = (string) randomBytesEnum.Current;
            a.Add(BigInteger.Parse(BinToDec(bitsString)));
            
            randomBytesEnum.MoveNext();
            bitsString = (string) randomBytesEnum.Current;
            a.Add(BigInteger.Parse(BinToDec(bitsString)));
            
            Int64 m = (Int64) Math.Ceiling(Math.Pow(2, t));
            

            // // C[0, ..., k-1] <-- 0
            List<Int64> C = new List<Int64>();
            for (Int64 i = 0; i < m; i++) {
                C.Add(0);
            }
            
            // //// BCS-PROCESS part ////-
            foreach (Tuple<ulong, int> pair in stream)
            {
                var hashValues = Algorithm2(pair.Item1, Algorithm1, a, t);
                var hx = hashValues.Item1;
                var sx = hashValues.Item2;
                
                
                var delta = (long) pair.Item2;
                C[(int) hx] = C[(int) hx] + sx * delta;
            }
            
            // //// BCS-2ND-MOMENT ////
            // // F^hat_2. 
            UInt64 secondMoment = 0;

            
            for (int i = 0; i < (int)m; i++)
            {
                secondMoment = secondMoment + (UInt64) Math.Pow(C[i], 2);
            }

            return secondMoment;
        }
    }
}