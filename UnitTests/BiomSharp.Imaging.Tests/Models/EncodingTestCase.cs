using BiomSharp.Imaging.Wsq;

namespace BiomSharp.Imaging.Tests.Models;

public sealed record EncodingTestCase(string ImageName, 
    byte[] InputBytes, 
   SimpleBitmap Bitmap, 
   WsqParameters Parameters, 
   WsqCodec Codec);
