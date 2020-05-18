using System.Collections.Generic;
using System;

namespace rad
{
    // hashtabellen vil være en list of lists
    // billedmængden er 2^l dvs. at den ydre liste i hash tabellen skal have størrelse 2^l 
    class HashTable {
        public List<List<MutableKeyValuePair<int, int>>> hashT {get;set;}
        public HashTable() {

            int l = 20;    

            var uni = Math.Pow(2, l);
            // here we create an empty list of lists
            hashT = new List<List<MutableKeyValuePair<int, int>>>();

            for (int i = 0; i<uni; i++){
                hashT.Add(new List<MutableKeyValuePair<int,int>>());
            }
        }

        // get(x): Skal returnere den værdi, der tilhører nøglen x. Hvis x ikke er i tabellen skal der returneres 0.
        public int get(UInt64 x) {
        //    Dictionary<int,int>. dic = new Dictionary<int,int>().ContainsKey()
            // var hashVal = HashFunctions.multiplyShift(x);
            // if (hashT[(int)hashVal].Exists(t => t.ContainsKey((int)x))){
                
            //     return hashT[(int)hashVal].Find(x => x.);
            // } else {
            //     return 0;
            // } 

            var hashVal = HashFunctions.multiplyShift(x);

            if (hashT[(int)hashVal].Exists(keyValPair => keyValPair.Key.Equals((int)x))) {
                return hashT[(int)hashVal].Find(keyValPair => keyValPair.Key.Equals((int)x)).Value;
            } return 0;
        }

        // set(x, v): Skal sætte nøglen x til at have værdien v. Hvis x ikke allerede er i tabellen
        // så tilføjes den til tabellen med værdien v.
        public void set(UInt64 x, int v)
        {
            var hashVal = HashFunctions.multiplyShift(x);
            if (hashT[(int) hashVal].Exists(keyValPair => keyValPair.Key.Equals((int)x)))
            {
                var index = hashT[(int)hashVal].FindIndex(keyValPair => keyValPair.Key.Equals((int)x));
                hashT[(int)hashVal][index].Value = v;
                Console.WriteLine("Key found. Overwriting value");
            } else {
                Console.WriteLine("Key wasn't found. Hashing k,v to hash table.");
                hashT[(int)hashVal].Add(new MutableKeyValuePair<int, int>((int)x, v));
            }
        }

        // increment(x, d): Skal lægge d til værdien tilhørende x. Hvis x ikke er i tabellen, skal
        // x tilføjes til tabellen med værdien d
        public void increment(UInt64 x, int d){
            
            var hashVal = HashFunctions.multiplyShift(x);
            if (hashT[(int)hashVal].Exists(keyValPair => keyValPair.Key.Equals((int)x))){
                Console.WriteLine("found");
                var index = hashT[(int)hashVal].FindIndex(keyValPair => keyValPair.Key.Equals((int)x));
                hashT[(int)hashVal][index].Value = hashT[(int)hashVal][index].Value + d;
            } else {
                Console.WriteLine("not found");
                hashT[(int)hashVal].Add(new MutableKeyValuePair<int, int>((int)x, d));
            }
        }
    }
}