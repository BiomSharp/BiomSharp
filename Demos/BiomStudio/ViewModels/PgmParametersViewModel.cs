// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using System.ComponentModel;

using BiomSharp.Imaging.Pgm;

namespace BiomStudio.ViewModels
{
    [TypeDescriptionProvider(typeof(ViewModelDescriptionProvider))]
    [DisplayName("PGM Codec")]
    [Description("Portable Gray Map (PGM) format - supports only 8-bit gray images")]
    public class PgmParametersViewModel : ViewModel
    {
        [DisplayName("Format")]
        [Description("Pixel format - BIN'ary or ASC'ii text")]
        [DefaultValue(PgmFormatType.BIN)]
        public PgmFormatType Format { get; set; } = PgmFormatType.BIN;

        [TypeConverter(typeof(ArrayPropertyConverter))]
        [DisplayName("Comments")]
        [Description("Text strings each stored with a preceding hash-tag and space")]
        public string[] Comments { get; set; } = Array.Empty<string>();

        public PgmParametersViewModel(object? parms, bool readOnly = false)
        {
            if (parms is not null and PgmParameters pgmParms)
            {
                Comments = pgmParms.Comments.Select(c => (string)c.Clone()).ToArray();
                Format = pgmParms.Format;
                ReadOnly = readOnly;
            }
        }

        public static implicit operator PgmParameters(PgmParametersViewModel viewModel)
            => new()
            {
                Comments = viewModel.Comments.Select(c => (string)c.Clone()).ToArray(),
                Format = viewModel.Format,
            };
    }
}
