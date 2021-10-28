using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OTIK_Encoder
{
    internal class ArchiveSaver
    {
        private readonly FileInfo fileInfo;
        private readonly FileStream stream;

        public ArchiveSaver(string filename)
        {
            if (!IsCorrectArchivePath(filename)) return;

            fileInfo = new FileInfo(filename);

            if (!fileInfo.Exists)
                fileInfo.Create();

            stream = fileInfo.OpenWrite();
        }

        public static bool IsCorrectArchivePath(string path)
        {
            return !Directory.Exists(path) && path.EndsWith(".otik");
        }

        public void AppendBytes(IReadOnlyList<byte> bytes)
        {
            stream.Write(bytes.ToArray());
        }

        public void AppendFile(string path, IReadOnlyList<byte> data)
        {
            var pathBytes = Encoding.Unicode.GetBytes(path);
            var pathlen = (ushort) pathBytes.Length;
            var pathlenBytes = BitConverter.GetBytes(pathlen);
            var datalenBytes = BitConverter.GetBytes(data.Count);
            AppendBytes(pathlenBytes);
            AppendBytes(pathBytes);
            AppendBytes(datalenBytes);
            AppendBytes(data);
        }

        public void CloseConnection()
        {
            stream.Dispose();
        }

        public void DeleteAndCloseConnection()
        {
            stream.Dispose();
            fileInfo.Delete();
        }
    }
}