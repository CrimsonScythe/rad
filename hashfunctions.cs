using System.Numerics;
using System;

namespace rad{
    class HashFunctions{

        public static UInt64 multiplyShift(UInt64 x) {
            // If the last digit of a binary number is 1, the number is odd
            // binary : 11111110 00111100 00101001 10110010 00001001 10001110 01001001 00110001
            // decimal : 18319563228877572401
            // h(x) = (a*x)>>(64-l)

            // l needs to be < 64
            int l = 30;
            UInt64 a = 18319563228877572401UL;

            UInt64 h = a*x;
            return h >> (64-l);

            // int l = 63;
            // UInt64 a = 18319563228877572401;

            // UInt64 h = a*x;
            // return h >> (64-l);

            
        }

        public static BigInteger multiplyMod(BigInteger x) {
            // h(x) = ((a*x+b) mod p) mod 2^l

            int q = 89;
            
            BigInteger p = new BigInteger(Math.Pow(2, q) - 1);
            int l = 63;
            BigInteger m = new BigInteger(Math.Pow(2, l));
            // computes (a*x+b) mod p fast
            // as x < 2q we can use the code in the interm function to compute
            // deciaml values are generated from random.org
            BigInteger a = BigInteger.Parse("233803183637780797534382925");
            BigInteger b = BigInteger.Parse("308071719960255608440264707");
            x = (a*x+b);
            BigInteger inter = interim(x,p,q);
            // computes y mod 2^l frim slides
            return inter&(m-1);

        }

        static BigInteger interim(BigInteger x, BigInteger p, int q) {
            BigInteger y = (x&p) + (x >> q);
            if (y >= p){
                y = y - p;
            }
            return y;
        }

    }
}