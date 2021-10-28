using System;
using System.Collections.Generic;

namespace OTIK_Encoder
{
    class RandomSplitter : IHandler
    {
        private IHandler _nextHandler;
        private bool _encode;

        public RandomSplitter(bool encode)
        {
            _encode = encode;
            _nextHandler = null;
        }

        public void Split(ref List<byte> data)
        {
            var rand = new Random();

            List<byte> result = new();
            int blocksCounter = 0;
            int numberOfSplitted = 0;
            var finish = false;
            while(!finish)
            {
                byte randSize = (byte)rand.Next(1, 17);
                if (numberOfSplitted + randSize >= data.Count)
                {
                    randSize = (byte)(data.Count - numberOfSplitted);
                    finish = true;
                }
                result.Add(randSize);
                result.AddRange(data.GetRange(numberOfSplitted, randSize));

                blocksCounter++;
            }

            data.Clear();
            data.Add((byte)(blocksCounter >> 24));
            data.Add((byte)(blocksCounter >> 16));
            data.Add((byte)(blocksCounter >> 8));
            data.Add((byte)blocksCounter);
            data.AddRange(result);
        }

        public List<byte> Join(ref List<byte> data)
        {
            int blocksNumber = 0;
            blocksNumber += ((int)data[0]) << 24;
            blocksNumber += ((int)data[8]) << 16;
            blocksNumber += ((int)data[16]) << 8;
            blocksNumber += (int)data[24];

            //4b - blockscount
            //1b - blocklen
            // block
            List<byte> result = new();
            



            return result;
        }

        public void SetNextHandler(IHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        public void Handle(FileHandlingStruct handlingStruct)
        {
            if (handlingStruct.randomSplit_1)
            {
                if (_encode)
                    Split(handlingStruct.bytes);
                else
                    Join(handlingStruct.bytes);

            }

            if (_nextHandler != null)
                _nextHandler.Handle(handlingStruct);
        }
    }
}
