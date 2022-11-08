using System;
using Emgu.CV.Structure;
using Emgu.CV;
using static System.Math;
using System.Windows.Controls;

namespace EmguCvUtils.Util.Transform
{
    /*
    references:
        https://en.wikipedia.org/wiki/Affine_transformation
        https://en.wikipedia.org/wiki/Transformation_matrix#Rotation
        https://www.algorithm-archive.org/contents/affine_transformations/affine_transformations.html

    the idea of the matrix given in wiki is that:
        given a point P, 
        after multiplying it with the matrix M, 
        we got P' which is the point after rotation

    so with every point P(y, x), after transformation it becomes P'(y', x') where:
        y' = x * sin(a) + y * cos(a)
        x' = x * cos(a) - y * sin(a)

    reversed approach:
        y = - x * sin(a) + y * cos(a)
        x = + x * sin(a) + y * cos(a)
    */

    public static class AffineRotation
    {
        static public Image<Bgr, Byte> CreateDeg(ref Image<Bgr, Byte> canvas, double deg)
        {
            return Create(ref canvas, deg / 90.0 * Acos(0));
        }

        static public Image<Bgr, Byte> Create(ref Image<Bgr, Byte> src, double rad)
        {
            Image<Bgr, Byte> res = new Image<Bgr, Byte>(
                src.Width, src.Height, new Bgr(0, 0, 0)
            );
            int cy = src.Height >> 1, cx = src.Width >> 1;
            //
            for (int y = 0; y < res.Height; y++)
            {
                for (int x = 0; x < res.Width; x++)
                {
                    // given P', calculate P

                    int py = -cy + y, px = -cx + x;
                    int dy = affine(-px, +py, rad) + cy,
                        dx = affine(+px, +py, rad) + cx;

                    if (0 <= dy && dy < src.Height && 0 <= dx && dx < src.Width)
                        res[y, x] = src[dy, dx];
                }
            }
            //
            return res;
        }

        static private int affine(int x, int y, double rad)
        {
            return (int)Round(x * Sin(rad) + y * Cos(rad));
        }
    }
}
