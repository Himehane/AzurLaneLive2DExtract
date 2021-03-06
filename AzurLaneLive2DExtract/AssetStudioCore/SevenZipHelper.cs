using System;
using System.IO;
using SevenZip.Compression.LZMA;


namespace AssetStudioCore
{
    public static class SevenZipHelper
    {
        public static MemoryStream StreamDecompress(MemoryStream inStream)
        {
            var decoder = new Decoder();

            inStream.Seek(0, 0);
            var newOutStream = new MemoryStream();

            var properties = new byte[5];
            if (inStream.Read(properties, 0, 5) != 5)
                throw (new Exception("input .lzma is too short"));
            long outSize = 0;
            for (var i = 0; i < 8; i++)
            {
                var v = inStream.ReadByte();
                if (v < 0)
                    throw (new Exception("Can't Read 1"));
                outSize |= ((long)(byte)v) << (8 * i);
            }
            decoder.SetDecoderProperties(properties);

            var compressedSize = inStream.Length - inStream.Position;
            decoder.Code(inStream, newOutStream, compressedSize, outSize, null);

            newOutStream.Position = 0;
            return newOutStream;
        }

        public static void StreamDecompress(Stream inStream, Stream outStream, long inSize, long outSize)
        {
            var decoder = new Decoder();
            var properties = new byte[5];
            if (inStream.Read(properties, 0, 5) != 5)
                throw new Exception("input .lzma is too short");
            decoder.SetDecoderProperties(properties);
            inSize -= 5L;
            decoder.Code(inStream, outStream, inSize, outSize, null);
        }
    }
}
