using FileWatcher.Services.Concrete;
using FileWatcher.Services.Abstract;
using System.Collections.Generic;
using FileWatcher.Models.Common;
using System.Threading.Tasks;
using System.Linq;
using FileWatcher;
using System.IO;
using System;

public class MonitorService : IMonitorService
{
    private FileSystemWatcher _fileWatcher;
    private readonly IFileService _fileRepository;
    private readonly string _directoryPath;
    private int _monitoringIntervalInMilliseconds;
    private readonly HashSet<string> _processingFiles = new HashSet<string>();
    public event Action<IEnumerable<TradeData>> NewDataLoaded;

    public MonitorService(IFileService fileRepository, string directoryPath, int monitoringIntervalInMilliseconds)
    {
        _fileRepository = fileRepository;
        _directoryPath = directoryPath;
        _monitoringIntervalInMilliseconds = monitoringIntervalInMilliseconds;
    }

    public void StartMonitoring()
    {
        _fileWatcher = new FileSystemWatcher(_directoryPath)
        {
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
            EnableRaisingEvents = true
        };

        _fileWatcher.Created += OnNewFileCreated;
    }

    private void OnNewFileCreated(object sender, FileSystemEventArgs e)
    {
        if (_processingFiles.Contains(e.FullPath))
            return;

        _processingFiles.Add(e.FullPath);

        Task.Run(async () =>
        {
            await Task.Delay(_monitoringIntervalInMilliseconds); 

            try
            {
                var tradeData = await LoadNewFile(e.FullPath);
                OnNewDataLoaded(tradeData);  
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file: {ex.Message}");
            }
            finally
            {
                _processingFiles.Remove(e.FullPath);
            }
        });
    }

    private async Task<IEnumerable<TradeData>> LoadNewFile(string filePath)
    {
        try
        {
            return await _fileRepository.LoadDataFromFileAsync(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
            return Enumerable.Empty<TradeData>();
        }
    }

    public void OnNewDataLoaded(IEnumerable<TradeData> data)
    {
        NewDataLoaded?.Invoke(data);
    }

    public void StopMonitoring()
    {
        _fileWatcher?.Dispose();
    }
}
