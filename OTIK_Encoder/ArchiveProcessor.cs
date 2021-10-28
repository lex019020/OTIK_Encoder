using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OTIK_Encoder
{
    class ArchiveProcessor
    {
        public static void Encode(RandSplitType rSplitting, EntropicBasedCompressionType entCompr, 
            ContextBasedCompressionType cbCompr, AntiInterferenceType antiInterf, 
            string input, string output)
        {
            if (!FileLoader.IsCorrectLoadPath(input))
                throw new Exception("Input path is incorrect!");
            if (!ArchiveSaver.IsCorrectArchivePath(output))
                throw new Exception("Output path is incorrect!");

            var manager = new FileLoader(input);

            var header = new ArchiveHeader();
            header.SetFilesCount(manager.GetNumberOfFiles());
            header.SetRandSplitType(rSplitting);
            header.SetAntiInterferenceType(antiInterf);
            header.SetContextBasedCompressionType(cbCompr);
            header.SetEntropicBasedCompressionType(entCompr);

            var arcSaver = new ArchiveSaver(output);

            arcSaver.AppendBytes(header.GetHeaderBytes());

            var handler1 = new RandomSplitter(true);
            var handlingStruct = new FileHandlingStruct()
            {
                randomSplit_1 = rSplitting == RandSplitType.RandomSplit
            };

            while (manager.GetNextFile(out var bytes, out var name))
            {
                handlingStruct.bytes = bytes;
                handler1.Handle(ref handlingStruct);
                
                arcSaver.AppendFile(name, bytes);
            }

            arcSaver.CloseConnection();
        }

        public static void Decode(string input, string output)
        {
            if (!ArchiveLoader.IsCorrectArchivePath(input))
                throw new Exception("Input path is incorrect!");
            if (!FileSaver.IsCorrectSavePath(output))
                throw new Exception("Output path is incorrect!");
            
            var saver = new FileSaver(output);
            
            var arcLoader = new ArchiveLoader(input);
            var header = arcLoader.GetArchiveHeader();

            if (header.HasErrors())
                throw new Exception("Archive header contains errors!");

            var handler1 = new RandomSplitter(false);
            var handlingStruct = new FileHandlingStruct()
            {
                randomSplit_1 = header.GetRandSplitType() == RandSplitType.RandomSplit
            };

            while (arcLoader.ReadNextFile(out var name, out var bytes))
            {
                handlingStruct.bytes = bytes;
                handler1.Handle(ref handlingStruct);

                saver.AddFile(name, handlingStruct.bytes);
            }
            arcLoader.CloseStream();
        }

        public static List<string> ListFiles(string path)
        {
            if (!ArchiveLoader.IsCorrectArchivePath(path))
                throw new Exception("Input path is incorrect!");

            var arcLoader = new ArchiveLoader(path);


            arcLoader.CloseStream();
            return new List<string>();// todo
        }

        public static bool CheckErrors(string path)
        {
            if (!ArchiveLoader.IsCorrectArchivePath(path))
                throw new Exception("Input path is incorrect!");

            return false; // todo
        }
    }
}
