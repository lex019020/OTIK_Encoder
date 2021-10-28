using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OTIK_Encoder
{
    class ArchiveLoader
    {
        //TODO 12 to constant
        public static bool IsCorrectArchivePath(string path)
        {
            if (File.Exists(path))
            {
                FileStream stream = File.OpenRead(path);

                byte[] header = new byte[12];
                stream.Read(header, 0, 12);
                ArchiveHeader headerChecker = new(new List<byte>(header));
                bool result = !headerChecker.HasErrors();
                stream.Close();
                return result;
            }

            return false;
        }

        private FileStream _stream;
        private int currentReadPos;
        private uint filesToRead;
        private uint fileCounter;

        public ArchiveLoader(string path)
        {
            _stream = File.OpenRead(path);
            currentReadPos = 12;
            fileCounter = 0;
            filesToRead = GetArchiveHeader().GetFileCount();
        }

        public void CloseStream()
        {
            _stream.Dispose();
        }

        public bool ReadNextFile(out string name, out List<byte> bytes)
        {
            if(fileCounter == filesToRead)
            {
                name = "";
                bytes = new();
                return false;
            }

            byte[] nameNumBytes = new byte[2];
            currentReadPos += _stream.Read(nameNumBytes, currentReadPos, 2);
            var numName = BitConverter.ToUInt16(nameNumBytes);

            byte[] readName = new byte[numName];
            currentReadPos += _stream.Read(readName, currentReadPos, numName);
            name = BitConverter.ToString(readName);

            byte[] dataNumBytes = new byte[4];
            currentReadPos += _stream.Read(dataNumBytes, currentReadPos, 4);
            var numData = BitConverter.ToInt32(nameNumBytes);

            byte[] readData = new byte[numData];
            currentReadPos += _stream.Read(readData, currentReadPos, numData);
            bytes = new(readData);

            return true;
        }

        public List<string> GetArchiveContent()
        {
            int curPos = 12;
            List<string> result = new();
            for (int i = 0; i < filesToRead; i++)
            {
                byte[] nameNumBytes = new byte[2];
                curPos += _stream.Read(nameNumBytes, curPos, 2);
                var numName = BitConverter.ToUInt16(nameNumBytes);

                byte[] readName = new byte[numName];
                curPos += _stream.Read(readName, curPos, numName);
                string name = BitConverter.ToString(readName);

                byte[] dataNumBytes = new byte[4];
                curPos += _stream.Read(dataNumBytes, curPos, 4);
                var numData = BitConverter.ToInt32(nameNumBytes);
                curPos += numData;

                result.Add(name + " - " + numName.ToString() + "bytes");
            }

            return result;
        }

        public ArchiveHeader GetArchiveHeader()
        {
            byte[] header = new byte[12];
            _stream.Read(header, 0, 12);
            return new ArchiveHeader(new List<byte>(header));
        }

    }
}
