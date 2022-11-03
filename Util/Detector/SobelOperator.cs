using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.Structure;
using Emgu.CV;

namespace EmguCvUtils.Util.Detector
{
    static public class SobelOperator
    {
        // just change the kernel and you got Canny's edge detech algorithm
        static private float[,,] kernel =
        {
            {
                {-1, 0, +1 },
                {-2, 0, +2 },
                {-1, 0, +1 }
            },
            {
                {-2, -1, +0 },
                {-1, +0, +1 },
                {+0, +1, +2 }
            },
        };

        static public void Apply(ref Image<Gray, byte> canvas, int choice = 0)
        {
            Image<Gray, float>
                src = canvas.Convert<Gray, float>(),
                des = new Image<Gray, float>(canvas.Size);
            for (int y = 1; y < canvas.Height - 1; y++)
                for (int x = 1; x < canvas.Width - 1; x++)
                    applyMaskSafe(ref src, ref des, y, x, choice);
            //
            for (int y = 0; y < canvas.Height; y++)
                for (int x = 0; x < canvas.Width; x++)
                    canvas[y, x] = new Gray(des[y, x].Intensity);
        }

        static private void applyMaskSafe(ref Image<Gray, float> src, ref Image<Gray, float> des, int cy, int cx, int choice)
        {
            double res = 0.0, rev = 0.0;
            for (int i = 0; i <= 2; i++)
                for (int j = 0; j <= 2; j++)
                {
                    double p = src[cy - 1 + i, cx - 1 + j].Intensity;
                    res += p * kernel[choice, i, j];
                    rev += -p * kernel[choice, i, j];
                }
            des[cy, cx] = new Gray(Math.Max(res, rev));
        }

        static public Image<Gray, byte> LibCreate(ref Image<Gray, byte> canvas)
        {
            Image<Gray, byte> res = new Image<Gray, byte>(canvas.Size);
            Image<Gray, float> gx = new Image<Gray, float>(canvas.Size);
            CvInvoke.Sobel(canvas, gx, Emgu.CV.CvEnum.DepthType.Cv32F, 1, 0, 3);
            //
            for (int y = 0; y < canvas.Height; y++)
                for (int x = 0; x < canvas.Width; x++)
                    res[y, x] = new Gray(gx[y, x].Intensity);
            return res;
        }
    }
}
