using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace eStrips
{
    public class Sector
    {
        public string Name { get; set; }
        public List<Point> Points { get; set; }
    }
}