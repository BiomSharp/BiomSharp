// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using System.Reflection;
using System.Text;
using BiomSharp.Imaging;
using BiomSharp.Plugins;
using BiomStudio.Data;

namespace BiomStudio.Factories.Imaging
{
    internal class PluginCodecFactory<TBitmapCodec>
        : BitmapCodecFactory<BitmapFormat, TBitmapCodec>
        where TBitmapCodec : IPlugin<BitmapFormat>, IBitmapCodec
    {
        public string FileDialogFilter { get; }

        private void LoadCodecs(
            string folderPath, ImagingFx fx)
        {
            Assembly? asm = fx switch
            {
                ImagingFx.BioSharp =>
                    Assembly.LoadFrom(Path.Combine(folderPath, "BiomSharp.dll")),
                ImagingFx.Windows =>
                    Assembly.LoadFrom(Path.Combine(folderPath, "BiomSharp.Windows.dll")),
                ImagingFx.ImageSharp =>
                    Assembly.LoadFrom(Path.Combine(folderPath, "BiomSharp.ImageSharp.dll")),
                ImagingFx.ImageMagick => throw new NotImplementedException(),
                _ => throw new NotImplementedException(),
            };
            if (asm != null)
            {
                var codecFactory = IBitmapCodecFactory<BitmapFormat, TBitmapCodec>
                    .GetDefaultInstance(asm);
                if (codecFactory != null)
                {
                    Enum.GetValues<BitmapFormat>()
                        .Where(format => codecFactory.Exists(format)
                            && codecFactory.Get(format) != null)
                        .ToList().ForEach(fmt => this[fmt] = codecFactory.Get(fmt));
                }
            }
        }

        public PluginCodecFactory(ImagingFx fx)
        {
            // Load default first, always!
            LoadCodecs(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
                ImagingFx.BioSharp);
            LoadCodecs(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, fx);

            FileDialogFilter = GetWindowsFileDialogFilter(includeAllImages: true);
        }

        public IBitmapCodec? CreateFromFilePath(string filePath)
            =>
            Get(Path.GetExtension(filePath.ToLower()));

        public string GetWindowsFileDialogFilter(
            bool includeAllImages)
        {
            StringBuilder sb = new();
            var codecList = Codecs.ToList();

            if (includeAllImages)
            {
                _ = sb.Append($"All Image Files|");

                codecList
                .ForEach(codec => _ = (codec.FileExtensions?.Aggregate(0, (index, e) =>
                {
                    _ = sb.Append($"*{e};");
                    return index + 1;
                })));
                _ = sb.Remove(sb.Length - 1, 1);
            }
            codecList
            .ForEach(codec =>
            {
                if (sb.Length > 0)
                {
                    _ = sb.Append('|');
                }

                if (codec is not IPlugin<BitmapFormat> plugin)
                {
                    throw new InvalidOperationException();
                }
                _ = sb.Append($"{plugin.Id} Files|");
                _ = (codec.FileExtensions?.Aggregate(0, (index, e) =>
                {
                    if (index > 0)
                    {
                        _ = sb.Append(';');
                    }

                    _ = sb.Append($"*{e}");
                    return index + 1;
                }));
            });
            return sb.ToString();
        }
    }
}
