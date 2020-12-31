using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VismaProject.Mappers;

namespace VismaProject.Services
{
    class OrderItemService
    {
        public List<OrderItem> ReadCSVFile(string location)
        {
            try
            {
                using (var reader = new StreamReader(location, Encoding.Default))
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CurrentCulture))
                {
                    csv.Configuration.RegisterClassMap<OrderItemMap>();
                    var records = csv.GetRecords<OrderItem>().ToList();
                    return records;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void WriteCSVFile(string path, List<OrderItem> OrderItem)
        {
            using (StreamWriter sw = new StreamWriter(path, false, new UTF8Encoding(true)))
            using (CsvWriter cw = new CsvWriter(sw, System.Globalization.CultureInfo.CurrentCulture))
            {
                cw.WriteHeader<OrderItem>();
                cw.NextRecord();
                foreach (OrderItem sti in OrderItem)
                {
                    cw.WriteRecord<OrderItem>(sti);
                    cw.NextRecord();
                }
            }
        }
    }
}
