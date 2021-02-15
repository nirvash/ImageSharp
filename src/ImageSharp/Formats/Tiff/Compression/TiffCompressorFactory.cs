// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System.IO;
using SixLabors.ImageSharp.Compression.Zlib;
using SixLabors.ImageSharp.Formats.Experimental.Tiff.Compression.Compressors;
using SixLabors.ImageSharp.Formats.Experimental.Tiff.Constants;
using SixLabors.ImageSharp.Memory;

namespace SixLabors.ImageSharp.Formats.Experimental.Tiff.Compression
{
    internal static class TiffCompressorFactory
    {
        public static TiffBaseCompressor Create(
            TiffEncoderCompression method,
            Stream output,
            MemoryAllocator allocator,
            int width,
            int bitsPerPixel,
            DeflateCompressionLevel compressionLevel,
            TiffPredictor predictor)
        {
            switch (method)
            {
                case TiffEncoderCompression.None:
                    DebugGuard.IsTrue(compressionLevel == DeflateCompressionLevel.DefaultCompression, "No deflate compression level is expected to be set");
                    DebugGuard.IsTrue(predictor == TiffPredictor.None, "Predictor should only be used with lzw or deflate compression");

                    return new NoCompressor(output);

                case TiffEncoderCompression.PackBits:
                    DebugGuard.IsTrue(compressionLevel == DeflateCompressionLevel.DefaultCompression, "No deflate compression level is expected to be set");
                    DebugGuard.IsTrue(predictor == TiffPredictor.None, "Predictor should only be used with lzw or deflate compression");
                    return new PackBitsCompressor(output, allocator, width, bitsPerPixel);

                case TiffEncoderCompression.Deflate:
                    return new DeflateCompressor(output, allocator, width, bitsPerPixel, predictor, compressionLevel);

                case TiffEncoderCompression.Lzw:
                    DebugGuard.IsTrue(compressionLevel == DeflateCompressionLevel.DefaultCompression, "No deflate compression level is expected to be set");
                    return new LzwCompressor(output, allocator, width, bitsPerPixel, predictor);

                case TiffEncoderCompression.CcittGroup3Fax:
                    DebugGuard.IsTrue(compressionLevel == DeflateCompressionLevel.DefaultCompression, "No deflate compression level is expected to be set");
                    DebugGuard.IsTrue(predictor == TiffPredictor.None, "Predictor should only be used with lzw or deflate compression");
                    return new T4BitCompressor(output, allocator, width, bitsPerPixel, false);

                case TiffEncoderCompression.ModifiedHuffman:
                    DebugGuard.IsTrue(compressionLevel == DeflateCompressionLevel.DefaultCompression, "No deflate compression level is expected to be set");
                    DebugGuard.IsTrue(predictor == TiffPredictor.None, "Predictor should only be used with lzw or deflate compression");
                    return new T4BitCompressor(output, allocator, width, bitsPerPixel, true);

                default:
                    throw TiffThrowHelper.NotSupportedCompressor(nameof(method));
            }
        }
    }
}
