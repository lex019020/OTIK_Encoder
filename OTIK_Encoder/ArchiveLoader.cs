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
                FileInfo fileInfo = new FileInfo(path);
                FileStream stream = fileInfo.OpenRead();
                byte[] header = new byte[12];
                stream.Read(header, 0, 12);
                ArchiveHeader headerChecker = new(new List<byte>(header));
                return !headerChecker.HasErrors();
            }

            return false;
        }

        private string _path;
        private FileStream _stream;

        public ArchiveLoader(string path)
        {
            _path = path;
        }
        //TODO:
        // List <string>: размер - имя
        // вернуть false, если ошибки (в лист строки ошибок)
        // читаем архив, вытаскиваем первые 12 байт, передаем в хэдер и смотрим ошибки.
        // если ошибок нет, читаем имена и размеры файлов

        //TODO метод проверки хэдера на ошибки (читай выше, там будет вызываться)

    }
}
