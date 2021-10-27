using System;
using System.Collections.Generic;

namespace OTIK_Encoder
{
    class RandomSplitter
    {
        public static List<List<byte>> Split(in List<byte> data)
        {
            var rand = new Random();
            List<List<byte>> result = new();
            var numberOfSplitted = 0;
            var finish = false;
            while(!finish)
            {
                int randSize = rand.Next(1, 17);
                if (numberOfSplitted + randSize >= data.Count)
                {
                    randSize = data.Count - numberOfSplitted;
                    finish = true;
                }
                result.Add(new List<byte>(data.GetRange(numberOfSplitted, randSize)));
            }
            return result;
        }

        public static List<byte> Join(in List<List<byte>> data)
        {
            List<byte> result = new();
            foreach(var i in data)
                result.AddRange(i);
            
            return result;
        }


    }
}
