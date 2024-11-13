using Newtonsoft.Json;
using System.IO;

namespace Application.Helper
{
    public static class Settings
    {
        public static int MonitoringFrequency { get; set; }
        public static string InputDirectoryPath { get; set; }
        public static string PluginDirectory { get; set; }
         static Settings()
        {
            var json = File.ReadAllText("D:\\Projects\\TestFileWatcher\\Presentation\\appsettings.json");
            dynamic config = JsonConvert.DeserializeObject(json);
            MonitoringFrequency = config.MonitoringFrequency ?? 10;
            InputDirectoryPath = config.InputDirectoryPath;
            PluginDirectory = config.PluginDirectory;
        }
    }
}
