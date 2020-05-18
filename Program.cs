using System;
using System.Numerics;
using System.Collections.Generic;
using System.Diagnostics;

// NOTE RUN WITH the command (100000 values to be generated where all (100000) are distinct):
// dotnet run 100000 100000

namespace rad
{
    class Program
    {
        static void Main(String[] args)
        {
            HashTable hashTable = new HashTable();
            Console.WriteLine(hashTable.get(5));

            // HashTable hashTable = new HashTable(Int32.Parse(args[0]));
            // hashTable.get(58);

            // IEnumerable<Tuple<ulong, int>> stream = Generator.CreateStream(Int32.Parse(args[0]), Int32.Parse(args[1]));

            // BigInteger sum = new BigInteger(0);

            // var watch = Stopwatch.StartNew();
    
            // foreach (Tuple<ulong, int> item in stream)
            // {
            //     sum = HashFunctions.multiplyShift(new BigInteger(item.Item1)) + sum;     
            // }
            
            // watch.Stop();

            // Console.WriteLine("value:" + sum);
            // Console.WriteLine("elapsed:" + watch.ElapsedMilliseconds);

            // sum=0;            

            // watch.Restart();
            // foreach (Tuple<ulong, int> item in stream)
            // {
            //     sum = HashFunctions.multiplyMod(new BigInteger(item.Item1)) + sum;     
            // }
            // watch.Stop();
            // Console.WriteLine("value:" + sum);
            // Console.WriteLine("elapsed:" + watch.ElapsedMilliseconds);
        }

    }
}

