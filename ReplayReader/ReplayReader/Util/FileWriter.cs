using System;
using System.IO;
using ReplayReader.Models;

namespace ReplayReader.Util
{
    public class FileWriter : IOutputWriter
    {
        public void WriteObject(string filePath, Header header)
        {
            filePath += $"\\{header.DataHeader.MatchId}.json";
            if(File.Exists(filePath)) { return; }

            try
            {
                using (var writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(header.Serialize());
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"[FileWriter] Encountered exception {ex.ToString()}");
            }
        }
    }
}
