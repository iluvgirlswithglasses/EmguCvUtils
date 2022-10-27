using System;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using EmguCvUtils.Util.Binary;

namespace EmguCvUtils.UIHandler
{
    internal class SkeletonizationController
    {
        static public void OnlineSkeletonize(string si, string so1, string so2)
        {
            Console.WriteLine("[UIHandler.SkeletonizationController][OnlineSkeletonize] processing {0}", si);
            Image<Gray, byte> fi = new Image<Gray, byte>(si);
            BinaryEffect.Apply(ref fi, ref fi, 100);

            var ori = new Skeletonization(ref fi);
            ori.Apply();

            var mod = new SkeletonVariant(ref fi);
            mod.Apply();

            CvInvoke.Imwrite(so1, ori.Img);
            CvInvoke.Imwrite(so2, mod.Img);
        }
    }
}
