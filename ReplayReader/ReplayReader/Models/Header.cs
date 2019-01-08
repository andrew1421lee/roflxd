using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace ReplayReader.Models
{
    /// <summary>
    /// Model of Replay Header Data
    /// </summary>
    public class Header
    {
        public IndexFields Index;
        public MetadataFields Metadata;
        public DataHeaderFields DataHeader;

        public override string ToString()
        {
            return $"{Index.ToString()}\n{Metadata.ToString()}\n{DataHeader.ToString()}";
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    /// <summary>
    /// Locations and sizes of file sections
    /// </summary>
    public struct IndexFields
    {
        public ushort HeaderSize;
        public uint FileSize;

        public uint MetadataOffset;
        public uint MetadataSize;

        public uint DataHeaderOffset;
        public uint DataHeaderSize;
        public uint DataOffset;

        public override string ToString()
        {
            return String.Format("HeaderSize :\t\t{0}\n" +
                                 "FileSize :\t\t{1}\n" +
                                 "MetadataOffset :\t{2}\n" + 
                                 "MetadataSize :\t\t{3}\n" +
                                 "DataHeaderOffset :\t{4}\n" +
                                 "DataHeaderSize :\t{5}\n" +
                                 "DataOffset :\t\t{6}",
            HeaderSize, FileSize, MetadataOffset, MetadataSize, DataHeaderOffset, DataHeaderSize, DataOffset);
        }
    }

    /// <summary>
    /// Post game stats
    /// </summary>
    public struct MetadataFields
    {
        public ulong GameLength;
        public string GameVersion;

        public uint LastGameChunkID;
        public uint LastKeyframeID;

        public JArray PlayerData;

        public override string ToString()
        {
            return String.Format("GameLength :\t\t{0}\n" +
                                 "GameVersion :\t\t{1}\n" +
                                 "LastGameChunkID :\t{2}\n" +
                                 "LastKeyframeID :\t{3}\n" +
                                 "Player Count :\t\t{4}",
            GameLength, GameVersion, LastKeyframeID, LastKeyframeID, PlayerData.Count);
        }
    }

    /// <summary>
    /// Fields for reading payload
    /// </summary>
    public struct DataHeaderFields
    {
        public ulong MatchId;
        public uint MatchLength;
        public uint KeyframeCount;
        public uint ChunkCount;
        public uint LastChunkID;
        public uint FirstChunkID;
        public uint KeyframeInterval;
        public ushort EncryptionKeySize;
        public string EncryptionKey; // base64

        public override string ToString()
        {
            return String.Format("MatchId :\t\t{0}\n" +
                                 "MatchLength :\t\t{1}\n" +
                                 "KeyframeCount :\t\t{2}\n" +
                                 "ChunkCount :\t\t{3}\n" +
                                 "LastChunkID :\t\t{4}\n" +
                                 "FirstChunkID :\t\t{5}\n" +
                                 "KeyframeInterval  :\t{6}\n" +
                                 "EncryptionKeySize :\t{7}\n" +
                                 "EncryptionKey :\t\t{8}",
            MatchId, MatchLength, KeyframeCount, ChunkCount, LastChunkID, FirstChunkID, KeyframeInterval, EncryptionKeySize, EncryptionKey);
        }
    }
}
