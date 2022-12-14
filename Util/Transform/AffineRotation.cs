using System;
using Emgu.CV.Structure;
using Emgu.CV;
using static System.Math;

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
        x = + x * cos(a) + y * sin(a)
    */

    public class AffineRotation<Color> where Color : struct, Emgu.CV.IColor
    {
        public AffineRotation()
        {

        }

        public Image<Color, Byte> CreateDeg(ref Image<Color, Byte> canvas, double deg) 
        {
            return Create(ref canvas, deg / 90.0 * Acos(0));
        }

        public Image<Color, Byte> Create(ref Image<Color, Byte> src, double rad)
        {
            Image<Color, Byte> res = new Image<Color, Byte>(
                (int) Ceiling(src.Width * Abs(Cos(rad)) + src.Height * Abs(Sin(rad))),
                (int) Ceiling(src.Width * Abs(Sin(rad)) + src.Height * Abs(Cos(rad))));
            Console.WriteLine("Image Resolution after {0} rad rorated: {1}x{2}", rad, res.Width, res.Height);
            int srcy = src.Height >> 1, srcx = src.Width >> 1;
            int resy = res.Height >> 1, resx = res.Width >> 1;
            //
            for (int y = 0; y < res.Height; y++)
            {
                for (int x = 0; x < res.Width; x++)
                {
                    // given P', calculate P
                    int py = y - resy, px = x - resx;
                    int dy = srcy + (int) Round(- px * Sin(rad) + py * Cos(rad)),
                        dx = srcx + (int) Round(+ px * Cos(rad) + py * Sin(rad));

                    if (0 <= dy && dy < src.Height && 0 <= dx && dx < src.Width)
                        res[y, x] = src[dy, dx];
                }
            }
            //
            return res;
        }
    }
}
