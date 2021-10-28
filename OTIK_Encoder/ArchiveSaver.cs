﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OTIK_Encoder
{
    class ArchiveSaver
    {

        public static bool IsCorrectArchivePath(string path)
        {
            return false;
        }
        
        private readonly FileInfo fileInfo;
        private readonly FileStream stream;

        public ArchiveSaver(string filename)
        {
            if(!IsCorrectArchivePath(filename)) return;

            fileInfo = new FileInfo(filename);

            if (!fileInfo.Exists)
                fileInfo.Create();

            stream = fileInfo.OpenWrite();
        }

        public void AppendBytes(IReadOnlyList<byte> bytes)
        {
            stream.Write(bytes.ToArray());
        }

        public void AppendFile(string path, IReadOnlyList<byte> data)
        {
            var pathBytes = Encoding.Unicode.GetBytes(path);
            var pathlen = (short) pathBytes.Length;
            var pathlenBytes = BitConverter.GetBytes(pathlen);
            var datalenBytes = BitConverter.GetBytes(data.Count);
            AppendBytes(pathlenBytes);
            AppendBytes(pathBytes);
            AppendBytes(datalenBytes);
            AppendBytes(data);
        }

        public void CloseConnection()
        {
            stream.Close();
        }

        public void DeleteAndCloseConnection()
        {
            stream.Close();
            fileInfo.Delete();
        }

    }
}
