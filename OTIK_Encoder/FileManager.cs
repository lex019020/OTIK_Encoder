using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OTIK_Encoder
{
    class FileManager
    {
        private readonly string _dirPath = "";
        private readonly List<string> _fileNames;
        private readonly bool _isDirectory = false;
        private int _currentNum = 0;

        public static bool PathIsCorrect (string path)
        {
            return File.Exists(path) || Directory.Exists(path);
        }

        public FileManager(string path)
        {
            _fileNames = new();
            if (Directory.Exists(path))
            {
                _dirPath = path;
                if (_dirPath.Last() != '\\')
                    _dirPath += '\\';

                _fileNames.AddRange(Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories));
                _isDirectory = true;
            }
            else if (File.Exists(path))
                _fileNames.Add(path);
        }

        /// <summary>
        /// Returns true, while return data is valid
        /// </summary>
        /// <param name="bytes">bytes, that file contains</param>
        /// <param name="name">relative file path or filename</param>
        /// <returns></returns>
        public bool GetNextFile(out List<byte> bytes, out string name)
        {
            if(_fileNames.Count == _currentNum)
            {
                bytes = new();
                name = "";
                return false;
            }

            if(!_isDirectory)
            {
                name = _fileNames[0].Substring(_fileNames[0].LastIndexOf('\\') + 1);
                bytes = new(File.ReadAllBytes(_fileNames[0]));
                return false;
            }

            bytes = new(File.ReadAllBytes(_fileNames[_currentNum]));
            name = _fileNames[_currentNum].TrimStart(_dirPath.ToCharArray());
            _currentNum++;
            return true;
        }

    }
}
