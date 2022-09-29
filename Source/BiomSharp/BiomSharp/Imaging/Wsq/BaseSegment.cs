// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.Reflection;
using BiomSharp.Imaging.Wsq.IO;

namespace BiomSharp.Imaging.Wsq
{
    internal abstract class BaseSegment
    {
        protected static Dictionary<Marker, Type> Types { get; private set; }
        public abstract Marker Marker { get; }
        public long Position { get; private set; }
        public bool Deserialized { get; protected set; }

        static BaseSegment()
        {
            Types = new Dictionary<Marker, Type>();
            foreach (Type? type in typeof(BaseSegment)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(BaseSegment)) && !t.IsAbstract))
            {
                ConstructorInfo? ctor = type.GetConstructor(
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                    null, Type.EmptyTypes, null);
                if (ctor != null)
                {
                    var instance = (BaseSegment)ctor.Invoke(null);
                    PropertyInfo? property = type.GetProperty("Marker");
                    if (property != null && property.PropertyType == typeof(Marker))
                    {
                        object? marker = property.GetValue(instance, null);
                        if (marker != null)
                        {
                            if (Types.ContainsKey((Marker)marker))
                            {
                                throw new WsqCodecException(
                                    $"Type with marker '{marker}' already added");
                            }
                            Types.Add((Marker)marker, type);
                        }
                        else
                        {
                            throw new WsqCodecException(
                                $"Could not find marker object for '{type.Name};");
                        }
                    }
                    else
                    {
                        throw new WsqCodecException(
                            $"Could not find marker property for '{type.Name};");
                    }
                }
                else
                {
                    throw new WsqCodecException(
                        $"Could not find .ctor for '{type.Name};");
                }
            }
        }

        protected BaseSegment() { }

        protected virtual void Read(EndianBinaryReader reader, Marker marker)
        {
            Position = reader.BaseStream.Position - sizeof(Marker);
            if (marker != Marker)
            {
                throw new WsqCodecException(string.Format(
                    "Expected marker '{0}' - read '{1}'", Marker, marker));
            }
        }

        public virtual void Write(EndianBinaryWriter writer) => writer.Write((ushort)Marker);

        public static BaseSegment? CreateRead(EndianBinaryReader reader, Marker marker)
        {
            BaseSegment? segment = CreateInstance(marker);
            if (segment != null)
            {
                segment.Read(reader, marker);
            }
            return segment;
        }

        public static BaseSegment? CreateRead(EndianBinaryReader reader)
        {
            Marker? marker = reader.NextMarker();
            return marker != null ? CreateRead(reader, marker.Value) : null;
        }

        public static BaseSegment CreateInstance(Marker marker)
        {
            if (Types.ContainsKey(marker))
            {
                ConstructorInfo? ctor = Types[marker].GetConstructor(
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                        null, Type.EmptyTypes, null);
                if (ctor != null)
                {
                    return (BaseSegment)ctor.Invoke(null);
                }
                else
                {
                    throw new WsqCodecException(
                        $"Could find .ctor for marker '{marker}'");
                }
            }
            throw new WsqCodecException(
                $"Could not create segment for '{marker.GetType().Name}'");
        }
    }
}
