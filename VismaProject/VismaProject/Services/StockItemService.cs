using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VismaProject.Mappers;

namespace VismaProject.Services
{
    class StockItemService
    {
        public List<StockItem> ReadCSVFile(string location)
        {
            try
            {
                using (var reader = new StreamReader(location, Encoding.Default))
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CurrentCulture))
                {
                    csv.Configuration.RegisterClassMap<StockItemMap>();
                    var records = csv.GetRecords<StockItem>().ToList();
                    return records;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void WriteCSVFile(string path, List<StockItem> StockItem)
        {
            using (StreamWriter sw = new StreamWriter(path, false, new UTF8Encoding(true)))
            using (CsvWriter cw = new CsvWriter(sw, System.Globalization.CultureInfo.CurrentCulture))
            {
                cw.WriteHeader<StockItem>();
                cw.NextRecord();
                foreach (StockItem sti in StockItem)
                {
                    cw.WriteRecord<StockItem>(sti);
                    cw.NextRecord();
                }
            }
        }
    }
}
