using System;
using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCvUtils.Util.Binary
{
    // I accidentally found this while working on Skeletonization

    // SkeletonVariant produces "a frame" of the binary image,
    // which contains just as much information as a skeleton
    // But it runs much faster than Skeletonization would

    internal class SkeletonVariant
    {
        // (DY[i], DX[i]): relative position of P[i]
        // remember that DY[] is expressed as relative position in MATRIX, not in euclid plane
        static private int[] DY = new int[8] { -1, -1, -0, +1, +1, +1, -0, -1 };
        static private int[] DX = new int[8] { +0, +1, +1, +1, +0, -1, -1, -1 };
        /*
        it looks like this:

            7 0 1
            6 P 2
            5 4 3
        */

        // epsilon but different
        static private double EPS = 20.0;

        public Image<Gray, byte> Img;
        byte[,] adj;     // adj[] is equivalent to B() in the article
        byte[,] shf;     // shf[] (means shifts[]) is equivalent to A()

        // requirement: src must be a binary image
        // creating a Skeletonization object will clone the image `src` into `Img`
        public SkeletonVariant(ref Image<Gray, byte> src)
        {
            Img = src.Copy();
            adj = new byte[Img.Height, Img.Width];
            shf = new byte[Img.Height, Img.Width];

            // initialize shf[] and adj[]
            init();
        }

        public void Apply() 
        {
            int iter = 0;
            bool updated;
            // is there really a way to make each iteration
            // in this do-while run in O(WH)
            do
            {
                iter++;
                updated = false;
                for (int y = 1; y < Img.Height - 1; y++)
                for (int x = 1; x < Img.Width - 1; x++)
                if (Img[y, x].Intensity > EPS) {
                    // detect (0, 1) patterns
                    shf[y, x] = 0;
                    for (int i = 0; i < 7; i++)
                        shf[y, x] += match01(y, x, i, i+1);
                    shf[y, x] += match01(y, x, 7, 0);

                    if (check(y, x))
                    {
                        updated = true;
                        Img[y, x] = new Gray(0);
                        // update its neighbors
                        for (int i = 0; i < 8; i++) 
                        {
                            int py = y + DY[i], px = x + DX[i];
                            adj[py, px]--;
                            // shf[py, px]
                        }
                    }
                }
            } while (updated);
            Console.WriteLine("[Util.Binary.SkeletonVariant][Apply] {0} iterations", iter);
        }

        // initialize shf[] and adj[]
        private void init()
        {
            //
            for (int y = 1; y < Img.Height - 1; y++)
            for (int x = 1; x < Img.Width - 1; x++) 
            {
                if (Img[y, x].Intensity > EPS) 
                {
                    // check neighbors
                    for (int i = 0; i < 8; i++)
                        adj[y + DY[i], x + DX[i]]++;
                }
            }
        }

        // check if this pixel matches Hildritch's conditions
        private bool check(int y, int x) 
        {
            // here I changed the last condition
            // from `p0 * p2 * p4 == 0 || shf[p2] != 1`
            // to   `p0 * p6 * p4 == 0 || shf[p6] != 1` (mirror)
            // for faster iterations in Apply()'s do-while
            return 
                (2 <= adj[y, x] && adj[y, x] <= 6) &&
                (shf[y, x] == 1) &&
                (isBackground(y, x, 0) || isBackground(y, x, 2) || isBackground(y, x, 6) || shf[y + DY[0], x + DX[0]] != 1) &&
                (isBackground(y, x, 0) || isBackground(y, x, 6) || isBackground(y, x, 4) || shf[y + DY[6], x + DX[6]] != 1);
        }

        private bool isBackground(int y, int x, int i)
        {
            return Img[y + DY[i], x + DX[i]].Intensity < EPS;
        }

        private byte match01(int y, int x, int a, int b) 
        {
            return Convert.ToByte(Img[y + DY[a], x + DX[a]].Intensity + EPS < Img[y + DY[b], x + DX[b]].Intensity);
        }
    }
}
