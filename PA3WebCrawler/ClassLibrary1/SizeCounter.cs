using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public static class SizeCounter
    {
        public static int SizeQueue { get; set; }

        public static int SizeIndex { get; set; }

        public static int NumCrawled { get; set; }

        public static void Clear()
        {
            SizeQueue = 0;
            SizeIndex = 0;
            NumCrawled = 0;
        }
    }
}
