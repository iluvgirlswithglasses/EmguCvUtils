using System;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using EmguCvUtils.Drawer;
using EmguCvUtils.Type;
using EmguCvUtils.Util.Detector;

namespace EmguCvUtils.UIHandler
{
    internal class ImageComparationController
    {
        static public void OfflineLibCompare(string sti, string ndi, string sto, string ndo)
        {
            var src = new Image<Bgr, byte>(sti);
            var cmp = new Image<Bgr, byte>(ndi);

            var st = new Image<Gray, byte>(sti);
            var nd = new Image<Gray, byte>(ndi);
            var stMatches = new List<Pair<int, int>>();
            var ndMatches = new List<Pair<int, int>>();
            
            KeypointsMatching.LibCompare(ref st, ref nd, ref stMatches, ref ndMatches);

            foreach (var p in stMatches)
                Dot.Rect(ref src, p.St, p.Nd, 2, new Bgr(0, 255, 0));
            foreach (var p in ndMatches)
                Dot.Rect(ref cmp, p.St, p.Nd, 2, new Bgr(0, 255, 0));

            CvInvoke.Imwrite(sto, src);
            CvInvoke.Imwrite(ndo, cmp);
        }
    }
}
