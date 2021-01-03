using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VismaProject.Mappers;

namespace VismaProject.Services
{
    class MenuItemService
    {
        public List<MenuItem> ReadCSVFile(string location)
        {
            try
            {
                using (var reader = new StreamReader(location, Encoding.Default))
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CurrentCulture))
                {
                    csv.Configuration.RegisterClassMap<MenuItemMap>();
                    var records = csv.GetRecords<MenuItem>().ToList();
                    return records;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void WriteCSVFile(string path, List<MenuItem> MenuItem)
        {
            using (StreamWriter sw = new StreamWriter(path, false, new UTF8Encoding(true)))
            using (CsvWriter cw = new CsvWriter(sw, System.Globalization.CultureInfo.CurrentCulture))
            {
                cw.WriteHeader<MenuItem>();
                cw.NextRecord();
                foreach (MenuItem i in MenuItem)
                {
                    cw.WriteRecord<MenuItem>(i);
                    cw.NextRecord();
                }
            }
        }
    }
}
