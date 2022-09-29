// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.Reflection;
using BiomSharp.Plugins;

namespace BiomSharp.Imaging
{
    public interface IBitmapCodecFactory<TFormat, TBitmapCodec>
        where TBitmapCodec : IPlugin<TFormat>, IBitmapCodec
        where TFormat : Enum
    {
        protected static IBitmapCodecFactory<TFormat, TBitmapCodec>? GetDefaultInstance(
            Assembly assembly)
        {
            Type? factoryType = PluginRegister.GetPlugins(assembly,
                    typeof(IBitmapCodecFactory<TFormat, TBitmapCodec>))
                    .FirstOrDefault();
            return factoryType != null
                ?
                Activator.CreateInstance(
                factoryType.MakeGenericType(typeof(TFormat), typeof(TBitmapCodec)))
                as IBitmapCodecFactory<TFormat, TBitmapCodec>
                :
                null;
        }

        bool Exists(TFormat format);

        IBitmapCodec? Create(TFormat format);

        IBitmapCodec? Get(TFormat format);

        IBitmapCodec? this[TFormat format] { get; set; }
    }
}
