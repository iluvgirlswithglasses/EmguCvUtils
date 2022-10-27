using System;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using EmguCvUtils.Util.Binary;

namespace EmguCvUtils.UIHandler
{
    internal class SkeletonizationController
    {
        static public void OnlineSkeletonize(string si)
        {
            Console.WriteLine("[UIHandler.SkeletonizationController][OnlineSkeletonize] processing {0}", si);
            Image<Gray, byte> fi = new Image<Gray, byte>(si);
            BinaryEffect.Apply(ref fi, ref fi, 100);

            var obj = new Skeletonization(ref fi);
            obj.Apply();

            ImageViewer.Show(obj.Img);
        }
    }
}
