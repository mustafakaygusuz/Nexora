using ProtoBuf;
using System.IO.Compression;

namespace Nexora.Core.Common.Extensions
{
    public static class CompressionExtensions
    {
        public static byte[] CompressAsBytes<T>(T data)
        {
            using var stream = new MemoryStream();
            Serializer.Serialize(stream, data);
            return CompressByBrotli(stream.ToArray());
        }

        public static T DecompressFromBytes<T>(byte[] data)
        {
            var cacheDecompressedData = DecompressByBrotli(data);
            using var stream = new MemoryStream(cacheDecompressedData);
            return Serializer.Deserialize<T>(stream);
        }

        public static string CompressAsString(string data)
        {
            using var memoryStream = new MemoryStream();
            Serializer.Serialize(memoryStream, data);
            var compressedData = CompressByBrotli(memoryStream.ToArray());
            return Convert.ToBase64String(compressedData);
        }

        public static string DecompressFromString(string data)
        {
            var decodedData = Convert.FromBase64String(data);
            var decompressedData = DecompressByBrotli(decodedData);
            using var stream = new MemoryStream(decompressedData);
            return Serializer.Deserialize<string>(stream);
        }

        private static byte[] CompressByBrotli(byte[] data)
        {
            using var input = new MemoryStream(data);
            using var output = new MemoryStream();
            using var stream = new BrotliStream(output, CompressionLevel.Optimal);
            input.CopyTo(stream);
            stream.Flush();
            return output.ToArray();
        }

        private static byte[] DecompressByBrotli(byte[] data)
        {
            using var input = new MemoryStream(data);
            using var output = new MemoryStream();
            using var stream = new BrotliStream(input, CompressionMode.Decompress);
            stream.CopyTo(output);
            stream.Flush();

            return output.ToArray();
        }
    }
}