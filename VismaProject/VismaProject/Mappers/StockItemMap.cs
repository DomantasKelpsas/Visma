using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace VismaProject.Mappers
{
    public sealed class StockItemMap : ClassMap<StockItem>
    {
        public StockItemMap()
        {
            Map(x => x.Id).Name("Id");
            Map(x => x.Name).Name("Name");
            Map(x => x.PortionCount).Name("PortionCount");
            Map(x => x.Unit).Name("Unit");
            Map(x => x.PortionSize).Name("PortionSize");
           
        }
    }
}
