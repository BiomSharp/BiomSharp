// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using BiomSharp.Imaging.Wsq;

namespace BiomStudio.ViewModels
{
    [TypeDescriptionProvider(typeof(ViewModelDescriptionProvider))]
    [DisplayName("WSQ Codec")]
    [Description("Wavelet Scalar Quantization (WSQ) FBI/NIST format 8-bit"
        + " gray images used for fingerprint friction-ridge images")]
    public class WsqParametersViewModel : ViewModel
    {
        [DisplayName("NIST Header")]
        [Description("'NIST_COM' header segment. True if present, else false.")]
        [DefaultValue(false)]
        public bool NistHeader { get; set; } = false;

        [TypeConverter(typeof(FloatNumberRangeConverter))]
        [DisplayName("Compression Bit Rate")]
        [Description("WSQ compression bit rate (bits-per-pixel). " +
            "0.75 <= WSQ bitrate <= 2.25, default = 0.75.")]
        [Range(0.75f, 2.25f, ErrorMessage = "0.75 <= WSQ bitrate <= 2.25")]
        [DefaultValue(0.75)]
        public float BitRate { get; set; } = 0.75f;

        [TypeConverter(typeof(FingerprintResolutionConverter))]
        [DisplayName("Image DPI")]
        [Description("Image DPI resolution (dots-per-inch).")]
        [DefaultValue(500)]
        public int Resolution { get; set; } = -1;

        [DisplayName("Black Pixel")]
        [Description("Image black pixel value.")]
        [DefaultValue(0)]
        public int Black { get; set; } = 0;

        [DisplayName("White Pixel")]
        [Description("Image white pixel value.")]
        [DefaultValue(255)]
        public int White { get; set; } = 255;

        [DisplayName("WSQ Filter Tap")]
        [Description("Two channel subband encoder filter type.")]
        [DefaultValue(WsqFilterType.Odd7x9)]
        public WsqFilterType Filter { get; set; } = WsqFilterType.Odd7x9;

        [DisplayName("Packed DHT")]
        [Description("True if multiple Huffman tables are packed in a single" +
            " DHT segment, else false if each table is in a single, discrete" +
            " DHT segment.")]
        [DefaultValue(false)]
        public bool PackedDHT { get; set; } = false;

        [TypeConverter(typeof(ArrayPropertyConverter))]
        [DisplayName("Comments")]
        [Description("Text strings, each placed in a separate COM segment")]
        public string[] Comments { get; set; } = Array.Empty<string>();

        [DisplayName("Implementer Id")]
        [Description("FBI allocated number for organizations with certified WSQ codecs.")]
        [DefaultValue(0)]
        [ReadOnly(true)]
        public int ImplementerId { get; set; } = 0;

        public WsqParametersViewModel(object? parms, bool readOnly = false)
        {
            if (parms is not null and WsqParameters wsqParms)
            {
                BitRate = wsqParms.BitRate;
                Black = wsqParms.Black;
                Comments = wsqParms.Comments.Select(c => (string)c.Clone()).ToArray();
                Filter = wsqParms.Filter;
                ImplementerId = wsqParms.ImplementerId;
                PackedDHT = wsqParms.PackedDHT;
                NistHeader = wsqParms.NistHeader;
                Resolution = wsqParms.Resolution;
                White = wsqParms.White;
                ReadOnly = readOnly;
            }
        }

        public static implicit operator WsqParameters(WsqParametersViewModel viewModel)
            => new()
            {
                BitRate = viewModel.BitRate,
                Black = viewModel.Black,
                Comments = viewModel.Comments.Select(c => (string)c.Clone()).ToArray(),
                Filter = viewModel.Filter,
                ImplementerId = viewModel.ImplementerId,
                PackedDHT = viewModel.PackedDHT,
                NistHeader = viewModel.NistHeader,
                Resolution = viewModel.Resolution,
                White = viewModel.White,
            };
    }
}
