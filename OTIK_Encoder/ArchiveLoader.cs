using System;
using System.Collections.Generic;
using System.IO;

namespace OTIK_Encoder
{
    internal class ArchiveLoader
    {
        private static readonly int headerSize = 12;

        private readonly FileStream _stream;
        private long currentReadPos;
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
            _stream.Position = currentReadPos;
            if (fileCounter == filesToRead)
            {
                name = "";
                bytes = new List<byte>();
                return false;
            }

            var nameNumBytes = new byte[2];
            _stream.Read(nameNumBytes, 0, 2);
            var numName = BitConverter.ToUInt16(nameNumBytes);

            var readName = new byte[numName];
            _stream.Read(readName, 0, numName);
            name = BitConverter.ToString(readName);

            var dataNumBytes = new byte[4];
            _stream.Read(dataNumBytes, 0, 4);
            var numData = BitConverter.ToInt32(nameNumBytes);

            var readData = new byte[numData];
            _stream.Read(readData, 0, numData);
            bytes = new List<byte>(readData);

            currentReadPos = _stream.Position;

            return true;
        }

        public List<string> GetArchiveContent()
        {
            _stream.Position = 0;
            List<string> result = new();
            for (var i = 0; i < filesToRead; i++)
            {
                var nameNumBytes = new byte[2];
                _stream.Read(nameNumBytes, 0, 2);
                var numName = BitConverter.ToUInt16(nameNumBytes);

                var readName = new byte[numName];
                _stream.Read(readName, 0, numName);
                var name = BitConverter.ToString(readName);

                var dataNumBytes = new byte[4];
                _stream.Read(dataNumBytes, 0, 4);
                var numData = BitConverter.ToInt32(nameNumBytes);
                _stream.Position += numData;

                result.Add(name + " - " + numName + "bytes");
            }

            return result;
        }

        public ArchiveHeader GetArchiveHeader()
        {
            var header = new byte[headerSize];
            _stream.Position = 0;
            _stream.Read(header, 0, headerSize);
            return new ArchiveHeader(new List<byte>(header));
        }
    }
}