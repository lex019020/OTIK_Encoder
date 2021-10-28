using System;

namespace OTIK_Encoder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintHelp();

                return;
            }

            string inpath;
            string outpath;

            switch (args[0]) // mode
            {
                case "-c": // compress

                    if (args.Length != 4)
                    {
                        Console.WriteLine("Incorrect parameters count!");
                        PrintHelp();
                        return;
                    }

                    switch (args[1]) // compression types
                    {
                        case "r1": // random splitting

                            var rSplitting = RandSplitType.RandomSplit;
                            var entCompr = EntropicBasedCompressionType.None;
                            var cbCompr = ContextBasedCompressionType.None;
                            var antiInterf = AntiInterferenceType.None;
                            outpath = args[2];
                            inpath = args[3];

                            try
                            {
                                ArchiveProcessor.Encode(rSplitting, entCompr, cbCompr,
                                    antiInterf, inpath, outpath);
                                Console.WriteLine("");
                                Console.WriteLine("Compressed successfully");
                                Console.WriteLine("");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }

                            break;
                        default:
                            Console.WriteLine("Incorrect compression type!");
                            PrintHelp();
                            return;
                    }

                    break;
                case "-d": // decompress

                    if (args.Length != 3)
                    {
                        Console.WriteLine("Incorrect parameters count!");
                        PrintHelp();
                        return;
                    }

                    outpath = args[1];
                    inpath = args[2];

                    try
                    {
                        ArchiveProcessor.Decode(inpath, outpath);
                        Console.WriteLine("");
                        Console.WriteLine("Decompressed successfully");
                        Console.WriteLine("");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }


                    break;
                case "-t": // test

                    if (args.Length != 2)
                    {
                        Console.WriteLine("Incorrect parameters count!");
                        PrintHelp();
                        return;
                    }

                    inpath = args[1];

                    try
                    {
                        var hasErrors = ArchiveProcessor.CheckErrors(inpath);
                        Console.WriteLine(hasErrors ? "Archive file has errors!" : "Archive file is OK");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    break;
                case "-l": // list

                    if (args.Length != 2)
                    {
                        Console.WriteLine("Incorrect parameters count!");
                        PrintHelp();
                        return;
                    }

                    inpath = args[1];

                    try
                    {
                        var files = ArchiveProcessor.ListFiles(inpath);
                        Console.WriteLine("Files list:   [size (compressed)    name]");
                        Console.WriteLine("");
                        foreach (var filestr in files) Console.WriteLine(filestr);
                        Console.WriteLine("");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

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
            Console.WriteLine(
                " [compression modes] (only if mode is -c) is list of applied encoding/compression algorithms:");
            Console.WriteLine("     r1 Split files to random pieces size of 1-16 bytes");
            Console.WriteLine("");
            Console.WriteLine(" [output] (only in modes -c/-d)");
            Console.WriteLine(
                "     output filename (for -c) or path (for -d); if contains whitespaces - write it in double quotes");
            Console.WriteLine("         if mode is -c, must end in .otik");
            Console.WriteLine("");
            Console.WriteLine(" *[input] is input file / directory");
            Console.WriteLine("     if input path contains whitespaces, path must be in double quotes.");
            Console.WriteLine("");
        }
    }
}