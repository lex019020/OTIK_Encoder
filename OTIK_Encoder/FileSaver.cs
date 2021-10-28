using System.Collections.Generic;
using System.IO;

namespace OTIK_Encoder
{
    internal class FileSaver
    {
        private readonly string _path;

        public FileSaver(string path)
        {
            _path = path;
            if (!_path.EndsWith('\\'))
                _path += '\\';
        }

        public static bool IsCorrectSavePath(string path)
        {
            return false;
        }

        public void AddFile(string name, List<byte> bytes)
        {
            //if name is just name of the file
            if (!name.Contains('\\'))
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