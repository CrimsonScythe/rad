using System;
using System.Numerics;
using System.Collections.Generic;
using System.Diagnostics;

// NOTE RUN WITH the command (100000 values to be generated where all (100000) are distinct):
// dotnet run 100000 100000

namespace rad
{
    public enum HashFuncType {shift, mod}
    class Program
    {
        public static string bytes;
        static void Main(String[] args)
        {
            // randombytes.txt downloaded from random.org is a string of random bits
            // here we simply read the file and store it as one huge string.
            // the bytes used by the algorithms are then extracted from this variable
            bytes = System.IO.File.ReadAllText("randombytes.txt");

            // HashTable hashTable = new HashTable();
            // Console.WriteLine(hashTable.get(5));
            // hashTable.set(5,10);
            // Console.WriteLine(hashTable.get(5));
            // hashTable.increment(5, 1);
            // Console.WriteLine(hashTable.get(5));

            

            // Exercise1(Int32.Parse(args[0]), Int32.Parse(args[1]));

            // Exercise3(HashFuncType.mod);

            // Exercise6(1000, 20);

            
            Exercise7();

            

        }

        static void Exercise7() {
                
            int n = 10000;
            int l = 25;
            var epsilon = 0.001;

            var stream = Generator.CreateStream(n,l);

            // calculate S from hashing with chaining from part 1
            // bascially we get the exact value of n i.e. 10000
            SFunc(stream, l, HashFuncType.mod);
            
            int index=0;
            double MSE=0;
            // the value S is in reality just the number of items in the data stream
            int S = n;

            for (int i = 0; i < 100; i++){
                // i    = 0,1,2,3...
                // index= 0,4,8,12
                index = i*4;
                // "[...] Beregn Count-Sketch af datastrømmen [...] Beregn estimateren X [...]"
                double estimate = Algorithms.CountSketch(stream, epsilon, index);
                Console.WriteLine(estimate);
                // we compute the mean-square error
                MSE += Math.Pow(estimate - S, 2);
            }

            // we compute the mean-square error
            MSE /= 100;
            Console.WriteLine("mean-squared error:" + MSE);
        }

        static void Exercise6(Int32 n, Int32 l)
        {
            IEnumerable<Tuple<ulong, int>> stream = Generator.CreateStream(n, l);

            var SecondMoment = Algorithms.CountSketch(stream, 1, 0);
            Console.WriteLine(SecondMoment);
        }
        
        
        static void Exercise3(HashFuncType type) {
            // l=1 .. 25
            // n=1000000
           
            int n=1000000;
            List<int> llist = new List<int> {5,10,15,20,25};
            var watch = new Stopwatch();
        

            foreach (int lval in llist){
                watch.Start();
                SFunc(Generator.CreateStream(n, lval), lval, type);
                watch.Stop();
                Console.WriteLine("type: " +type + " l is: " + lval+ " time: " + watch.ElapsedMilliseconds);
                watch.Reset();
            }
            
            
        }

        static void Exercise1(Int32 n, Int32 l) {


            IEnumerable<Tuple<ulong, int>> stream = Generator.CreateStream(n, l);

            BigInteger sum = new BigInteger(0);

            var watch = Stopwatch.StartNew();
    
            foreach (Tuple<ulong, int> item in stream)
            {
                sum = HashFunctions.multiplyShift(item.Item1, l) + sum;     
            }
            
            watch.Stop();

            Console.WriteLine("MultiplyShift sum:" + sum);
            Console.WriteLine("MultiplyShift elapsed:" + watch.ElapsedMilliseconds);

            sum=0;            

            watch.Restart();
            foreach (Tuple<ulong, int> item in stream)
            {
                sum = HashFunctions.multiplyMod(item.Item1, l) + sum;     
            }
            watch.Stop();
            Console.WriteLine("MultiplyMod sum:" + sum);
            Console.WriteLine("MultiplyMod elapsed:" + watch.ElapsedMilliseconds);
            
            
        }

        static void SFunc(IEnumerable<Tuple<ulong , int>> stream, int l, HashFuncType funcType) {
            // we begin by storing the key value pairs in the hash table using the hashfunctions
            HashTable hashTable = new HashTable(l, funcType);
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

