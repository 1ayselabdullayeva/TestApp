using System.Collections.Generic;
using FileWatcher.Models.Common;
using System.Threading.Tasks;

namespace FileWatcher.Services.Abstract
{
    public interface IFileService
    {
        Task<IList<TradeData>> LoadDataFromFileAsync(string filePath);
    }
}
