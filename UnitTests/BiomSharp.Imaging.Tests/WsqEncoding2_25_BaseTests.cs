//using BiomSharp.Imaging.Pgm;
//using BiomSharp.Imaging.Tests.Models;
//using BiomSharp.Imaging.Wsq;
//using System.Runtime.CompilerServices;
//using System.Security.Cryptography;
//using System.Text.Json;
//using Xunit.Abstractions;

//namespace BiomSharp.Imaging.Tests;

//public abstract class WsqEncoding2_25_BaseTests
//{
//    protected readonly ITestOutputHelper output;

//    protected WsqEncoding2_25_BaseTests(ITestOutputHelper output) => this.output = output;

//    protected EncodingTestCase ArrangeEncoding(string imageName,
//        WsqFilterType filterType = WsqFilterType.Odd7x9)
//    {
//        Assert.True(Path.HasExtension(imageName) && Path.GetFileName(imageName) == imageName,
//            "The input image name must include a file extension and must not contain a directory path.");
//        string imageStem = Path.GetFileNameWithoutExtension(imageName);
//        string referenceName = Path.Combine("decode", "225", imageStem + ".wsq");
//        byte[] inputBytes = ReadVerifiedInput(imageName);
//        byte[]? referenceBytes = filterType == WsqFilterType.Odd7x9
//            ? ReadVerifiedInput(referenceName)
//            : null;

//        var pgm = new PgmCodec();
//        using (var input = new MemoryStream(inputBytes, writable: false))
//        {
//            pgm.Read(input);
//        }
//        SimpleBitmap bitmap = pgm.ToRaw() ?? throw new InvalidDataException("PGM reader returned no image.");
//        Assert.False(bitmap.IsColor);
//        Assert.Equal(checked(bitmap.Width * bitmap.Height), bitmap.Pixels.Length);

//        WsqParameters parameters = CreateEncodingParameters(filterType);
//        Assert.Equal(2.25f, parameters.BitRate);
//        var codec = new WsqCodec();
//        codec.FromRaw(bitmap);
//        return new EncodingTestCase(imageName, inputBytes, referenceBytes, bitmap, parameters, codec);
//    }

 

//    protected void AssertMatchesReference(EncodingTestCase test, byte[] encodedBytes)
//    {
//        EncodingTestReport report = ReadComparisonReport(test, encodedBytes);
//        output.WriteLine(JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true }));
//        Assert.True(report.Comparison.Failures.Count == 0,
//            $"{test.ImageName}: " + string.Join(Environment.NewLine, report.Comparison.Failures));
//    }

//    private protected static EncodingTestReport ReadComparisonReport(EncodingTestCase test, byte[] encodedBytes)
//    {
//        byte[] referenceBytes = test.ReferenceBytes
//            ?? throw new InvalidOperationException($"No NIST reference is configured for {test.Parameters.Filter}.");
//        WsqInspection reference = WsqInspection.Read(referenceBytes);
//        WsqInspection actual = WsqInspection.Read(encodedBytes);
//        return EncodingTestReport.Create(test.ImageName, test.Parameters, actual, reference,
//            test.Bitmap.Width, test.Bitmap.Height);
//    }

//    private static byte[] ReadVerifiedInput(string relativeName)
//    {
//        string source = Path.Combine(DatasetDirectory, relativeName);
//        Assert.True(File.Exists(source), $"Required NIST input is missing: {source}");
//        string manifest = Path.Combine(DatasetDirectory, "wsq_v2.0_pgm.md5");
//        Assert.True(File.Exists(manifest), $"Required NIST checksum manifest is missing: {manifest}");
//        string manifestName = relativeName.Replace(Path.DirectorySeparatorChar, '/');
//        string entry = Assert.Single(File.ReadLines(manifest)
//            .Where(line => line.Length > 34 && line[34..].Trim() == manifestName));
//        byte[] bytes = File.ReadAllBytes(source);
//        // MD5 is used only to check the supplied NIST dataset manifest, not for authentication.
//        Assert.Equal(entry[..32].ToUpperInvariant(), Convert.ToHexString(MD5.HashData(bytes)));
//        return bytes;
//    }

//    private static string FindUnitTestsDirectory([CallerFilePath] string sourcePath = "") =>
//        Directory.GetParent(Path.GetDirectoryName(sourcePath)!)!.FullName;
//}
