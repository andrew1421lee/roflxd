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
                new Option {Key = "--set-output", Description = "Set the output path", Usage = "ReplayReader.exe --set-output [OUTPUT PATH]"}
            };

        public static void Main(string[] args)
        {
            ////////////// CATCH ARGUMENTS
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: ReplayReader.exe [OPTIONS] [PATH]\n\tAvailable options:");
                foreach (var option in OPTIONS)
                {
                    Console.WriteLine($"\t{option.Key}\t\t{option.Description}");
                }
                Environment.Exit(-1);
            }

            ////////////// OPTIONS
            // Currently hardcoded, fix in the future to allow for easier upgradability
            if(args[0].Equals("--set-output"))
            {
                if(args.Length != 2)
                {
                    Console.WriteLine($"Usage: {OPTIONS[0].Usage}");
                    Environment.Exit(-1);
                }

                SetOutpath(args[1]);
                Console.WriteLine($"Changed output path to: {args[1]}");
                Environment.Exit(-1);
            }

            ////////////// READ REPLAYS
            string inputPath = args[0];
            // Ensure ouput path is set
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["OutputPath"]))
            {
                Console.WriteLine($"Output path is not set, run {OPTIONS[0].Key}");
                Environment.Exit(-1);
            }
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
                Console.WriteLine(fileResult.ToString());
            }

            IOutputWriter writer = new FileWriter();
            writer.WriteObject(ConfigurationManager.AppSettings["OutputPath"], fileResult);
        }

        private static void SetOutpath(string path)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configFile.AppSettings.Settings["OutputPath"].Value = path;
            configFile.Save(ConfigurationSaveMode.Modified);
        }
    }
}
