using Emgu.CV;
using Emgu.CV.Structure;
using EmguCvUtils.Algorithm.SweepLine;

namespace EmguCvUtils.Util.Binary
{
    static public class Dilation
    {
        static public void Apply(ref Image<Gray, byte> src, int r)
        {
            DilationSweepLine sl = new DilationSweepLine(src.Height, src.Width);
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    if (src[y, x].Intensity > 250)
                        sl.AddPoint(y, x, r);
            src = new Image<Gray, byte>(src.Size);
            sl.Execute(ref src);
        }
    }
}
