using System.Collections.Generic;

namespace OTIK_Encoder
{
    internal class FileHolder
    {
        private readonly List<byte> _data;
        private readonly string _name;

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
            return (uint) _data.Count;
        }

        public ushort getNameLength()
        {
            return (ushort) _name.Length;
        }
    }
}