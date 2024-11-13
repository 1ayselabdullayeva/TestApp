using System.Collections.Generic;
using FileWatcher.Models.Common;
using System.Threading.Tasks;

namespace FileWatcher.Loaders.Abstract
{
    public interface ILoader
    {
        Task<IList<TradeData>> LoadDataAsync(string filePath);
        bool CanHandle(string filePath);
    }
}
