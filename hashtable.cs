using System.Collections.Generic;
using System;

namespace rad
{
    // hashtabellen vil være en list of lists
    // billedmængden er 2^l dvs. at den ydre liste i hash tabellen skal have størrelse 2^l 
    class HashTable {
        public List<List<Tuple<int, int>>> hashT {get;set;}
        public HashTable() {

            int l = 20;    

            var uni = Math.Pow(2, l);
            // here we create an empty list of lists
            hashT = new List<List<Tuple<int, int>>>();

            for (int i = 0; i<uni; i++){
                hashT.Add(new List<Tuple<int,int>>());
            }

        }

        // get(x): Skal returnere den værdi, der tilhører nøglen x. Hvis x ikke er i tabellen skal der returneres 0.
        public int get(UInt64 x) {
           
            var hashVal = HashFunctions.multiplyShift(x);
            if (hashT[(int)hashVal].Exists(t => t.Item1.Equals(x))){
                return hashT[(int)hashVal].Find(x => x.Item1.Equals(x)).Item2;
            } else {
                return 0;
            } 

           
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