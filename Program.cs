using System;
using System.Numerics;
using System.Collections.Generic;
using System.Diagnostics;

// NOTE RUN WITH the command (100000 values to be generated where all are distinct):
// dotnet run 100000 100000
// second function has runtime 21s, while the first has 31s
namespace rad
{
    class Program
    {
        static void Main(String[] args)
        {
            IEnumerable<Tuple<ulong, int>> stream = CreateStream(Int32.Parse(args[0]), Int32.Parse(args[1]));
            var watch = Stopwatch.StartNew();
            
            foreach (Tuple<ulong, int> item in stream)
            {
                Console.WriteLine(multiplyShift(new BigInteger(item.Item1)));     
            }
            
            watch.Stop();
            var prevTime = watch.ElapsedMilliseconds;
            

            watch.Restart();
            foreach (Tuple<ulong, int> item in stream)
            {
                Console.WriteLine(multiplyMod(new BigInteger(item.Item1)));
            }
            watch.Stop();
            Console.WriteLine("elapsed" + prevTime);
            Console.WriteLine("elapsed" + watch.ElapsedMilliseconds);
        }

        static BigInteger multiplyShift(BigInteger x) {
            // If the last digit of a binary number is 1, the number is odd
            // binary : 11111110 00111100 00101001 10110010 00001001 10001110 01001001 00110001
            // decimal : 18319563228877572401
            // h(x) = (a*x)>>(64-l)


            int l = 63;
            BigInteger a = new BigInteger(18319563228877572401);

            BigInteger h = a*x;
            return h >> (64-l);

            
        }

        static BigInteger multiplyMod(BigInteger x) {
            // h(x) = ((a*x+b) mod p) mod 2^l

            int q = 89;
            BigInteger p = new BigInteger(2^q - 1);
            int l = 63;
            BigInteger m = new BigInteger(2^l);
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

        public static IEnumerable<Tuple<ulong , int>> CreateStream(int n , int l ) {
    // We generate a random uint64 number .
    Random rnd = new System.Random ();
    ulong a = 0UL;
    Byte [] b = new Byte [8];
    rnd.NextBytes ( b );
    
    for(int i = 0; i < 8; ++ i ) {
        a = ( a << 8) + ( ulong ) b [ i ];
        }
// We demand that our random number has 30 zeros on the least
// significant bits and then a one.

    a = ( a | ((1UL << 31) - 1UL ) ) ^ ((1UL << 30) - 1UL );
    ulong x = 0UL;

    for( int i = 0; i < n /3; ++ i ) {
        x = x + a ;
        yield return Tuple . Create ( x & (((1UL << l ) - 1UL ) << 30) , 1);
    }

    for( int i = 0; i < ( n + 1) /3; ++ i ) {
        x = x + a;
        yield return Tuple . Create ( x & (((1UL << l ) - 1UL ) << 30) , -1);
    }

    for( int i = 0; i < ( n + 2) /3; ++ i ) {
    x = x + a ;
    yield return Tuple . Create ( x & (((1UL << l ) - 1UL ) <<
    30) , 1) ;
    }
}

    }
}

