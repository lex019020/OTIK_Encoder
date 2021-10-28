using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTIK_Encoder
{
    class FileSaver
    {
        public static bool IsCorrectSavePath()
        {
            return false;
        }

        private readonly string _path;

        public FileSaver(string path) 
        {
            _path = path;
        }

        public void AddFile(string name, List<byte> bytes) 
        {
            
        }
    }
}
