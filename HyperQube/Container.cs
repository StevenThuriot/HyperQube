using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HyperQube
{
    internal static class Container
    {
        private static CompositionContainer _container;

        internal static void RegisterInstance<T>(this T instance)
        {
            _container.ComposeExportedValue(instance);
        }
        
        public static CompositionContainer CreateCompositionContainer()
        {
            if (_container != null) return _container;

            var executingAssembly = Assembly.GetExecutingAssembly();
            var assemblyCatalog = new AssemblyCatalog(executingAssembly);

            var aggregateCatalog = new AggregateCatalog(assemblyCatalog);

            var directory = Path.GetDirectoryName(executingAssembly.Location);

            var catalogs = GetDirectoriesRecursive(directory).Select(dir => new DirectoryCatalog(dir));

            foreach (var catalog in catalogs)
                aggregateCatalog.Catalogs.Add(catalog);

            return _container = new CompositionContainer(aggregateCatalog);
        }

        private static IEnumerable<string> GetDirectoriesRecursive(string path)
        {
            return Directory.GetDirectories(path)
                            .Select(GetDirectoriesRecursive)
                            .SelectMany(x => x)
                            .Union(new[] {path});
        }
    }
}