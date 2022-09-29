// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using BiomSharp.Plugins;

namespace BiomSharp.Factories
{
    public sealed class PluginFactory<TId, T>
        where T : IPlugin<TId>
        where TId : notnull
    {
        private readonly Dictionary<TId, Type> items = new();

        public IEnumerable<Type> Items => items.Values;

        public Type InterfaceType => typeof(T);

        public PluginFactory(string? pluginFolderPath,
            bool includeSubFolders,
            bool setEnvironment,
            Func<Type, bool> isPlugin)
        {
            IEnumerable<Type> plugins = PluginRegister.GetPlugins(
                pluginFolderPath, includeSubFolders, setEnvironment, isPlugin);
            if (plugins != null)
            {
                plugins.ToList().ForEach(t => Add(t));
            }
        }

        private void Add(IEnumerable<Type> implTypes)
            => implTypes.ToList().ForEach(i => Add(i));

        private void Add(Type implType)
        {
            if (implType.IsClass
                &&
                !implType.IsAbstract
                &&
                implType.GetInterfaces().Any(t => t == typeof(T)))
            {
                if (Activator.CreateInstance(implType) is IPlugin<TId> plugin)
                {
                    if (plugin.Id != null && !items.ContainsKey(plugin.Id))
                    {
                        items.Add(plugin.Id, implType);
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            $"{implType.Name} implementer already added");
                    }
                }
                else
                {
                    throw new InvalidOperationException(
                        $"{implType.Name} IPlugin interface invalid");
                }
            }
            else
            {
                throw new InvalidOperationException(
                    $"{implType.Name} implementer is invalid: check implementer" +
                    " is class, non-abstract, and is assignable from " +
                    nameof(T));
            }
        }

        public T Create(TId id)
        {
            if (items.ContainsKey(id))
            {
                object? instance =
                    Activator
                    .CreateInstance(items[id]);
                if (instance != null)
                {
                    return (T)instance;
                }
                throw new InvalidOperationException(
                $"{id} implementer could not be instantiated - " +
                    "check implementer has default constructor");
            }
            else
            {
                throw new InvalidOperationException(
                    $"{id} implementer not found");
            }
        }
    }
}
