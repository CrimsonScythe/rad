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
            // HashTable hashTable = new HashTable();
            // Console.WriteLine(hashTable.get(5));
            // hashTable.set(5,10);
            // Console.WriteLine(hashTable.get(5));
            // hashTable.increment(5, 1);
            // Console.WriteLine(hashTable.get(5));

            int n=1000000;
            int l=20;
            SFunc(Generator.CreateStream(n, l));

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

        static void SFunc(IEnumerable<Tuple<ulong , int>> stream) {
            // we begin by storing the key value pairs in the hash table using the hashfunctions
            HashTable hashTable = new HashTable();
            // the loop below computes s(x)
            foreach(Tuple<ulong, int> pair in stream){
                
                if (hashTable.get(pair.Item1)==0){ // key does not exist
                    hashTable.set(pair.Item1, pair.Item2); // put key value pair in hashtable
                } else{ // key exists
                    hashTable.increment(pair.Item1, pair.Item2); // add value to already existing key value
                }
            }

            // next we add the s(x)^2 above to compute S, as required
            UInt64 sum = 0UL;
            foreach(List<MutableKeyValuePair<int, int>> lst in hashTable.hashT){
                foreach(MutableKeyValuePair<int,int> pair in lst){
                    sum += (ulong) Math.Pow(pair.Value, 2);
                }
            }

            Console.WriteLine("S is:" + sum);
        }

    }
}

