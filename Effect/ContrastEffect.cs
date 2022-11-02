using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCvUtils.Effect
{
    static internal class ContrastEffect
    {
        static public void Apply(ref Image<Bgr, byte> canvas, double d, double sep)
        {
            for (int y = 0; y < canvas.Height; y++) for (int x = 0; x < canvas.Width; x++)
            {
                Bgr v = canvas[y, x];

                double b = v.Blue / 255.0;
                double g = v.Green / 255.0;
                double r = v.Red / 255.0;

                b = (((b - sep) * d) + sep) * 255.0;
                g = (((g - sep) * d) + sep) * 255.0;
                r = (((r - sep) * d) + sep) * 255.0;

                b = b > 255 ? 255 : b;
                b = b < 0 ? 0 : b;
                g = g > 255 ? 255 : g;
                g = g < 0 ? 0 : g;
                r = r > 255 ? 255 : r;
                r = r < 0 ? 0 : r;

                canvas[y, x] = new Bgr(b, g, r);
            }
        }
    }
}
