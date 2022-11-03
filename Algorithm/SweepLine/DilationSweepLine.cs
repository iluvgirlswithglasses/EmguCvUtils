using System;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCvUtils.Algorithm.SweepLine
{
    public class DilationSweepLine
    {
        int height, width;
        List<Event> ev;
        Segtree.UpdNQry1.Segtree st;

        public DilationSweepLine(int height, int width)
        {
            this.height = height;
            this.width = width;
            // scan from left to right
            ev = new List<Event>();
            st = new Segtree.UpdNQry1.Segtree(height);
        }

        public void AddPoint(int y, int x, int r)
        {
            int lo = Math.Max(0, y - r), hi = Math.Min(height, y + r + 1);
            ev.Add(
                new Event(Math.Max(0, x - r), lo, hi, Event.OPEN)
            );
            ev.Add(
                new Event(Math.Min(width, x + r + 1), lo, hi, Event.CLOSE)
            );
        }

        // All points added. The line sweeping begins.
        private void ready()
        {
            ev.Sort((a, b) => {
                // c# and java sort is kinda weird
                // can't write in a sorter way
                if (a.x.CompareTo(b.x) == 0)
                    return a.typ.CompareTo(b.typ);  // close typ is prioritized
                return a.x.CompareTo(b.x);
            });
        }

        // write the result to canvas
        // the canvas must be empty beforehand
        public void Execute(ref Image<Gray, byte> canvas)
        {
            ready();
            for (int i = 0; i < ev.Count - 1; i++)
            {
                Event cr = ev[i], nx = ev[i + 1];
                if (cr.typ == Event.OPEN)
                    st.upd(cr.lo, cr.hi, +1);
                else
                    st.upd(cr.lo, cr.hi, -1);
                // draw
                for (int y = 0; y < height; y++)
                    if (st.get(y) > 0)
                        for (int x = cr.x; x < nx.x; x++)
                            canvas[y, x] = new Gray(255);
            }
        }
    }
}
