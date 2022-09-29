// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.ComponentModel;

namespace BiomStudio.ViewModels
{
    public class ViewModelDescriptionProvider : TypeDescriptionProvider
    {
        public ViewModelDescriptionProvider()
            : base(TypeDescriptor.GetProvider(typeof(IViewModel)))
        {

        }

        public override ICustomTypeDescriptor? GetTypeDescriptor(Type type, object? obj)
            => new ViewModelDescriptor(base.GetTypeDescriptor(type, (IViewModel?)obj));
    }
}
