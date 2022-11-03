using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// used in sweepline algorithms
namespace EmguCvUtils.Algorithm.Segtree.UpdNQry1
{
    public class Segtree
    {
        int n;
        int[] f;

        public Segtree(int _n)
        {
            n = _n;
            f = new int[n<<1];
        }
    }
}
