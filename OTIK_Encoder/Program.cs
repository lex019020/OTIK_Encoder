using System;

namespace OTIK_Encoder
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintHelp();

                return;
            }

            string inpath;
            string outpath;
            char mode;
            byte rSplitting = 0, entCompr = 0, cbCompr = 0, antiInterf = 0; 

            switch (args[0]) // mode
            {
                case "-c":  // compress
                    mode = 'c';

                    if (args.Length != 4)
                    {
                        Console.WriteLine("Incorrect parameters count!");
                        PrintHelp();
                        return;
                    }

                    switch (args[1])    // compression types
                    {
                        case "r1":      // random splitting

                            outpath = args[2];
                            inpath = args[3];

                            // todo call methods

                            break;
                        default:
                            Console.WriteLine("Incorrect compression type!");
                            PrintHelp();
                            return;
                    }

                    break;
                case "-d":  // decompress
                    mode = 'd';

                    if (args.Length != 3)
                    {
                        Console.WriteLine("Incorrect parameters count!");
                        PrintHelp();
                        return;
                    }

                    outpath = args[1];
                    inpath = args[2];

                    // todo call methods


                    break;
                case "-t":  // test
                    mode = 't';

                    if (args.Length != 2)
                    {
                        Console.WriteLine("Incorrect parameters count!");
                        PrintHelp();
                        return;
                    }
                    
                    inpath = args[2];

                    // todo call methods

                    break;
                case "-l":  // list
                    mode = 'l';

                    if (args.Length != 2)
                    {
                        Console.WriteLine("Incorrect parameters count!");
                        PrintHelp();
                        return;
                    }

                    inpath = args[2];

                    // todo call methods

                    break;
                default:
                    Console.WriteLine("Incorrect mode!");
                    PrintHelp();
                    return;
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Simple encoder/archivator, version 1.");
            Console.WriteLine("Usage: [mode] [compression modes] [output] [input]");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Where:   ( * is mandatory parameters )");
            Console.WriteLine(" *[mode] is working mode:");
            Console.WriteLine("     -c  Compress or enCode");
            Console.WriteLine("     -d  Decompress or Decode");
            Console.WriteLine("     -t  Testing");
            Console.WriteLine("     -l  Files list");
            Console.WriteLine("");
            Console.WriteLine(" [compression modes] (only if mode is -c) is list of applied encoding/compression algorithms:");
            Console.WriteLine("     r1 Split files to random pieces size of 1-16 bytes");
            Console.WriteLine("");
            Console.WriteLine(" [output] (only in modes -c/-d)");
            Console.WriteLine("     output filename; if contains whitespaces - write it in double quotes");
            Console.WriteLine("");
            Console.WriteLine(" *[input] is input file / directory");
            Console.WriteLine("     if input path contains whitespaces, path must be in double quotes");
        }
    }
}
