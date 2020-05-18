using System.Collections.Generic;
using System;

namespace rad
{
    // hashtabellen vil være en list of lists
    // billedmængden er 2^l dvs. at den ydre liste i hash tabellen skal have størrelse 2^l 
    class HashTable {
        public List<List<Tuple<int, int>>> hashT {get;set;}
        public HashTable(int l) {
            var uni = Math.Pow(2, l);
            // here we create an empty list of lists
            hashT = new List<List<Tuple<int, int>>>();

            for (int i = 0; i<uni; i++){
                hashT.Add(new List<Tuple<int,int>>());
            }

        }

        // get(x): Skal returnere den værdi, der tilhører nøglen x. Hvis x ikke er i tabellen skal der returneres 0.
        public void get(int x) {
            // Console.WriteLine(hashT.Capacity);
            // List<Tuple<int,int>> trt = new List<Tuple<int,int>>();
            // trt.Add(new Tuple<int, int>(5 , 8));
            // hashT.Add(trt);
            // hashT.Add(trt);
            // hashT.Add(trt);
            
            // Console.WriteLine((hashT[0][0].Item1));
            // Console.WriteLine((hashT[1][0].Item1));
            // Console.WriteLine((hashT[2][0].Item1));

            // 1. we compute h(x)
            // 2. we check to see if this value exists in the outer list
            // var hashVal = HashFunctions.multiplyShift(x);
            // long ii=0;

            // hashT[ii]

        }

        // set(x, v): Skal sætte nøglen x til at have værdien v. Hvis x ikke allerede er i tabellen
        // så tilføjes den til tabellen med værdien v.
        public void set(int x, int v){

        }

        // increment(x, d): Skal lægge d til værdien tilhørende x. Hvis x ikke er i tabellen, skal
        // x tilføjes til tabellen med værdien d
        public void increment(int x, int d){

        }

    }
}