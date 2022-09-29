// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;

namespace BiomStudio.ViewModels
{
    [TypeConverter(typeof(ViewModelCollectionConverter))]
    public sealed class ViewModelCollection : Collection<object>, ICustomTypeDescriptor
    {
        [ReadOnly(true)]
        [Description("Name or filepath of entity")]
        public string Name { get; }

        private PropertyDescriptorCollection GetProperties(Attribute[]? attributes = null)
        {
            var pds = new PropertyDescriptorCollection(null);
            List<Attribute> attrs = attributes?.ToList() ?? new List<Attribute>();
            Attribute[] attrsArray = attrs.ToArray();
            _ = pds.Add(TypeDescriptor.CreateProperty(GetType(),
                "Name", typeof(string), attrsArray));
            for (int i = 0; i < Count; i++)
            {
                _ = pds.Add(new ViewModelCollectionDescriptor(this, i, attrsArray));
            }
            return pds;
        }

        public ViewModelCollection(string name) => Name = name;

        public override string ToString() => Name;

        public object? GetPropertyObject(string name)
            => this
            .FirstOrDefault(po => po.GetType()
            .GetCustomAttribute<DisplayNameAttribute>()?.DisplayName == name)
            ??
            throw new InvalidOperationException("View model DisplayNameAttribute is null");

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
