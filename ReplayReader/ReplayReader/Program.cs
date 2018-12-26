using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}
