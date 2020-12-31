using System;
using System.Collections.Generic;
using System.Text;

namespace VismaProject
{
    public class OrderItem
    {
        public int Id
        {
            get;
            set;
        }
        public DateTime DateTime
        {
            get;
            set;
        }
        public string MenuItems
        {
            get;
            set;
        }


        public override string ToString()
        {
            return $"{Id} {DateTime} {MenuItems}";
        }
    }
}
