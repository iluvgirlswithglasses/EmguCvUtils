using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCvUtils.Util.Binary
{
    static public class SkeletonCorner
    {

        static private int[] DY = new int[8] { -1, -1, -0, +1, +1, +1, -0, -1 };
        static private int[] DX = new int[8] { +0, +1, +1, +1, +0, -1, -1, -1 };

        const double threshold = 250;

        static public void Apply(ref Image<Gray, byte> src)
        {
            var des = new Image<Gray, byte>(src.Size);
            for (int y = 1; y < src.Height - 1; y++)
                for (int x = 1; x < src.Width - 1; x++)
                    if (src[y, x].Intensity > threshold && isCorner(ref src, y, x))
                        des[y, x] = new Gray(255);
            src = des.Copy();
        }

        static private bool isCorner(ref Image<Gray, byte> src, int y, int x)
        {
            int cnt = 0;
            for (int i = 0; i < 8; i++)
                if (src[y + DY[i], x + DX[i]].Intensity > threshold)
                    cnt++;
            return cnt == 1;
        }
    }
}
