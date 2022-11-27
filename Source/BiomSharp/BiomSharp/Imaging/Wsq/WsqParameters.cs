// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

namespace BiomSharp.Imaging.Wsq
{
    public class WsqParameters
    {
        public static readonly WsqParameters Default = new();

        public WsqParameters()
        {
            NistHeader = false;
            BitRate = 0.75f;
            Resolution = -1;
            Black = 0;
            White = 255;
            Filter = WsqFilterType.Odd7x9;
            PackedDHT = false;
            Comments = Array.Empty<string>();
        }

        public bool NistHeader { get; set; } = false;

        private float bitRate = 0.75f;
        public float BitRate
        {
            get => bitRate;
            set => bitRate = value > 2.25f ? 2.25f : value < 0.75f ? 0.75f : value;
        }

        public int Resolution { get; set; } = -1;

        public int Black { get; set; } = 0;

        public int White { get; set; } = 255;

        public WsqFilterType Filter { get; set; } = WsqFilterType.Odd7x9;

        public bool PackedDHT { get; set; } = false;

        public string[] Comments { get; set; } = Array.Empty<string>();

        public int ImplementerId { get; set; } = 0;
    }
}
