// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

namespace BiomSharp.Imaging.Wsq.Segment
{
    internal class Eoi : BaseSegment
    {
        public override Marker Marker => Marker.EOI;

        private Eoi() { }
    }
}
