using ReplayReader.Models;

namespace ReplayReader.Util
{
    public interface IOutputWriter
    {
        void WriteObject(string filePath, Header header);
    }
}
