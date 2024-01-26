using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStrips
{
    public class Logging
    {
        //  LOGGING
        public static void Log(string str)
        {
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss:ff") + " | " + str);
        }
    }
}
