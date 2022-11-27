// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using System.Reflection;
using BiomSharp.Plugins;

namespace BiomSharp.Imaging
{
    public sealed class DefaultBitmapCodecFactory<TFormat, TBitmapCodec>
        :
        BitmapCodecFactory<TFormat, TBitmapCodec>
        where TBitmapCodec : IPlugin<TFormat>, IBitmapCodec
        where TFormat : Enum
    {
        public DefaultBitmapCodecFactory()
            => PluginRegister
            .GetPlugins(Assembly.GetExecutingAssembly(),
                typeof(IPlugin<TFormat>), typeof(IBitmapCodec))
            .ToList()
            .ForEach(type =>
            {
                if (Create(type, out TFormat? format) is IBitmapCodec codec)
                {
                    this[format!] = codec;
                }
            });
    }
}
