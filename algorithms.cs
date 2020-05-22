using System.Numerics;
using System;
using System.Collections.Generic;

namespace rad{
    class Algorithms{

        public static UInt64 Algorithm1(UInt64 x) {
            // to get the a's which must be random in [p], we use random.org/bytes
            // a need to be 89 bits long each, so we generate a 12 byte number and throw away the first 7 bits to get 89 bits
            // 10111110 01000110 01000011 10011110 10011000 01000111 11100001 11111100 01010110 11101101 10000011 0
            // 10101110 01000001 10100001 00101111 00001111 11010111 10000100 01011010 11010001 10110111 01111100 1
            // 01011100 00010110 11101001 00111000 10101010 00001100 01000011 00100001 11001011 00010111 11101110 0
            // 01010000 00100110 01100111 01101000 11111001 10111001 10000000 10010100 11101111 10100111 00101111 1
            
            UInt64 p = (UInt64)(BigInteger.Pow(2, 89)-1);
            int b = 89;
            List<BigInteger> a = new List<BigInteger>();
            a.Add(BigInteger.Parse("460055437480792894556986118"));
            a.Add(BigInteger.Parse("421326039502587756936392441"));
            a.Add(BigInteger.Parse("222658739283255370454544348"));
            a.Add(BigInteger.Parse("193790846148879967259151967"));
            int q = a.Capacity;


            BigInteger y = a[q-1];
            for(int i = q-2; i > 0; i--) {
                y = y*x+a[i];
                y = (y&p) + (y>>b);
            }
            if (y >= p){
                y = y - p;
            }
            return (UInt64) y;
        }

        public static (UInt64, UInt64) Algorithm2(UInt64 x, Func<UInt64, UInt64> g, int t = 64) {
            /**
            Questions: The algorithm 2 described in the notes, doesn't seem to encompass the whole
            function described in task 5. Rather Task 5 uses algorithm 2 together with some additional
            code to achieve the functionality of the function described in the task. For now, I'll expand
            algorithm 2, this function, to also encompass the function described in task 5.

            Algorithm 2, page 6, "Second moment estimation" (Thorup)

            <param name="x">
                Key x belonging to/in [u]. 
            </param>
            <param name="t">
                The k to use in the calculation of m = 2^k. Default value is 64.
            </param>
            <param name="g">
                4-universal hash function to use (e.g. algorithm 1), 
                which takes a UInt64 key and returns hash value
            </param>
            **/
            
            // Accord. to task 5: "Let m = 2^t <= 2^64"
            // UInt64 m = (UInt64)BigInteger.Pow(2, t);
            
            // b is set to 89 bits according to p. 5 of "Implementeringsprojekt.pdf"
            // and is related to p. 
            int b = 89;

            UInt64 gx = g(x);
            // Unsure whether to use m here, or t (k) = log2 m = 64 in this instance (assuming t (k) = 64)
            UInt64 hx = gx&((ulong)t-1);
            UInt64 bx = gx >> (b-1);
            UInt64 sx = 1 - 2*bx;

            return(hx, sx);
        }



        public static void CountSketch() {
            // /**
            // "Basic Count Sketch for Second Moment",
            // from 2moment-lect.pdf, p. 16, sl. 5. 
            // **/

            // //// BCS-INITIALIZE part ////

            // // Is epsilon a parameter?
            // UInt64 k = Ceiling(8/Pow(epsilon, 2);
            
            // // Pick 4-universal s and h 
            // // (these are set by algorithm2 and can't be picked)

            // // C[0, ..., k-1] <-- 0
            // List<UInt64> C = Enumerable.Repeat(0, k-1).ToList();
            
            // //// BCS-PROCESS part ////
            // // Where do we get the x from?? Can't be a parameter for CountSketch, as we're probably expecting a whole stream.. or??
            // // Is all this for one key or for a stream??
            // var hashValues = Algorithm2(x, Algorithm1, t=64)
            // hx = hashValues[0]
            // sx = hashValues[1]
            // // What is delta??
            // // So, this should probably be in a loop of some sort, going over a stream of keys right??
            // C[hx] = C[hx] + sx * delta
            

            // //// BCS-2ND-MOMENT ////
            // // F^hat_2. 
            // int secondMoment = 0

            // for (int i = 0; i < k+1; i++) {
            //     secondMoment = secondMoment + Pow(C[i], 2)
            // }



            
        }        

    }
}