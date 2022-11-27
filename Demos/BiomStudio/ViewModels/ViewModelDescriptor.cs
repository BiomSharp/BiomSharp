// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using System.ComponentModel;

namespace BiomStudio.ViewModels
{
    public class ViewModelDescriptor : CustomTypeDescriptor
    {
        private readonly ICustomTypeDescriptor? customTypeDescriptor;

        public ViewModelDescriptor(ICustomTypeDescriptor? customTypeDescriptor)
            : base(customTypeDescriptor)
        => this.customTypeDescriptor = customTypeDescriptor;

        public override PropertyDescriptorCollection GetProperties()
            => GetProperties(Array.Empty<Attribute>());

        public override PropertyDescriptorCollection GetProperties(Attribute[]? attributes)
            => new(
            base.GetProperties()
            .Cast<PropertyDescriptor>()
            .Select(pd =>
                new ViewModelPropertyDescriptor(pd,
                    customTypeDescriptor?.GetPropertyOwner(pd) as IViewModel))
            .ToArray());
    }
}
