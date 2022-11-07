using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCvUtils.Drawer
{
    static public class Dot
    {
        static public void Rect(ref Image<Bgr, byte> src, int y, int x, int r, Bgr color)
        {
            for (int i = -r; i <= r; i++)
                for (int j = -r; j <= r; j++)
                    if (0 <= y + i && y + i < src.Height && 0 <= x + j && x + j < src.Width)
                        src[y + i, x + j] = color;
        }
    }
}
