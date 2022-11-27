// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using BiomSharp.Plugins;

namespace BiomSharp.Imaging
{
    public abstract class BitmapCodecFactory<TFormat, TBitmapCodec>
        :
        IBitmapCodecFactory<TFormat, TBitmapCodec>
        where TBitmapCodec : IPlugin<TFormat>, IBitmapCodec
        where TFormat : Enum
    {
        private readonly SortedList<TFormat, IBitmapCodec> codecs = new();

        public IBitmapCodec[] Codecs => codecs.Values.Cast<IBitmapCodec>().ToArray();

        protected BitmapCodecFactory() { }

        protected static IBitmapCodec? Create(Type type, out TFormat? format)
        {
            var codec = Activator.CreateInstance(
                type.MakeGenericType(typeof(TFormat))) as IBitmapCodec;
            format = codec is not null and IPlugin<TFormat> plugin ? plugin.Id : default;
            return codec;
        }

        public virtual IBitmapCodec? this[TFormat format]
        {
            get => codecs.ContainsKey(format) ? codecs[format] : null;
            set
            {
                if (codecs.ContainsKey(format) && value != null)
                {
                    codecs[format] = value;
                }
                else
                {
                    if (value != null)
                    {
                        if (value.FileExtensions != null
                            && Get(value.FileExtensions) is IBitmapCodec codec
                            && codec != null
                            && codec is IPlugin<TFormat> plugin
                            && plugin.Id != null)
                        {
                            codecs[plugin.Id] = value;
                        }
                        else
                        {
                            codecs.Add(format, value);
                        }
                    }
                }
            }
        }

        public virtual IBitmapCodec? Get(TFormat format)
            => codecs.ContainsKey(format) ? codecs[format] : default;

        public virtual IBitmapCodec? Get(params string[] fileExtensions)
            => codecs.Values.FirstOrDefault(
            codec => codec.FileExtensions
            ?.Select(e => e.ToLower())
            ?.Intersect(fileExtensions.Select(e => e.ToLower())).Count() > 0);

        public virtual IBitmapCodec? Create(TFormat format)
            =>
            codecs.ContainsKey(format)
            ? Create(codecs[format].GetType(), out TFormat? _) : null;

        public virtual bool Exists(TFormat format) => codecs.ContainsKey(format);
    }
}
