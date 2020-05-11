using System;
using System.Collections.Generic;
using System.Text;

namespace EscapeFromTheWoods
{
    public class Forest
    {
        public Forest(int forestId, int xmin, int xmax, int ymin, int ymax)
        {
            this.forestId = forestId;
            this.xmin = xmin;
            this.xmax = xmax;
            this.ymin = ymin;
            this.ymax = ymax;

            monkeys = new List<Monkey>();
            trees = new List<Tree>();
        }

        public int forestId { get; set; }
        public int xmin { get; set; }
        public int xmax { get; set; }
        public int ymin { get; set; }
        public int ymax { get; set; }
        public List<Monkey> monkeys { get; set; }
        public List<Tree> trees { get; set; }

    }
}
