using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTIK_Encoder
{
    class ArchiveLoader
    {
        public static bool IsCorrectArchivePath(string path)
        {
            return false;
        }
        //TODO:
        // List <string>: размер - имя
        // вернуть false, если ошибки (в лист строки ошибок)
        // читаем архив, вытаскиваем первые 12 байт, передаем в хэдер и смотрим ошибки.
        // если ошибок нет, читаем имена и размеры файлов

        //TODO метод проверки хэдера на ошибки (читай выше, там будет вызываться)

    }
}
