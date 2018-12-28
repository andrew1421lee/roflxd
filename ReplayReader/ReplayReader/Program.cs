using ReplayReader.Util;
using System;
using System.IO;

namespace ReplayReader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if(args.Length != 1)
            {
                Console.WriteLine("Usage: ReplayReader.exe [MODE] [PATH]");
                Environment.Exit(-1);
            }

            string inputPath = args[0];

            if(!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file does not exist: {inputPath}");
                Environment.Exit(-1);
            }

            using (var bReader = new BinaryReader(File.Open(inputPath, FileMode.Open)))
            {
                var result = HeaderReader.Read(bReader);
                Console.WriteLine(result.ToString());
            }
        }
    }
}
