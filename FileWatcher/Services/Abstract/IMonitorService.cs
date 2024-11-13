using System;
using System.Collections.Generic;
using FileWatcher.Models.Common;

namespace FileWatcher.Services.Abstract
{
    public interface IMonitorService
    {
        void StartMonitoring();
        void StopMonitoring();
        void OnNewDataLoaded(IEnumerable<TradeData> data);
        event Action<IEnumerable<TradeData>> NewDataLoaded;
    }
}
