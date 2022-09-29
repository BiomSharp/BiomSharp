// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.ComponentModel;
using System.Reflection;

namespace BiomStudio.Extensions
{
    public static class PropertyDescriptorExtensions
    {
        public static void SetReadOnlyAttribute(this PropertyDescriptor pd, bool value)
        {
            var attributes = pd.Attributes.Cast<Attribute>()
                .Where(attr => attr is not ReadOnlyAttribute).ToList();
            attributes.Add(new ReadOnlyAttribute(value));
            typeof(MemberDescriptor)?.GetProperty("AttributeArray",
                BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(pd, attributes.ToArray());
        }

        public static bool GetReadOnlyAttribute(this PropertyDescriptor pd)
        {
            Attribute? attr = pd.Attributes.Cast<Attribute>().FirstOrDefault(attr => attr is ReadOnlyAttribute);
            return attr != null && ((attr as ReadOnlyAttribute)?.IsReadOnly ?? false);
        }
    }
}
