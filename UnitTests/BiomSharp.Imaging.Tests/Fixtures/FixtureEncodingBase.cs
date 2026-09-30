using BiomSharp.Imaging.Pgm;
using BiomSharp.Imaging.Tests.Models;
using BiomSharp.Imaging.Wsq;
using System.Diagnostics;
using System.Security.Cryptography;

namespace BiomSharp.Imaging.Tests.Fixtures
{
    public abstract class FixtureEncodingBase<TFixture> : IClassFixture<TFixture> where TFixture : class
    {
        protected FixtureEncodingBase()
        {
        }

        private byte[] ReadVerifiedInput(string relativeName)
        {
            string source = Path.Combine(TestResources.DatasetDirectory, relativeName);
            Assert.True(File.Exists(source), $"Required NIST input is missing: {source}");
            string manifest = Path.Combine(TestResources.DatasetDirectory, "wsq_v2.0_pgm.md5");
            Assert.True(File.Exists(manifest), $"Required NIST checksum manifest is missing: {manifest}");
            string manifestName = relativeName.Replace(Path.DirectorySeparatorChar, '/');
            string entry = Assert.Single(File.ReadLines(manifest)
                .Where(line => line.Length > 34 && line[34..].Trim() == manifestName));
            byte[] bytes = File.ReadAllBytes(source);
            // MD5 is used only to check the supplied NIST dataset manifest, not for authentication.
            Assert.Equal(entry[..32].ToUpperInvariant(), Convert.ToHexString(MD5.HashData(bytes)));
            return bytes;
        }

        internal WsqParameters GetEncoding2_25Parameters(WsqFilterType filterType)
        {
            return new WsqParameters()
            {
                BitRate = 2.25f,
                Filter = filterType,
                NistHeader = true
            };
        }

        internal WsqParameters GetEncoding0_75Parameters(WsqFilterType filterType)
        {
            return new WsqParameters()
            {
                BitRate = 0.75f,
                Filter = filterType,
                NistHeader = true
            };
        }

        internal EncodingTestCase ArrangeEncoding(string imageName, WsqParameters parameters)
        {
            Assert.True(Path.HasExtension(imageName) && Path.GetFileName(imageName) == imageName,
                "The input image name must include a file extension and must not contain a directory path.");
            byte[] inputBytes = ReadVerifiedInput(imageName);

            var pgm = new PgmCodec();
            using (var input = new MemoryStream(inputBytes, writable: false))
            {
                pgm.Read(input);
            }

            SimpleBitmap bitmap = pgm.ToRaw() ?? throw new InvalidDataException("PGM reader returned no image.");
            Assert.False(bitmap.IsColor);
            Assert.Equal(checked(bitmap.Width * bitmap.Height), bitmap.Pixels.Length);                   

            var codec = new WsqCodec();
            codec.FromRaw(bitmap);
            return new EncodingTestCase(imageName, inputBytes, bitmap, parameters, codec);
        }

        internal TimedEncoding EncodeToMemory(EncodingTestCase test, Barrier? start = null)
        {
            try
            {
                var parameters = test.Parameters;

                var codec = new WsqCodec();
                codec.FromRaw(test.Bitmap.Clone());                
                if (start != null && !start.SignalAndWait(TimeSpan.FromSeconds(30)))
                {
                    throw new TimeoutException("Parallel WSQ encoders did not reach the start barrier within 30 seconds.");
                }

                DateTimeOffset startedUtc = DateTimeOffset.UtcNow;
                var timer = Stopwatch.StartNew();
                byte[] bytes = codec.Encode(parameters)
                    ?? throw new InvalidDataException($"WSQ encoder returned no output for {test.ImageName}.");
                timer.Stop();

                DateTimeOffset endedUtc = DateTimeOffset.UtcNow;
                return new TimedEncoding(bytes, timer.Elapsed, startedUtc, endedUtc);
            }
            finally
            {
                // A worker that fails during preparation must not leave the other workers blocked.
                start?.RemoveParticipant();
            }
        }

    }
}
