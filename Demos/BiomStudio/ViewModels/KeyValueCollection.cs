// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BiomStudio.ViewModels
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public sealed class KeyValueCollection<TValue> : Collection<KeyValuePair<string, TValue>>, ICustomTypeDescriptor
        where TValue : struct
    {
        private readonly bool isReadOnly;

        private PropertyDescriptorCollection GetProperties(Attribute[]? attributes = null)
        {
            var pds = new PropertyDescriptorCollection(null);
            List<Attribute> attrs = attributes?.ToList() ?? new List<Attribute>();
            attrs.Add(new ReadOnlyAttribute(isReadOnly));
            Attribute[] attrsArray = attrs.ToArray();
            for (int i = 0; i < Count; i++)
            {
                _ = pds.Add(new KeyValueCollectionDescriptor<TValue>(this, i, attrsArray));
            }
            return pds;
        }

        public KeyValueCollection(ICollection<KeyValuePair<string, TValue>> col, bool isReadOnly)
        {
            this.isReadOnly = isReadOnly;
            col.ToList().ForEach(kvp => Add(new KeyValuePair<string, TValue>(kvp.Key, kvp.Value)));
        }

        public override string ToString() => "";

        public object? GetPropertyObject(string name) => this.FirstOrDefault(po => po.Key == name).Value;

        #region ICustomTypeDescriptor implementation.

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
            => TypeDescriptor.GetAttributes(this, true);

        string? ICustomTypeDescriptor.GetClassName() => TypeDescriptor.GetClassName(this, true);

        string? ICustomTypeDescriptor.GetComponentName()
            => TypeDescriptor.GetComponentName(this, true);

        TypeConverter ICustomTypeDescriptor.GetConverter() => TypeDescriptor.GetConverter(this, true);

        EventDescriptor? ICustomTypeDescriptor.GetDefaultEvent() => TypeDescriptor.GetDefaultEvent(this, true);

        PropertyDescriptor? ICustomTypeDescriptor.GetDefaultProperty()
            => TypeDescriptor.GetDefaultProperty(this, true);

        object? ICustomTypeDescriptor.GetEditor(Type editorBaseType)
            => TypeDescriptor.GetEditor(this, editorBaseType, true);

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[]? attributes)
            => TypeDescriptor.GetEvents(this, attributes, true);

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
            => TypeDescriptor.GetEvents(this, true);

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[]? attributes)
            => GetProperties(attributes);

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
            => GetProperties();

        object? ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor? pd) => this;

        #endregion ICustomTypeDescriptor implementation.
    }
}
