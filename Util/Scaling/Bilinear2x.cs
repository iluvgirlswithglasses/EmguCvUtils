using System;
using System.Security.Cryptography;
using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCvUtils.Util.Scaling
{
    public static class Bilinear2x
    {
        private static double[] kernel = new double[4]{ 1.414, 0.632, 0.471, 0.632 };
        private static double sweight = 3.149;

        public static Image<Bgr, byte> Create(ref Image<Bgr, byte> src)
        {
            var res = new Image<Bgr, byte>(src.Width << 1, src.Height << 1);
            // process the corners
            res[0, 0] = src[0, 0];
            res[res.Height - 1, 0] = src[src.Height - 1, 0];
            res[0, res.Width - 1] = src[0, src.Width - 1];
            res[res.Height - 1, res.Width - 1] = src[src.Height - 1, src.Width - 1];
            // process the edges
            for (int y = 0; y < src.Height - 1; y++)
            {
                res[1 + (y << 1), 0] = calc(src[y, 0], src[y+1, 0], kernel[0], kernel[1]);
                res[2 + (y << 1), 0] = calc(src[y, 0], src[y+1, 0], kernel[0], kernel[1]);

                int w = src.Width - 1;
                res[1 + (y << 1), res.Width - 1] = calc(src[y, w], src[y + 1, w], kernel[0], kernel[1]);
                res[2 + (y << 1), res.Width - 1] = calc(src[y, w], src[y + 1, w], kernel[1], kernel[0]);
            }
            for (int x = 0; x < src.Width - 1; x++)
            {
                res[0, 1 + (x << 1)] = calc(src[0, x], src[0, x+1], kernel[0], kernel[1]);
                res[0, 2 + (x << 1)] = calc(src[0, x], src[0, x+1], kernel[1], kernel[0]);

                int h = src.Height - 1;
                res[res.Height - 1, 1 + (x << 1)] = calc(src[h, x], src[h, x + 1], kernel[0], kernel[1]);
                res[res.Height - 1, 2 + (x << 1)] = calc(src[h, x], src[h, x + 1], kernel[1], kernel[0]);
            }
            // anything else
            for (int y = 0; y < src.Height - 1; y++)
                for (int x = 0; x < src.Width - 1; x++)
                    writeKernel(y, x, ref src, ref res);
            //
            Console.WriteLine("Image Size after scaling: {0}x{1}", res.Width, res.Height);
            return res;
        }

        private static void writeKernel(int y, int x, ref Image<Bgr, byte> src, ref Image<Bgr, byte> res)
        {
            Bgr[] pts = new Bgr[4] { src[y, x], src[y, x+1], src[y+1, x+1], src[y+1, x] };
            res[1 + (y << 1), 1 + (x << 1)] = calc(ref pts, 0);
            res[1 + (y << 1), 2 + (x << 1)] = calc(ref pts, 1);
            res[2 + (y << 1), 2 + (x << 1)] = calc(ref pts, 2);
            res[2 + (y << 1), 1 + (x << 1)] = calc(ref pts, 3);
        }

        private static Bgr calc(Bgr x, Bgr y, double wx, double wy)
        {
            double b = wx * x.Blue + wy * y.Blue, 
                   g = wx * x.Green + wy * y.Green, 
                   r = wx * x.Red + wy * y.Red;
            return new Bgr(b / (wx + wy), g / (wx + wy), r / (wx + wy));
        }

        private static Bgr calc(ref Bgr[] v, int start)
        {
            double b = 0.0, g = 0.0, r = 0.0;
            for (int i = 0; i < 4; i++)
            {
                b += kernel[i] * v[(start + i) % 4].Blue;
                g += kernel[i] * v[(start + i) % 4].Green;
                r += kernel[i] * v[(start + i) % 4].Red;
            }
            return new Bgr(b / sweight, g / sweight, r / sweight);
        }
    }
}
