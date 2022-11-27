// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

namespace BiomSharp.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsAssignableToGeneric(this Type sourceType, Type targetType)
        {
            bool IsAssignable(Type comperand)
            {
                if (comperand.IsAssignableTo(targetType))
                {
                    return true;
                }

                if (comperand.IsGenericType && targetType.IsGenericType &&
                    comperand.GetGenericTypeDefinition() == targetType.GetGenericTypeDefinition())
                {
                    for (int i = 0; i < targetType.GenericTypeArguments.Length; i++)
                    {
                        Type comperandArgument = comperand.GenericTypeArguments[i];
                        Type targetArgument = targetType.GenericTypeArguments[i];

                        if (!comperandArgument.IsGenericTypeParameter &&
                            !targetArgument.IsGenericTypeParameter &&
                            !comperandArgument.IsAssignableTo(targetArgument))
                        {
                            return false;
                        }
                    }

                    return true;
                }

                return false;
            }

            if (IsAssignable(sourceType))
            {
                return true;
            }

            if (targetType.IsInterface && sourceType.GetInterfaces().Any(IsAssignable))
            {
                return true;
            }

            return false;
        }
    }
}
