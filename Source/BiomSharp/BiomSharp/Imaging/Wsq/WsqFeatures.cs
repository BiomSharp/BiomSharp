// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.ComponentModel;
using System.Globalization;

namespace BiomSharp.Imaging.Wsq
{
    internal abstract class WsqFeatures
    {
        protected Dictionary<string, string?> Items { get; set; }

        public abstract IEnumerable<string> Names { get; }
        public abstract bool IsValidName(string name);

        public virtual void AddOrUpdate(string name, object? value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new WsqCodecException(
                    "Feature name cannot be null or empty");
            }
            if (!IsValidName(name))
            {
                throw new WsqCodecException(string.Format(
                    "'{0}' is not a valid feature", name));
            }
            string? valueStr = null;
            if (value != null)
            {
                valueStr = value is string @string ?
                    @string
                    :
                    TypeDescriptor.GetConverter(value).ConvertToInvariantString(null, value);
            }
            if (Items.ContainsKey(name))
            {
                Items[name] = valueStr;
            }
            else
            {
                Items.Add(name, valueStr);
            }
        }

        public char KeyValueTerminator { get; protected set; }

        public char KeyValueSeparator { get; protected set; }

        protected WsqFeatures()
        {
            Items = new Dictionary<string, string?>();
            KeyValueTerminator = '\n';
            KeyValueSeparator = ' ';
        }

        protected void Combine(string text)
        {
            string[] fets = text.Split(KeyValueTerminator, StringSplitOptions.RemoveEmptyEntries);
            if (fets.Length > 0)
            {
                foreach (string fet in fets)
                {
                    string[] parts = fet.Split(KeyValueSeparator, StringSplitOptions.RemoveEmptyEntries);
                    string? name = null;
                    string value = string.Empty;
                    if (parts.Length > 0)
                    {
                        name = parts[0];
                    }

                    if (parts.Length > 1)
                    {
                        value = parts[1];
                    }

                    if (name != null)
                    {
                        AddOrUpdate(name, value);
                    }
                }
            }
        }
        public bool Delete(string name) => Items.Remove(name);

        public int Count => Items.Count;

        public T? GetValue<T>(string name) where T : struct
        {
            if (Items.ContainsKey(name))
            {
                string? value = GetValue(name);
                if (!string.IsNullOrEmpty(value))
                {
                    return (T?)TypeDescriptor.GetConverter(typeof(T))
                        .ConvertFrom(null, CultureInfo.InvariantCulture, value);
                }
            }
            return null;
        }

        public string? GetValue(string name)
        {
            if (!IsValidName(name))
            {
                throw new WsqCodecException(string.Format(
                    "'{0}' is not a valid feature", name));
            }
            return Items.ContainsKey(name) ? Items[name] : null;
        }
    }
}
