using FileWatcher.Loaders.Abstract;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System;

namespace FileWatcher.Helper
{
    public  class PluginManager
    {
        public  List<ILoader> LoadPlugins(string pluginsDirectory)
        {
            var loaders = new List<ILoader>();

            if (!Directory.Exists(pluginsDirectory))
            {
                Console.WriteLine("Plugin directory does not exist.");
                return loaders;
            }

            var pluginFiles = Directory.GetFiles(pluginsDirectory, "*.dll");

            foreach (var pluginFile in pluginFiles)
            {
                try
                {
                    Assembly pluginAssembly = Assembly.LoadFrom(pluginFile);
                    var types = pluginAssembly.GetTypes();

                    foreach (var type in types)
                    {
                        if (typeof(ILoader).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                        {
                            var loader = (ILoader)Activator.CreateInstance(type);
                            loaders.Add(loader);
                            Console.WriteLine($"Loaded plugin: {type.Name} from {pluginFile}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading plugin {pluginFile}: {ex.Message}");
                }
            }

            return loaders;
        }
    }

}
