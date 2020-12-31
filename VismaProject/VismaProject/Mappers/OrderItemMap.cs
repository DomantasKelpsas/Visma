using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace VismaProject.Mappers
{
    public sealed class OrderItemMap : ClassMap<OrderItem>
    {
        public OrderItemMap()
        {
            Map(x => x.Id).Name("Id");
            Map(x => x.DateTime).Name("DateTime");
            Map(x => x.MenuItems).Name("MenuItems");


        }
    }
}