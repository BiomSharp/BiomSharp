// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.ComponentModel;
using System.Reflection;

namespace BiomStudio.ViewModels
{
    public sealed class ViewModelCollectionDescriptor : PropertyDescriptor
    {
        private readonly ViewModelCollection collection;

        private readonly int index;

        public ViewModelCollectionDescriptor(
            ViewModelCollection collection, int index, Attribute[]? attributes = null)
            :
            base(string.Format("#{0}", index), attributes)
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

        public override string DisplayName
            => collection[index].GetType()
            .GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
            ??
            throw new InvalidOperationException("View model DisplayNameAttribute is null");

        public override string Description
            => collection[index].GetType()
            .GetCustomAttribute<DescriptionAttribute>()?.Description ?? "";

        public override object? GetValue(object? component) => collection[index];

        public override bool IsReadOnly
            => collection[index].GetType().GetCustomAttribute<ReadOnlyAttribute>()
            ?.IsReadOnly ?? false;

        public override string Name
            => collection[index].GetType().GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
            ??
            throw new InvalidOperationException("View model DisplayNameAttribute is null");

        public override Type PropertyType => collection[index].GetType();

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component) => true;

        public override void SetValue(object? component, object? value)
        {
        }
    }
}
