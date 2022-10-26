using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCvUtils.UIHandler
{
    internal class Tester
    {
        public static void Test(string img0p, string img1p, string op)
        {
            var st = new Image<Gray, byte>(img0p);
            CvInvoke.Imwrite(op, st);
        }
    }
}
