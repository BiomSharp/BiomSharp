// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using System.IO.Compression;
using System.Text;

namespace BiomSharp.Nist.Nfiq
{
    public class RandomForestModel
    {
        public string? Name { get; }
        public string? Trainer { get; }
        public string? Description { get; }
        public string? Version { get; }
        public string? Hash { get; }

        public RandomForestModel(string txtModel)
        {
            Name = GetParam(txtModel, "Name");
            Trainer = GetParam(txtModel, "Trainer");
            Description = GetParam(txtModel, "Description");
            Version = GetParam(txtModel, "Version");
            Hash = GetParam(txtModel, "Hash");
        }

        private static string? GetParam(string txtModel, string paramName) => txtModel
                .Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault(p => p.Trim().ToLower().StartsWith(paramName.ToLower()))
                ?.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();

        public string UnzipToYaml(byte[] zippedBytes)
        {
            using var zipStream = new MemoryStream(zippedBytes);
            using Stream yamlStream = new ZipArchive(zipStream, ZipArchiveMode.Read).Entries[0].Open();
            using var reader = new StreamReader(yamlStream, Encoding.ASCII);
            return reader.ReadToEnd();
        }

        public override string ToString() => $"Name: {Name}\n" +
                $"Trainer: {Trainer}\n" +
                $"Description: {Description}\n" +
                $"Version: {Version}\n" +
                $"Hash: {Hash}";
    }
}
