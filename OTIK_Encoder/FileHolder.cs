using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTIK_Encoder
{
    class FileHolder
    {
        private string _name;
        private List<byte> _data;

        public FileHolder(string name, in List<byte> data)
        {
            _name = name;
            _data = data;
        }

        public string GetName()
        {
            return _name;
        }

        public List<byte> GetData()
        {
            return _data;
        }

        public uint getDataLength()
        {
            return (uint)_data.Count;
        }

        public ushort getNameLength()
        {
            return (ushort)_name.Length;
        }

    }
}
