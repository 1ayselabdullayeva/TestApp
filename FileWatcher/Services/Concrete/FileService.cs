using FileWatcher.Loaders.Abstract;
using FileWatcher.Services.Abstract;
using System.Collections.Generic;
using FileWatcher.Models.Common;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace FileWatcher.Services.Concrete
{
    public class FileService : IFileService
    {
        private readonly IList<ILoader> _fileLoaders;
        public FileService(IList<ILoader> fileLoaders)
        {
            _fileLoaders = fileLoaders;
        }
        public async Task<IList<TradeData>> LoadDataFromFileAsync(string filePath)
        {
            ILoader loader = _fileLoaders.FirstOrDefault(l => l.CanHandle(filePath));
            if (loader == null)
            {
                throw new InvalidOperationException("No loader found for this file type.");
            }

            return await loader.LoadDataAsync(filePath);
        }
    }
}
