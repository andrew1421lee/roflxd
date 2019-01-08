using ReplayReader.Models;
using ReplayReader.Util;
using System;
using System.IO;
using System.Configuration;

namespace ReplayReader
{
    public class Program
    {
        public static Option[] OPTIONS =
            {
                new Option {Key = "-o", Description = "Set the output path", Usage = "ReplayReader.exe -o [OUTPUT PATH]"}
            };


        private static int inputIndex = 0;
        private static string outputPath = "";

        public static void Main(string[] args)
        {
            ////////////// CATCH ARGUMENTS
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: ReplayReader.exe [OPTIONS] [REPLAY PATH]\n\tAvailable options:");
                foreach (var option in OPTIONS)
                {
                    Console.WriteLine($"\t{option.Key}\t\t{option.Description}");
                }
                Environment.Exit(-1);
            }

            ////////////// OPTIONS
            // Currently hardcoded, fix in the future to allow for easier upgradability
            if(args[0].Equals("-o"))
            {
                if(args.Length != 3)
                {
                    Console.WriteLine($"Usage: {OPTIONS[0].Usage}");
                    Environment.Exit(-1);
                }

                outputPath = args[1];
                inputIndex += 2;
            }

            ////////////// READ REPLAYS
            string inputPath = args[inputIndex];
            // Ensure input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file does not exist: {inputPath}");
                Environment.Exit(-1);
            }

            Header fileResult;

            using (var bReader = new BinaryReader(File.Open(inputPath, FileMode.Open)))
            {
                fileResult = HeaderReader.Read(bReader);
            }

            ////////////// WRITE OUTPUT
            Console.WriteLine(fileResult.Serialize());
            if (!string.IsNullOrEmpty(outputPath))
            {
                IOutputWriter writer = new FileWriter();
                writer.WriteObject(outputPath, fileResult);
            }
        }
    }
}
