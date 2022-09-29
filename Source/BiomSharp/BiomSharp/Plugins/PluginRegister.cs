// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.Reflection;
using System.Text;
using BiomSharp.Extensions;

namespace BiomSharp.Plugins
{
    public static class PluginRegister
    {
        //public static IEnumerable<Type> GetPlugins(Assembly assembly, Func<Type, bool> isPlugin)
        //    => assembly.GetTypes().Where(t => !t.IsAbstract && isPlugin(t));

        public static IEnumerable<Type> GetPlugins(Assembly assembly, params Type[] interfaces)
            =>
            assembly.GetTypes()
            .Where(t => !t.IsAbstract &&
            interfaces
            .All(i => t.IsAssignableTo(i) || t.IsAssignableToGeneric(i)));

        public static IEnumerable<Type> GetPlugins(
            string? pluginFolderPath,
            bool includeSubFolders,
            bool setEnvironment,
            Func<Type, bool> isPlugin)
        {
            if (string.IsNullOrWhiteSpace(pluginFolderPath))
            {
                throw new ArgumentNullException(nameof(pluginFolderPath));
            }

            var rootFolder = new DirectoryInfo(Path.GetFullPath(pluginFolderPath));

            var pathVariable = new StringBuilder(
                Environment.GetEnvironmentVariable(
                    "PATH", EnvironmentVariableTarget.Process));

            var paths = new Dictionary<string, string>();
            var plugins = new List<Type>();

            foreach (
                string path in pathVariable.ToString().Split(new char[] { ';' },
                StringSplitOptions.RemoveEmptyEntries))
            {
                if (!paths.ContainsKey(path.ToLower()))
                {
                    paths.Add(path.ToLower(), path);
                }
            }
            if (rootFolder.Exists)
            {
                foreach (
                    FileInfo file
                    in
                    rootFolder.GetFiles("*.dll",
                        includeSubFolders
                        ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                {
                    try
                    {
                        var asm = Assembly.LoadFrom(file.FullName);
                        foreach (Type asmType in asm.GetTypes())
                        {
                            if (!asmType.IsAbstract && isPlugin(asmType))
                            {
                                plugins.Add(asmType);
                                string? asmFolderPath = Path.GetDirectoryName(asm.Location);
                                if (asmFolderPath == null)
                                {
                                    continue;
                                }
                                var asmFolder = new DirectoryInfo(asmFolderPath);
                                if (asmFolder != null && asmFolder.Exists)
                                {
                                    while (
                                        asmFolder != null
                                        &&
                                        asmFolder.FullName != rootFolder.FullName)
                                    {
                                        if (!paths.ContainsKey(
                                                asmFolder.FullName.ToLower()))
                                        {
                                            paths.Add(
                                                asmFolder.FullName.ToLower(),
                                                asmFolder.FullName);
                                            _ = pathVariable.AppendFormat(
                                                ";{0}", asmFolder.FullName);
                                        }
                                        asmFolder = asmFolder.Parent;
                                    }
                                }
                            }
                        }
                    }
                    catch {/*ignore*/}
                }
            }
            if (setEnvironment)
            {
                Environment.SetEnvironmentVariable(
                    "PATH",
                    pathVariable.ToString(),
                    EnvironmentVariableTarget.Process);
            }
            return plugins;
        }
    }
}
