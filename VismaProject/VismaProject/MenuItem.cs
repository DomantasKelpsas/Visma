using System;
using System.Collections.Generic;
using System.Text;

namespace VismaProject
{
    public class MenuItem
    {
        public int Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string Products
        {
            get;
            set;
        }
       

        public override string ToString()
        {
            return $"{Id} {Name} {Products}";
        }
    }
}
