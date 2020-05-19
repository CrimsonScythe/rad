using System.Numerics;
using System;
using System.Collections.Generic;

namespace rad{
    class HashFunctions{

        public static UInt64 multiplyShift(UInt64 x) {
            // If the last digit of a binary number is 1, the number is odd
            // binary : 11111110 00111100 00101001 10110010 00001001 10001110 01001001 00110001
            // decimal : 18319563228877572401
            // h(x) = (a*x)>>(64-l)

            // l needs to be < 64
            int l = 20;
            UInt64 a = 18319563228877572401UL;

            UInt64 h = a*x;
            return h >> (64-l);
            
        }

        public static UInt64 multiplyMod(UInt64 x) {
            // h(x) = ((a*x+b) mod p) mod 2^l

            int q = 89;
            
            UInt64 p = (UInt64)(Math.Pow(2, q) - 1);
            int l = 20;
            UInt64 m = (UInt64)(Math.Pow(2, l));
            // computes (a*x+b) mod p fast
            // as x < 2q we can use the code in the interm function to compute
            // deciaml values are generated from random.org
            BigInteger a = BigInteger.Parse("233803183637780797534382925");
            BigInteger b = BigInteger.Parse("308071719960255608440264707");
            var res = (a*x+b);
            UInt64 inter = interim(res,p,q);
            // computes y mod 2^l frim slides
            return inter&(m-1);

        }

        static UInt64 interim(BigInteger x, BigInteger p, int q) {
            BigInteger y = (x&p) + (x >> q);
            if (y >= p){
                y = y - p;
            }
            return (UInt64) y;
        }

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

        public static (UInt64, UInt64) Algorithm2(UInt64 x) {
            
            UInt64 m = (UInt64)BigInteger.Pow(2, 64);
            int b = 89;

            UInt64 gx = Algorithm1(x);
            UInt64 hx = gx&(m-1);
            UInt64 bx = gx >> (b-1);
            UInt64 sx = 1 - 2*bx;

            return(hx, sx);

        }

    }
}