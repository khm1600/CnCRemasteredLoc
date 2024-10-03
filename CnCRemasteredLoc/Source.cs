using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace CnCRemasteredLoc
{
    public static class Extractor
    {
        public static string LocFileToJson(string path)
        {
            var entries = LoadFile(path);

            var json = JsonSerializer.Serialize(entries);

            return json;
        }

        public static string LocFileToCsv(string path)
        {
            var entries = LoadFile(path);

            var result = new StringBuilder();
            result.AppendLine("\"entryTag\",\"entry\"");

            foreach (var item in entries)
            {
                result.AppendLine($"\"{item.entryTag}\",\"{item.entry.Replace("\"", "\"\"")}\"");
            }
            return result.ToString();
        }

        static Entry[] LoadFile(string path)
        {
            using var s = File.OpenRead(path);

            var count = ReadUInt32(s);

            var result = new Entry[count];

            for (int i = 0; i < count; i++)
            {
                result[i].magic = ReadUInt32(s);
                result[i].entryLength = ReadUInt32(s);
                result[i].entryTagLength = ReadUInt32(s);
            }

            for (int i = 0; i < count; i++)
            {
                var length = result[i].entryLength;
                var sb = new StringBuilder((int)length);
                for (int j = 0; j < length; j++)
                {
                    var ch = ReadUInt16(s);
                    _ = sb.Append((char)ch);
                }
                result[i].entry = sb.ToString();
            }

            for (int i = 0; i < count; i++)
            {
                var length = result[i].entryTagLength;
                var sb = new StringBuilder((int)length);
                for (int j = 0; j < length; j++)
                {
                    var ch = s.ReadByte();
                    _ = sb.Append((char)ch);
                }
                result[i].entryTag = sb.ToString();
            }

            return result;

            static uint ReadUInt32(Stream s)
            {
                uint result = 0;

                for (int i = 0; i < 4; i++)
                {
                    var b = s.ReadByte();
                    if (b == -1)
                    {
                        throw new EndOfStreamException();
                    }
                    result |= (uint)(b & 0xFF) << (i * 8);
                }

                return result;
            }

            static ushort ReadUInt16(Stream s)
            {
                ushort result = 0;

                for (int i = 0; i < 2; i++)
                {
                    var b = s.ReadByte();
                    if (b == -1)
                    {
                        throw new EndOfStreamException();
                    }
                    result |= (ushort)((b & 0xFF) << (i * 8));
                }

                return result;
            }
        }
    }

    struct Entry
    {
        public uint magic;
        public uint entryLength;
        public uint entryTagLength;
        public string entry { get; set; }
        public string entryTag { get; set; }
    }
}
