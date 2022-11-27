// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using System.ComponentModel;
using System.Reflection;

namespace BiomStudio.ViewModels
{
    public sealed class KeyValueCollectionDescriptor<TValue> : PropertyDescriptor
        where TValue : struct
    {
        private readonly KeyValueCollection<TValue> collection;

        private readonly int index;

        public KeyValueCollectionDescriptor(
            KeyValueCollection<TValue> collection, int index, Attribute[]? attributes = null)
            :
            base(string.Format(collection[index].Key, index), attributes)
        {
            this.collection = collection;
            this.index = index;
        }

        public override AttributeCollection Attributes
        {
            get
            {
                var attrList = ComponentType.GetCustomAttributes().ToList();
                attrList.AddRange(AttributeArray!);
                return new AttributeCollection(attrList.ToArray());
            }
        }

        public override bool CanResetValue(object component) => true;

        public override Type ComponentType => collection.GetType();

        public override string DisplayName => collection[index].Key;

        public override string Description => collection[index].Key;

        public override object? GetValue(object? component) => collection[index].Value;

        public override bool IsReadOnly
            => collection[index].GetType().GetCustomAttribute<ReadOnlyAttribute>()
            ?.IsReadOnly ?? false;

        public override string Name => collection[index].Key;

        public override Type PropertyType => typeof(TValue);

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component) => true;

        public override void SetValue(object? component, object? value)
        {
        }
    }
}
