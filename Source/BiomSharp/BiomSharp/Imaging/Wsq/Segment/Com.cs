// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using BiomSharp.Imaging.Wsq.IO;

namespace BiomSharp.Imaging.Wsq.Segment
{
    internal class Com : DataSegment
    {
        public override Marker Marker => Marker.COM;
        public string? Comment { get; protected set; }

        private Com() { }

        public Com(string comment) => Set(comment);

        public void Set(string comment)
        {
            Comment = comment;
            ContentSize = Comment.Length;
        }

        protected override void Read(EndianBinaryReader reader, Marker marker)
        {
            base.Read(reader, marker);
            Comment = ContentSize > 0 ?
                reader.ReadString(ContentSize)
                :
                string.Empty;
            Deserialized = true;
        }

        public override void Write(EndianBinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(Comment ?? "");
        }
    }
}
