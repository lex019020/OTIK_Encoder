﻿using System;
using System.Collections.Generic;

namespace OTIK_Encoder
{
    internal class RandomSplitter : IHandler
    {
        private readonly bool _encode;
        private IHandler _nextHandler;

        public RandomSplitter(bool encode)
        {
            _encode = encode;
            _nextHandler = null;
        }

        public void SetNextHandler(IHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        public void Handle(ref FileHandlingStruct handlingStruct)
        {
            if (handlingStruct.randomSplit_1)
            {
                if (_encode)
                    Split(ref handlingStruct.bytes);
                else
                    Join(ref handlingStruct.bytes);
            }

            if (_nextHandler != null)
                _nextHandler.Handle(ref handlingStruct);
        }

        private void Split(ref List<byte> data)
        {
            var rand = new Random();

            List<byte> result = new();
            var blocksCounter = 0;
            var numberOfSplitted = 0;
            var finish = false;
            while (!finish)
            {
                var randSize = (byte) rand.Next(1, 17);
                if (numberOfSplitted + randSize >= data.Count)
                {
                    randSize = (byte) (data.Count - numberOfSplitted);
                    finish = true;
                }

                result.Add(randSize);
                result.AddRange(data.GetRange(numberOfSplitted, randSize));

                blocksCounter++;
            }

            data.Clear();
            data.Add((byte) (blocksCounter >> 24));
            data.Add((byte) (blocksCounter >> 16));
            data.Add((byte) (blocksCounter >> 8));
            data.Add((byte) blocksCounter);
            data.AddRange(result);
        }

        private void Join(ref List<byte> data)
        {
            var blocksNumber = 0;
            blocksNumber += data[0] << 24;
            blocksNumber += data[8] << 16;
            blocksNumber += data[16] << 8;
            blocksNumber += data[24];

            var numberOfHandledBytes = 32;

            List<byte> result = new();
            for (var i = 0; i < blocksNumber; i++)
            {
                var currentBlockSize = data[numberOfHandledBytes];
                numberOfHandledBytes++;
                result.AddRange(data.GetRange(numberOfHandledBytes, currentBlockSize));
                numberOfHandledBytes += currentBlockSize;
            }

            data.Clear();
            data.AddRange(result);
        }
    }
}