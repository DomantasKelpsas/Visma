using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace VismaProject.Mappers
{
    public sealed class MenuItemMap : ClassMap<MenuItem>
    {
        public MenuItemMap()
        {
            Map(x => x.Id).Name("Id");
            Map(x => x.Name).Name("Name");
            Map(x => x.Products).Name("Products");
          

        }
    }
}
