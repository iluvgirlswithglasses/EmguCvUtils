using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmguCvUtils.Algorithm.SweepLine
{
    internal class Event
    {
        public static int CLOSE = 0, OPEN = 1;

        public int x;
        public int lo, hi;
        public int typ;

        public Event(int x, int lo, int hi, int typ)
        {
            this.x = x;
            this.lo = lo;
            this.hi = hi;
            this.typ = typ;
        }
    }
}
