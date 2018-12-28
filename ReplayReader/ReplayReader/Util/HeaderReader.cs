using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using ReplayReader.Models;

namespace ReplayReader.Util
{
    public class HeaderReader
    {
        public static Header Read(BinaryReader byteStream)
        {
            if(byteStream == null) { throw new ArgumentNullException(); }

            try
            {
                // Read the first 4 bytes from the file and check the magic sequence
                var magicChars = byteStream.ReadChars(4);
                if (!magicChars.SequenceEqual("RIOT"))
                {
                    throw new Exception("Magic sequence did not match");
                }

                // Skip 258 bytes
                byteStream.ReadBytes(258);

                // Read the index 
                var indexBytes = byteStream.ReadBytes(26);
                // Make into object
                var indexType = ConstructIndexFields(indexBytes);

                // Verify our current position
                if (indexType.MetadataOffset != 288) { throw new EndOfStreamException("Metadata offset does not match current position"); }

                // Read the metadata
                var metadataChars = byteStream.ReadChars((int)indexType.MetadataSize);
                // Make into object
                var metadataType = ConstructMetadataFields(metadataChars);

                // Verify our current position
                if (indexType.DataHeaderOffset != indexType.MetadataOffset + indexType.MetadataSize) { throw new EndOfStreamException("Data Header offset does not match current position"); }

                // Read the data header
                var dataHeaderBytes = byteStream.ReadBytes((int)indexType.DataHeaderSize);
                // Make into object
                var dataHeaderType = ConstructDataHeaderFields(dataHeaderBytes);

                return new Header { Index = indexType, Metadata = metadataType, DataHeader = dataHeaderType };
            }
            catch (Exception ex)
            {
                throw new Exception($"[HeaderReader] Encountered exception {ex.ToString()}");
            }
        }

        private static IndexFields ConstructIndexFields(byte[] byteData)
        {
            var result = new IndexFields { };
            result.HeaderSize = BitConverter.ToUInt16(byteData, 0);
            result.FileSize = BitConverter.ToUInt32(byteData, 2);
            result.MetadataOffset = BitConverter.ToUInt32(byteData, 6);
            result.MetadataSize = BitConverter.ToUInt32(byteData, 10);
            result.DataHeaderOffset = BitConverter.ToUInt32(byteData, 14);
            result.DataHeaderSize = BitConverter.ToUInt32(byteData, 18);
            result.DataOffset = BitConverter.ToUInt32(byteData, 22);

            return result;
        }

        private static MetadataFields ConstructMetadataFields(char[] charData)
        {
            var result = new MetadataFields { };
            var jsonstring = new string(charData);

            var jsonobject = JObject.Parse(jsonstring);

            result.GameLength = (ulong)jsonobject["gameLength"];
            result.GameVersion = (string)jsonobject["gameVersion"];
            result.LastGameChunkID = (uint)jsonobject["lastGameChunkId"];
            result.LastKeyframeID = (uint)jsonobject["lastKeyFrameId"];

            result.PlayerData = JArray.Parse(((string)jsonobject["statsJson"]).Replace(@"\", ""));

            return result;
        }

        private static DataHeaderFields ConstructDataHeaderFields(byte[] byteData)
        {
            var result = new DataHeaderFields { };

            result.MatchId = BitConverter.ToUInt64(byteData, 0);
            result.MatchLength = BitConverter.ToUInt32(byteData, 8);
            result.KeyframeCount = BitConverter.ToUInt32(byteData, 12);
            result.ChunkCount = BitConverter.ToUInt32(byteData, 16);
            result.LastChunkID = BitConverter.ToUInt32(byteData, 20);
            result.FirstChunkID = BitConverter.ToUInt32(byteData, 24);
            result.KeyframeInterval = BitConverter.ToUInt32(byteData, 28);
            result.EncryptionKeySize = BitConverter.ToUInt16(byteData, 32);
            result.EncryptionKey = Encoding.UTF8.GetString(byteData, 34, result.EncryptionKeySize);

            return result;
        }
    }
}
