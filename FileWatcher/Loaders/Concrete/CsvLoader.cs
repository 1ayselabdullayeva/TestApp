using FileWatcher.Loaders.Abstract;
using System.Collections.Generic;
using FileWatcher.Models.Common;
using System.Threading.Tasks;
using System.IO;
using System;

namespace FileWatcher.Loaders.Concrete
{
    public class CsvLoader : ILoader
    {
        public async Task<IList<TradeData>> LoadDataAsync(string filePath)
        {
            var tradeDataList = new List<TradeData>();

            using (var reader = new StreamReader(filePath))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var columns = line.Split(',');

                    var tradeData = new TradeData
                    {
                        Date = DateTime.Parse(columns[0]),
                        Open = decimal.Parse(columns[1]),
                        High = decimal.Parse(columns[2]),
                        Low = decimal.Parse(columns[3]),
                        Close = decimal.Parse(columns[4]),
                        Volume = int.Parse(columns[5])
                    };

                    tradeDataList.Add(tradeData);
                }
            }

            return tradeDataList;
        }
        public bool CanHandle(string filePath)
        {
            return filePath.EndsWith(".csv", StringComparison.OrdinalIgnoreCase);
        }
    }
}
