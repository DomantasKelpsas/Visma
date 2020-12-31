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
    class ItemService<I>
    {

        public List<I> ReadCSVFile(string location)
        {
            try
            {
                using (var reader = new StreamReader(location, Encoding.Default))
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CurrentCulture))
                {
                    csv.Configuration.RegisterClassMap<ClassMap<I>>();
                    var records = csv.GetRecords<I>().ToList();
                    return records;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void WriteCSVFile(string path, List<I> MenuItem)
        {
            using (StreamWriter sw = new StreamWriter(path, false, new UTF8Encoding(true)))
            using (CsvWriter cw = new CsvWriter(sw, System.Globalization.CultureInfo.CurrentCulture))
            {
                cw.WriteHeader<I>();
                cw.NextRecord();
                foreach (I sti in MenuItem)
                {
                    cw.WriteRecord<I>(sti);
                    cw.NextRecord();
                }
            }
        }
    }
}
