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
            if (hashT[(int)hashVal].Exists(x => x.Key.Equals(x))) {
                return hashT[(int)hashVal].Find(t => t.Key.Equals(x)).Value;
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
        public void increment(UInt64 x, int d){
            
            var hashVal = HashFunctions.multiplyShift(x);
            if (hashT[(int)hashVal].Exists(x => x.Key.Equals(x))){
                var index = hashT[(int)hashVal].FindIndex(0,2,x => x.Key.Equals(x));
                hashT[(int)hashVal][index].Value += d;
            } else {
                hashT[(int)hashVal].Add(new MutableKeyValuePair<int, int>((int)x, d));
            }

        }

    }
}