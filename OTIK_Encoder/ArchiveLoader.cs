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
        public static bool IsCorrectArchivePath(string path)
        {
            if(File.Exists(path))
            {
                FileStream stream = File.OpenRead(path);
                bool result = HeaderErrors(stream).Count == 0;
                stream.Close();
                return result;
            }

            return false;
        }

        private static List<string> HeaderErrors(FileStream stream)
        {
            List<string> result = new();
            byte[] header = new byte[12];
            stream.Read(header, 0, 12);
            ArchiveHeader headerChecker = new(new List<byte>(header));
            if(headerChecker.HasErrors())
            {
                HashSet<HeaderError> errors = headerChecker.GetErrors();
                foreach(HeaderError error in errors)
                    result.Add(error.ToString());
            }
            return result;
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

            byte[] header = new byte[12];
            _stream.Read(header, 0, 12);
            ArchiveHeader headerChecker = new(new List<byte>(header));
            filesToRead = headerChecker.GetFileCount();
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

        //TODO:
        // прочитать имена и размеры

        public List<string> HaveHeaderErrors()
        {
            return HeaderErrors(_stream);
        }

    }
}
