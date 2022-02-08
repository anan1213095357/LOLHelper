using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLHelper
{
    public enum Position
    { 
        上单,
        打野,
        中单,
        射手,
        辅助
    }

    public class Hero
    {
        public string Name { get; set; }
        public Talent Talent { get; set; }
        public int Order { get; set; }
        public Position Position { get; set; }
        public bool Available { get; set; } = true;
    }
}
