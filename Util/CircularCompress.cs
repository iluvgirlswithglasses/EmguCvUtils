using System;
using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCvUtils.Util
{
    static public class CircularCompress
    {
        static public void Apply(ref Image<Bgr, byte> presenter)
        {
            int cy = presenter.Height >> 1, cx = presenter.Width >> 1;
            int r = Math.Min(cy, cx);
            r *= r;
            //
            for (int y = 0; y < presenter.Height; y++) for (int x = 0; x < presenter.Width; x++)
                {
                    if (sqr(cy - y) + sqr(cx - x) < r)
                    {
                        // half the circular pixel in this row
                        int half = cx - x;
                        // assume the diameter is odd
                        int cnt = half << 1 | 1;
                        // circular pixel [0] = avg(row[0:range])
                        double range = (double)presenter.Width / cnt;
                        // Console.WriteLine(String.Format("y={0} cnt={1} range={2}", y, cnt, range));
                        //
                        Bgr[] v = new Bgr[cnt];
                        for (int i = 0; i < cnt; i++)
                            v[i] = avg(ref presenter, y, (int)Math.Round(range * i), (int)Math.Round(range * (i + 1)));
                        for (int i = 0; i < presenter.Width; i++)
                            presenter[y, i] = new Bgr(0, 0, 0);
                        for (int i = 0; i < cnt; i++)
                            presenter[y, cx - half + i] = v[i];
                        break;
                    }
                    else if (x == presenter.Width - 1)
                        for (int i = 0; i < presenter.Width; i++)
                            presenter[y, i] = new Bgr(0, 0, 0);
                }
        }

        static private Bgr avg(ref Image<Bgr, byte> presenter, int y, int le, int ri)
        {
            int n = ri - le;
            double b = 0, g = 0, r = 0;
            if (n == 0)
                return new Bgr(0, 0, 0);
            for (int x = le; x < ri; x++)
            {
                Bgr v = presenter[y, x];
                b += v.Blue;
                g += v.Green;
                r += v.Red;
            }
            return new Bgr(
                b / n,
                g / n,
                r / n
            );
        }

        static private int sqr(int x)
        {
            return x * x;
        }
    }
}
