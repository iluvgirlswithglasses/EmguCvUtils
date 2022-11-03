using System;

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

        public void upd(int l, int r, int v)
        {
            for (l += n, r += n; l < r; l >>= 1, r >>= 1)
            {
                if (Convert.ToBoolean(l & 1)) f[l++] += v;
                if (Convert.ToBoolean(r & 1)) f[--r] += v;
            }
        }

        public int get(int i)
        {
            int res = 0;
            for (i += n; i > 0; i >>= 1)
                res += f[i];
            return res;
        }
    }
}
