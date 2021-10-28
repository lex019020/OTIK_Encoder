using System;
using System.Collections.Generic;
using Exception = System.Exception;

namespace OTIK_Encoder
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

    public enum Version
    {
        V1,
    }

    class ArchiveHeader // version 1
    {
        private byte _version, _randSplit, _entrCompr, _cbCompr, _antiinterf;
        private ulong _fileCount;
        private readonly bool _isReadFromFile;
        private readonly HashSet<HeaderError> _errors = new();

        /// <summary>
        /// Use this constructor when creating new header.
        /// Default header is:
        /// Version: 1; all compression types are disabled; 0 files.
        /// </summary>
        public ArchiveHeader()
        {
            _isReadFromFile = false;
            _version = 0;
            _entrCompr = 0;
            _randSplit = 0;
            _cbCompr = 0;
            _antiinterf = 0;
            _fileCount = 0;
        }

        /// <summary>
        /// Use this constructor when yao want to read header from file
        /// </summary>
        /// <param name="headerBytes">First 12 bytes of file</param>
        public ArchiveHeader(IReadOnlyList<byte> headerBytes)
        {
            _isReadFromFile = true;
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

            _fileCount = 0;
            _fileCount += (ulong) ((ulong)headerBytes[9] << 16);
            _fileCount += (ulong) ((ulong)headerBytes[10] << 8);
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

        public ulong GetFileCount() => HasErrors() ? 0 : _fileCount;

        public Version GetVersion() =>  HasErrors() ? 0 : (Version)_version;

        public RandSplitType GetRandSplitType() => HasErrors() ? 0 : (RandSplitType) _randSplit;

        public EntropicBasedCompressionType GEntropicBasedCompressionType()
            => HasErrors() ? 0 : (EntropicBasedCompressionType) _entrCompr;

        public ContextBasedCompressionType GetContextBasedCompressionType()
            => HasErrors() ? 0 : (ContextBasedCompressionType) _cbCompr;

        public AntiInterferenceType GetAntiInterferenceType()
            => HasErrors() ? 0 : (AntiInterferenceType) _antiinterf;

        public IReadOnlyList<byte> GetHeaderBytes()
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            List<byte> bytes = new();

            // signature
            bytes.Add(0x4b);
            bytes.Add(0x45);
            bytes.Add(0x4b);
            bytes.Add(0x57);

            // version
            bytes.Add(0x00);

            // rand splitting
            bytes.Add(_randSplit);

            // entropic compression
            bytes.Add(_entrCompr);

            // context-based compression
            bytes.Add(_cbCompr);

            // anti-interference
            bytes.Add(_antiinterf);

            // files count is 3 bytes
            var c1 = (byte) _fileCount;
            var c2 = (byte) (_fileCount >> 8);
            var c3 = (byte) (_fileCount >> 16);

            bytes.Add(c3);
            bytes.Add(c2);
            bytes.Add(c1);

            return bytes;
        }

        /// <summary>
        /// Set header version. If header is read from file, method does nothing.
        /// </summary>
        /// <param name="version"></param>
        public void SetVersion(Version version)
        {
            if (_isReadFromFile) return;
            _version = (byte) version;
        }

        /// <summary>
        /// Set random splitting type. If header is read from file, method does nothing.
        /// </summary>
        /// <param name="type"></param>
        public void SetRandSplitType(RandSplitType type)
        {
            if (_isReadFromFile) return;
            _randSplit = (byte) type;
        }

        /// <summary>
        /// Set entropic-based compression type. If header is read from file, method does nothing.
        /// </summary>
        /// <param name="type"></param>
        public void SetEntropicBasedCompressionType(EntropicBasedCompressionType type)
        {
            if (_isReadFromFile) return;
            _entrCompr = (byte) type;
        }

        /// <summary>
        /// Set context-based compression type. If header is read from file, method does nothing.
        /// </summary>
        /// <param name="type"></param>
        public void SetContextBasedCompressionType(ContextBasedCompressionType type)
        {
            if (_isReadFromFile) return;
            _cbCompr = (byte) type;
        }

        /// <summary>
        /// Set anti-interference algorithm type. If header is read from file, method does nothing.
        /// </summary>
        /// <param name="type"></param>
        public void SetAntiInterferenceType(AntiInterferenceType type)
        {
            if (_isReadFromFile) return;
            _antiinterf = (byte) type;
        }

        /// <summary>
        /// Set files count. Limit: 2^24. If header is read from file, method does nothing.
        /// </summary>
        /// <param name="fcount">Limit: 2^24</param>
        public void SetFilesCount(ulong fcount)
        {
            if(_isReadFromFile) return;
            if (fcount >= Math.Pow(2, 24)) throw new Exception("WTF are you trying to compress????");
            _fileCount = fcount;
        }
    }
}
