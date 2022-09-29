// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using BiomSharp.Imaging.Wsq.IO;

namespace BiomSharp.Imaging.Wsq
{
    internal class Segmenter
    {
        public EndianBinaryReader? Reader { get; private set; }
        public EndianBinaryWriter? Writer { get; private set; }
        private readonly List<BaseSegment> segments = new();

        public delegate void SegmentReadEventHandler(Segmenter segmenter, BaseSegment lastRead);
        public event SegmentReadEventHandler? SegmentRead;

        public delegate void SegmentWriteEventHandler(Segmenter segmenter, BaseSegment lastWrite);
        public event SegmentWriteEventHandler? SegmentWrite;

        public IEnumerable<BaseSegment> AllSegments() => segments;
        public IEnumerable<T> Segments<T>() where T : BaseSegment
        {
            if (segments != null)
            {
                return segments.OfType<T>();
            }
            throw new WsqCodecException("Segments have not been enumerated");
        }

        public Segmenter(EndianBinaryReader reader) => Reader = reader;

        public Segmenter(EndianBinaryWriter writer) => Writer = writer;

        private BaseSegment? First()
        {
            if (Reader != null)
            {
                _ = Reader.BaseStream.Seek(0L, SeekOrigin.Begin);
                return BaseSegment.CreateRead(Reader);
            }
            throw new WsqCodecException("Reader is null");
        }

        private BaseSegment? Next(BaseSegment current)
        {
            if (Reader != null)
            {
                if (!current.Deserialized)
                {
                    _ = Reader.BaseStream.Seek(
                        current.Position +
                        sizeof(Marker) +
                        (current is DataSegment segment ? segment.ContentSize : 0) +
                        (current is DataSegment ? sizeof(ushort) : 0),
                            SeekOrigin.Begin);
                }
                Marker? marker = Reader.NextMarker();
                return marker != null ? BaseSegment.CreateRead(Reader, marker.Value) : null;
            }
            throw new WsqCodecException("Reader is null");
        }

        private void AddReadSegment(BaseSegment segment)
        {
            segments.Add(segment);
            SegmentRead?.Invoke(this, segment);
        }

        public void AddWriteSegment(BaseSegment segment) => segments.Add(segment);

        public void EnumerateSegments()
        {
            BaseSegment? segment = First();
            if (segment != null)
            {
                while (segment.Marker != Marker.EOI)
                {
                    AddReadSegment(segment);
                    segment =
                        Next(segment)
                        ??
                        throw new WsqCodecException(
                            "Could not create valid WSQ segment");
                }
                AddReadSegment(segment);
            }
            else
            {
                throw new WsqCodecException(
                    "Failed to enumerate segments");
            }
        }

        public Segment.DhtTable DhtTableWithId(byte id)
        {
            foreach (Segment.Dht dht in segments.OfType<Segment.Dht>())
            {
                Segment.DhtTable? table = dht.Tables.FirstOrDefault(t => t.Th == id);
                if (table != null)
                {
                    return table;
                }
            }
            throw new WsqCodecException(
                $"Dht table with id = '{id}' not found");
        }

        public T First<T>() where T : BaseSegment => segments.OfType<T>().FirstOrDefault()
                ??
                throw new WsqCodecException(
                    $"Type '{typeof(T).Name}' segment not defined");

        public T Last<T>() where T : BaseSegment => segments.OfType<T>().LastOrDefault()
                ??
                throw new WsqCodecException(
                    $"Type '{typeof(T).Name}' segment not defined");

        public T? Prior<T>(BaseSegment current) where T : BaseSegment
        {
            int ci = segments.IndexOf(current);
            return segments.OfType<T>()
                .LastOrDefault(s => segments.IndexOf(s) < ci);
        }

        public int IndexOf<T>(T current) where T : BaseSegment => segments.OfType<T>().ToList().IndexOf(current);

        public void WriteAll()
        {
            if (Writer != null)
            {
                foreach (BaseSegment segment in segments)
                {
                    segment.Write(Writer);
                    SegmentWrite?.Invoke(this, segment);
                }
            }
            else
            {
                throw new WsqCodecException(
                    "Writer is null");
            }
        }
    }
}
