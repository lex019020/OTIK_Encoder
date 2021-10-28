using System;
using System.Collections.Generic;
using System.Linq;
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
            if (  !FileLoader.IsCorrectLoadPath(input)       // todo check this shit
                ||!FileLoader.IsCorrectLoadPath(output))
                throw new Exception("Input path is incorrect!");

            var manager = new FileLoader(input);

            var header = new ArchiveHeader();
            header.SetFilesCount(manager.GetNumberOfFiles());
            header.SetRandSplitType(rSplitting);
            header.SetAntiInterferenceType(antiInterf);
            header.SetContextBasedCompressionType(cbCompr);
            header.SetEntropicBasedCompressionType(entCompr);

            // todo write header

            var handler1 = new RandomSplitter(true);
            var handlingStruct = new FileHandlingStruct() {randomSplit_1 = rSplitting == RandSplitType.RandomSplit };

            while (manager.GetNextFile(out var bytes, out var name))
            {
                handlingStruct.bytes = bytes;
                handler1.Handle(ref handlingStruct);
                
                // todo write bytes
            }
        }
    }
}
