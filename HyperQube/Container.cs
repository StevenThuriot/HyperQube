#region License

//  Copyright 2014 Steven Thuriot
//   
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//  
//  http://www.apache.org/licenses/LICENSE-2.0
//  
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

#endregion

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