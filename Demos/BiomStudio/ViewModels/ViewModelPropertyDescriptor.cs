// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.ComponentModel;

namespace BiomStudio.ViewModels
{
    public class ViewModelPropertyDescriptor : PropertyDescriptor
    {
        private readonly PropertyDescriptor pd;
        private readonly IViewModel? owner;
        private readonly bool readOnly;

        public ViewModelPropertyDescriptor(PropertyDescriptor pd, IViewModel? owner)
            : base(pd)
        {
            this.pd = pd;
            this.owner = owner;
            Attribute? attr = pd.Attributes
                .Cast<Attribute>().FirstOrDefault(attr => attr is ReadOnlyAttribute);
            readOnly = attr is not null and ReadOnlyAttribute readOnlyAttr && readOnlyAttr.IsReadOnly;
        }

        public override bool CanResetValue(object component) => pd.CanResetValue(component);

        public override object? GetValue(object? component) => pd.GetValue(component);

        public override void ResetValue(object component) => pd.ResetValue(component);

        public override void SetValue(object? component, object? value) => pd.SetValue(component, value);

        public override bool ShouldSerializeValue(object component) => pd.ShouldSerializeValue(component);

        public override AttributeCollection Attributes => pd.Attributes;

        public override Type ComponentType => pd.ComponentType;

        public override bool IsReadOnly => readOnly || (owner?.ReadOnly ?? false);

        public override Type PropertyType => pd.PropertyType;
    }
}
