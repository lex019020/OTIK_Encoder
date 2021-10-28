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
            if (  !FileManager.PathIsCorrect(input)       // todo check this shit
                ||!FileManager.PathIsCorrect(output))
                throw new Exception("Input is incorrect!");

            var manager = new FileManager(input);

            // todo create & write header

            var handler1 = new RandomSplitter(true);

            var handlingStruct = new FileHandlingStruct() {randomSplit_1 = rSplitting == RandSplitType.RandomSplit };

            while (manager.GetNextFile(out var bytes, out var name))
            {
                handlingStruct.bytes = bytes;

                handler1.Handle(handlingStruct);
            }

            // todo write this shit

        }
    }
}
