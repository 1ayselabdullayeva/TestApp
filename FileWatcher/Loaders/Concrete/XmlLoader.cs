using FileWatcher.Loaders.Abstract;
using System.Collections.Generic;
using FileWatcher.Models.Common;
using System.Threading.Tasks;
using System.Xml.Linq;
using System;

namespace FileWatcher.Loaders.Concrete
{
    public class XmlLoader : ILoader
    {
        public async Task<IList<TradeData>> LoadDataAsync(string filePath)
        {
            var tradeDataList = new List<TradeData>();

            var xDoc = XDocument.Load(filePath);
            foreach (var element in xDoc.Descendants("value"))
            {
                var tradeData = new TradeData
                {
                    Date = DateTime.Parse(element.Attribute("date").Value),
                    Open = decimal.Parse(element.Attribute("open").Value),
                    High = decimal.Parse(element.Attribute("high").Value),
                    Low = decimal.Parse(element.Attribute("low").Value),
                    Close = decimal.Parse(element.Attribute("close").Value),
                    Volume = int.Parse(element.Attribute("volume").Value)
                };

                tradeDataList.Add(tradeData);
            }

            return tradeDataList;
        }
        public bool CanHandle(string filePath)
        {
            return filePath.EndsWith(".xml", StringComparison.OrdinalIgnoreCase);
        }
    }
}
