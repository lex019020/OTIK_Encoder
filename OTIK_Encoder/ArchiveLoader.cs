using System;
using System.Collections.Generic;
using System.IO;

namespace OTIK_Encoder
{
    internal class ArchiveLoader
    {
        private static readonly int headerSize = 12;

        private readonly FileStream _stream;
        private int currentReadPos;
        private readonly uint fileCounter;
        private readonly uint filesToRead;

        public ArchiveLoader(string path)
        {
            _stream = File.OpenRead(path);
            currentReadPos = headerSize;
            fileCounter = 0;
            filesToRead = GetArchiveHeader().GetFileCount();
        }

        public static bool IsCorrectArchivePath(string path)
        {
            if (File.Exists(path))
            {
                var stream = File.OpenRead(path);

                var header = new byte[headerSize];
                stream.Read(header, 0, headerSize);
                ArchiveHeader headerChecker = new(new List<byte>(header));
                var result = !headerChecker.HasErrors();
                stream.Close();
                return result;
            }

            return false;
        }

        public void CloseStream()
        {
            _stream.Dispose();
        }

        public bool ReadNextFile(out string name, out List<byte> bytes)
        {
            if (fileCounter == filesToRead)
            {
                name = "";
                bytes = new List<byte>();
                return false;
            }

            var nameNumBytes = new byte[2];
            currentReadPos += _stream.Read(nameNumBytes, currentReadPos, 2);
            var numName = BitConverter.ToUInt16(nameNumBytes);

            var readName = new byte[numName];
            currentReadPos += _stream.Read(readName, currentReadPos, numName);
            name = BitConverter.ToString(readName);

            var dataNumBytes = new byte[4];
            currentReadPos += _stream.Read(dataNumBytes, currentReadPos, 4);
            var numData = BitConverter.ToInt32(nameNumBytes);

            var readData = new byte[numData];
            currentReadPos += _stream.Read(readData, currentReadPos, numData);
            bytes = new List<byte>(readData);

            return true;
        }

        public List<string> GetArchiveContent()
        {
            var curPos = headerSize;
            List<string> result = new();
            for (var i = 0; i < filesToRead; i++)
            {
                var nameNumBytes = new byte[2];
                curPos += _stream.Read(nameNumBytes, curPos, 2);
                var numName = BitConverter.ToUInt16(nameNumBytes);

                var readName = new byte[numName];
                curPos += _stream.Read(readName, curPos, numName);
                var name = BitConverter.ToString(readName);

                var dataNumBytes = new byte[4];
                curPos += _stream.Read(dataNumBytes, curPos, 4);
                var numData = BitConverter.ToInt32(nameNumBytes);
                curPos += numData;

                result.Add(name + " - " + numName + "bytes");
            }

            return result;
        }

        public ArchiveHeader GetArchiveHeader()
        {
            var header = new byte[headerSize];
            _stream.Read(header, 0, headerSize);
            return new ArchiveHeader(new List<byte>(header));
        }
    }
}