using System;
using System.Collections.Generic;
using System.Text;

namespace EscapeFromTheWoods
{
    public class Monkey : IComparable
    {
        public Monkey(int iD, string name)
        {
            ID = iD;
            this.name = name;
        }

        public int ID { get; set; }
        public string name { get; set; }

        public int CompareTo(object obj)
        {
            Monkey m = (Monkey)obj;
            return String.Compare(this.name, m.name);
        }
    }
}
