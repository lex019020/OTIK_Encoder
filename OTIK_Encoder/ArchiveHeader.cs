using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTIK_Encoder
{
    class ArchiveHeader // version 1
    {
        public enum HeaderError
        {
            IncorrectLength,
            IncorrectSignature,
            NewerVersion,
            IncorrectRandSplittingType,
            IncorrectEntropicComprType,
            IncorrectContextBasedComprType,
            IncorrectAntiInterfType,
            UnsupportedFeatures,
        };

        public enum RandSplitType
        {
            NoSplit,
            RandomSplit,
        };

        public enum EntropicBasedCompressionType
        {
            None,
        };

        public enum ContextBasedCompressionType
        {
            None,
        };

        public enum AntiInterferenceType
        {
            None,
        };

        private readonly byte _version, _randSplit, _entrCompr, _cbCompr, _antiinterf;
        private readonly ulong _fileCount;
        private readonly HashSet<HeaderError> _errors = new();
        public ArchiveHeader(IReadOnlyList<byte> headerBytes)
        {
            if (headerBytes.Count != 12)
            {
                _errors.Add(HeaderError.IncorrectLength);
                return;
            }

            if (headerBytes[0] != 0x4b || headerBytes[1] != 0x45 || headerBytes[2] != 0x4b || headerBytes[3] != 0x57)
            {
                _errors.Add(HeaderError.IncorrectSignature);
            }

            _version = headerBytes[4];

            if (_version > 0)
            {
                _errors.Add(HeaderError.NewerVersion);
            }

            _randSplit = headerBytes[5];
            _entrCompr = headerBytes[6];
            _cbCompr = headerBytes[7];
            _antiinterf = headerBytes[8];

            _fileCount += (ulong) (headerBytes[9] << 16);
            _fileCount += (ulong) (headerBytes[10] << 8);
            _fileCount += (ulong)  headerBytes[11];

            if (_randSplit > 1)
            {
                _errors.Add(HeaderError.UnsupportedFeatures);
                _errors.Add(HeaderError.IncorrectRandSplittingType);
            }

            if (_entrCompr > 0)
            {
                _errors.Add(HeaderError.UnsupportedFeatures);
                _errors.Add(HeaderError.IncorrectEntropicComprType);
            }

            if (_cbCompr > 0)
            {
                _errors.Add(HeaderError.UnsupportedFeatures);
                _errors.Add(HeaderError.IncorrectContextBasedComprType);
            }

            if (_antiinterf > 0)
            {
                _errors.Add(HeaderError.UnsupportedFeatures);
                _errors.Add(HeaderError.IncorrectAntiInterfType);
            }
        }

        public bool HasErrors() => _errors.Count > 0;

        public HashSet<HeaderError> GetErrors() => _errors;

        public ulong GetFileCount() => _fileCount;

        public byte GetVersion() => _version;

        public RandSplitType GetRandSplitType() => (RandSplitType) _randSplit;

        public EntropicBasedCompressionType GEntropicBasedCompressionType()
            => (EntropicBasedCompressionType) _entrCompr;

        public ContextBasedCompressionType GetContextBasedCompressionType()
            => (ContextBasedCompressionType) _cbCompr;

        public AntiInterferenceType GetAntiInterferenceType()
            => (AntiInterferenceType) _antiinterf;
    }
}
