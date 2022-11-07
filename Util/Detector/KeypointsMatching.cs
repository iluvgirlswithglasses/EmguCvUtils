using System;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using EmguCvUtils.Type;

namespace EmguCvUtils.Util.Detector
{
    /*
    take two images
    response whether these are the same image or not

    references:
        https://stackoverflow.com/questions/843972/image-comparison-fast-algorithm
        https://stackoverflow.com/questions/35194681/how-to-compare-two-group-keypoint-in-opencv
        https://ai.stanford.edu/~syyeung/cvweb/tutorial2.html
        me
    */
    static public class KeypointsMatching
    {
        // self-written implementations of Harris Conner Detection and Keypoints Matching
        // are written in the EmguCvDoodle repository

        static public void LibCompare(
            ref Image<Gray, byte> src, ref Image<Gray, byte> cmp, 
            ref List<Pair<int, int>> srcRes, ref List<Pair<int, int>> cmpRes
        ) {
            var orb = new ORB();

            var keySrc = new VectorOfKeyPoint();
            var descSrc = new Image<Gray, byte>(src.Size);
            orb.DetectAndCompute(src, null, keySrc, descSrc, false);

            var keyCmp = new VectorOfKeyPoint();
            var descCmp = new Image<Gray, byte>(src.Size);
            orb.DetectAndCompute(cmp, null, keyCmp, descCmp, false);

            // brute force matching
            var bf = new BFMatcher(DistanceType.Hamming, true);
            var matches = new VectorOfDMatch();
            bf.Match(descSrc, descCmp, matches);

            // add matched keypoints to res[]
            
            foreach (var match in matches.ToArray())
            {
                var p = keySrc[match.QueryIdx].Point;
                srcRes.Add(new Pair<int, int>(
                    (int) Math.Round(p.Y), (int) Math.Round(p.X)
                ));
                p = keyCmp[match.TrainIdx].Point;
                cmpRes.Add(new Pair<int, int>(
                    (int) Math.Round(p.Y), (int) Math.Round(p.X)
                ));
            }

            Console.WriteLine("[Util.Detector.KeypointsMatching][LibCompare] {0} out of {1} keypoints matched", matches.Size, keySrc.Size);
        }
    }
}
