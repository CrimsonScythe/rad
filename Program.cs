﻿using System;
using System.Numerics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

            
            var stream = Generator.CreateStream(50000, 25);
            // testing the running times
            Exercise1(stream, 25);
            Exercise4(stream);

            // Exercise2();
            
            // Exercise3(HashFuncType.mod);

            // Exercise6(1000, 20);
            
            var stream1 = Generator.CreateStream(10000,25);
            AnnouncementPart1(stream1);

            var stream2 = Generator.CreateStream(100000,12);
            AnnouncementPart2(stream2);



        }

        static void Exercise2()
        {
            /*
             * Show that we have implemented hashtabel with chaining and that it supports get, set and increment.
             */
            HashTable hashTable = new HashTable(25, HashFuncType.mod);
            Console.WriteLine(hashTable.get(5));
            hashTable.set(5,10);
            Console.WriteLine(hashTable.get(5));
            hashTable.increment(5, 1);
            Console.WriteLine(hashTable.get(5));
        }
        static void AnnouncementPart2(IEnumerable<Tuple<ulong , int>> stream)
        {
            var answers = PerformCountSketch(stream, 4);
            var estimatesSorted = new List<double>(answers.estimatesUnsorted);
            estimatesSorted.Sort();
            WriteToCSV(answers.estimatesUnsorted, "estimates_unsorted_m16.csv");
            WriteToCSV(estimatesSorted, "estimates_sorted_m16.csv");
            WriteToCSV(answers.medians, "medians_m16.csv");
            
            t = (int) Math.Log2(2/(Math.Pow(epsilon, 2)));
            answers = PerformCountSketch(stream, 7);
            estimatesSorted = new List<double>(answers.estimatesUnsorted);
            estimatesSorted.Sort();
            WriteToCSV(answers.estimatesUnsorted, "estimates_unsorted_m128.csv");
            WriteToCSV(estimatesSorted, "estimates_sorted_m128.csv");
            WriteToCSV(answers.medians, "medians_m128.csv");
            
            epsilon = 0.001;
            t = (int) Math.Log2(1/(Math.Pow(epsilon, 2)));
            answers = PerformCountSketch(stream, 10);
            estimatesSorted = new List<double>(answers.estimatesUnsorted);
            estimatesSorted.Sort();
            WriteToCSV(answers.estimatesUnsorted, "estimates_unsorted_m1024.csv");
            WriteToCSV(estimatesSorted, "estimates_sorted_m1024.csv");
            WriteToCSV(answers.medians, "medians_m1024.csv");
        }


        static void AnnouncementPart1(IEnumerable<Tuple<ulong , int>> stream)
        {
            // HERE WE RUN 3 EXPERIMENTS WITH l = 25, and m=16, m=128, m=1024. 
            var epsilon = 0.001;
            //int t = (int) Math.Log2(8/(Math.Pow(epsilon, 2))); 

            // EXPERIMENT 1, l = 25, m = 16, therefore t=4
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


            watch.Restart()
            double estimate = Algorithms.CountSketch(stream, epsilon, 0, t)
            watch.Stop()
            Console.WriteLine("[Experiment 1][l=25, m=16, t=4]")
            Console.WriteLine("Estimate:" + estimate)
            Console.WriteLine("Elapsed time:" + watch.ElapsedMilliseconds)

            watch.Restart()
            watch.Start()
            double estimate = Algorithms.CountSketch(stream, epsilon, 4, t)
            watch.Stop()
            Console.WriteLine("[Experiment 2][l=25, m=128, t=7]")
            Console.WriteLine("Estimate:" + estimate)
            Console.WriteLine("Elapsed time:" + watch.ElapsedMilliseconds)

            watch.Restart()
            watch.Start()
            double estimate = Algorithms.CountSketch(stream, epsilon, 8, t)
            watch.Stop()
            Console.WriteLine("[Experiment 3][l=25, m=1024, t=10]")
            Console.WriteLine("Estimate:" + estimate)
            Console.WriteLine("Elapsed time:" + watch.ElapsedMilliseconds)
        }
        static (List<double> estimatesUnsorted, double MSE, double mean, List<double> medians) PerformCountSketch(IEnumerable<Tuple<ulong , int>> stream, int t) {
            var epsilon = 0.001;
            var l = 12;
            
            // calculate S from hashing with chaining from part 1
            // bascially we get the exact value of n i.e. 10000
            SFunc(stream, l, HashFuncType.shift);
            
            int index=0;
            double MSE=0;
            double mean=0;
            // the value S is in reality just the number of items in the data stream
            int S = n;
            
            List<double> estimatesUnsorted = new List<double>();

            for (int i = 0; i < 100; i++){
                // i    = 0,1,2,3...
                // index= 0,4,8,12
                index = i*4;
                // "[...] Beregn Count-Sketch af datastrømmen [...] Beregn estimateren X [...]"
                // This is done in one go by our count sketch algorithm. It IS the estimate that it returns. 
                double estimate = Algorithms.CountSketch(stream, epsilon, index, t);
                estimatesUnsorted.Add(estimate);
                //Console.WriteLine(estimate);
                // we compute the mean-square error
                MSE += Math.Pow((estimate - S), 2);
                // we also compute the mean
                mean += estimate;
            }
            
            // we compute the mean-square error
            MSE = MSE/100;
            // so that MSE is the correct variance? page. 6 implementeringsopgave
            Console.WriteLine("mean-squared error:" + MSE);
            mean /= 100;
            Console.WriteLine("mean:" + mean);
            
            List<double> M = new List<double>(estimatesUnsorted.Where((x, i) => (i + 6) % 11 == 0));
            M.Sort();
            
            return (estimatesUnsorted, MSE, mean, M);
        }

        static void Exercise6(Int32 n, Int32 l)
        {
            IEnumerable<Tuple<ulong, int>> stream = Generator.CreateStream(n, l);
            var epsilon = 0.001;
            int t = (int) Math.Log2(8/(Math.Pow(epsilon, 2)));
            var secondMoment = Algorithms.CountSketch(stream, 1, 0, t);
            Console.WriteLine(secondMoment);
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
        // Test the runtime of Algorithm1 which is actually the 4-universal hash function.
        // In the report, we compare the runtime against mod-prime and multiply-shift from exercise 1.
        static void Exercise4(IEnumerable<Tuple<ulong, int>> stream) {
            // index is just the random seed - we set it to 1
            
            UInt64 sum = 0;
            var watch = Stopwatch.StartNew();
            
            foreach(Tuple<ulong, int> item in stream){
                sum += Algorithms.Algorithm1(item.Item1, 0);
            }

            watch.Stop();

            Console.WriteLine("4-universal hash function sum:" + sum);
            Console.WriteLine("4-universal hash function elapsed:" + watch.ElapsedMilliseconds);

        }

        static void Exercise1(IEnumerable<Tuple<ulong, int>> stream, Int32 l) { 

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

        static void WriteToCSV<T>(List<T> list, string filename) 
        {
            List<string> linesList = new List<string>();

            for (int i = 0; i < list.Count; i++)
            {
                var _string = "{0},{1}";
                var _formatted = string.Format(_string, (i+1).ToString(), list[i].ToString());
                linesList.Add(_formatted);
            }

            string[] linesArray = linesList.ToArray();

            string path = filename;
            System.IO.File.WriteAllLines(@path, linesArray);
        }
    }
}

