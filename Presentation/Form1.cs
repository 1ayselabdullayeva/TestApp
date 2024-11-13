using FileWatcher.Services.Concrete;
using FileWatcher.Services.Abstract;
using FileWatcher.Loaders.Abstract;
using FileWatcher.Loaders.Concrete;
using System.Collections.Generic;
using FileWatcher.Models.Common;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application.Helper;
using FileWatcher.Helper;
using System.Linq;
using System.IO;
using System;

namespace Presentation
{
    public partial class Form1 : Form
    {
        private readonly IFileService _fileService;
        private readonly IMonitorService _monitorService;
        private readonly PluginManager _pluginManager;
        public Form1()
        {
            InitializeComponent();
            _pluginManager = new PluginManager();
            var loaders1 = new List<ILoader> { new CsvLoader(), new XmlLoader(), new TxtLoader() };
            //_settings = new Settings();
            var loaders = _pluginManager.LoadPlugins(Settings.PluginDirectory);
            var allLoaders = new List<ILoader>(loaders1);
            allLoaders.AddRange(loaders);
            _fileService = new FileService(allLoaders);
            _monitorService = new MonitorService(
             _fileService, Settings.InputDirectoryPath, Settings.MonitoringFrequency
         );

            _monitorService.NewDataLoaded += OnNewDataLoaded;
            _monitorService.StartMonitoring();

            ShowExistingFiles(Settings.InputDirectoryPath);

        }

        private void OnNewDataLoaded(IEnumerable<TradeData> tradeData)
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke(new Action(() => UpdateListBox(tradeData)));
            }
            else
            {
                UpdateListBox(tradeData);
            }
        }

        private void UpdateListBox(IEnumerable<TradeData> tradeData)
        {
            foreach (var data in tradeData)
            {
                listBox1.Items.Add($"{data.Date.ToShortDateString()} - " +
                                   $"Open: {data.Open} - High: {data.High} - Low: {data.Low} - " +
                                   $"Close: {data.Close} - Volume: {data.Volume}");
            }
            listBox1.Items.Add("---------------------------------");
        }

        private async void ShowExistingFiles(string directoryPath)
        {
            try
            {
                var files = Directory.GetFiles(directoryPath);

                var loadTasks = files.Select(async file =>
                {
                    var tradeData = await _fileService.LoadDataFromFileAsync(file);
                    DisplayFileContent(file, tradeData);
                });

                await Task.WhenAll(loadTasks);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading existing files: {ex.Message}");
            }
        }

        private void DisplayFileContent(string filePath, IEnumerable<TradeData> tradeData)
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke(new Action(() => DisplayFileContentInternal(filePath, tradeData)));
            }
            else
            {
                DisplayFileContentInternal(filePath, tradeData);
            }
        }

        private void DisplayFileContentInternal(string filePath, IEnumerable<TradeData> tradeData)
        {
            
            if (tradeData != null && tradeData.Any())
            {
                listBox1.Items.Add($"File: {filePath}");
                foreach (var data in tradeData)
                {
                    listBox1.Items.Add($"{data.Date.ToShortDateString()} - " +
                                       $"Open: {data.Open} - High: {data.High} - Low: {data.Low} - " +
                                       $"Close: {data.Close} - Volume: {data.Volume}");
                }
                listBox1.Items.Add("---------------------------------");
            }
        }
    }
}
