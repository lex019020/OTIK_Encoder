using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OTIK_Encoder
{
    internal class FileLoader
    {
        private readonly string _dirPath = "";
        private readonly List<string> _fileNames;
        private readonly bool _isDirectory;
        private int _currentNum;

        public FileLoader(string path)
        {
            _fileNames = new List<string>();
            if (Directory.Exists(path))
            {
                _dirPath = path;
                if (_dirPath.Last() != '\\')
                    _dirPath += '\\';

                _fileNames.AddRange(Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories));
                _isDirectory = true;
            }
            else if (File.Exists(path))
            {
                _fileNames.Add(path);
            }
        }

        public static bool IsCorrectLoadPath(string path)
        {
            return File.Exists(path) || Directory.Exists(path);
        }

        /// <summary>
        ///     Returns true, while return data is valid
        /// </summary>
        /// <param name="bytes">bytes, that file contains</param>
        /// <param name="name">relative file path or filename</param>
        /// <returns></returns>
        public bool GetNextFile(out List<byte> bytes, out string name)
        {
            if (_fileNames.Count == _currentNum)
            {
                bytes = new List<byte>();
                name = "";
                return false;
            }

            if (!_isDirectory)
            {
                name = _fileNames[0].Substring(_fileNames[0].LastIndexOf('\\') + 1);
                bytes = new List<byte>(File.ReadAllBytes(_fileNames[0]));
                _currentNum++;
                return true;
            }

            bytes = new List<byte>(File.ReadAllBytes(_fileNames[_currentNum]));
            name = _fileNames[_currentNum].TrimStart(_dirPath.ToCharArray());
            _currentNum++;
            return true;
        }

        public int GetNumberOfFiles()
        {
            return _fileNames.Count;
        }
    }
}