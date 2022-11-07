using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmguCvUtils.Type
{
    public class Pair<T, U>
    {
        public T St { get; set; }
        public U Nd { get; set; }

        public Pair(T st, U nd)
        {
            St = st;
            Nd = nd;
        }

        public override string ToString()
        {
            return String.Format("({0}, {1})", St, Nd);
        }
    }
}
