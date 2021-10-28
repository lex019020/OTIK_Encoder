using System;
using System.Collections.Generic;
using System.IO;

namespace OTIK_Encoder
{
    class FileSaver
    {
        public static bool IsCorrectSavePath(string path)
        {
            return false;
        }

        private readonly string _path;

        public FileSaver(string path) 
        {
            _path = path;
            if(!_path.EndsWith('\\'))
                _path += '\\';
        }

        public void AddFile(string name, List<byte> bytes) 
        {
            //if name is just name of the file
            if(!name.Contains('\\'))
            {
                File.WriteAllBytes(_path + name, bytes.ToArray());
                return;
            }

            var separatorPosition = name.LastIndexOf('\\');
            var tempPath = _path + name.Substring(0, separatorPosition);
            Directory.CreateDirectory(tempPath);

            File.WriteAllBytes(tempPath + name[separatorPosition..], bytes.ToArray());
        }
    }
}
