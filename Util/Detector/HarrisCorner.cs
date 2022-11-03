using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.Structure;
using Emgu.CV;

namespace EmguCvUtils.Util.Detector
{
    static public class HarrisCorner
    {
        static public void Apply(ref Image<Gray, byte> canvas, float threshold = 1e9f)
        {
            Image<Gray, float>
                gradX = new Image<Gray, float>(canvas.Size),
                gradY = gradX.Copy(),
                x2Derivative = gradX.Copy(),
                y2Derivative = gradX.Copy(),
                xyDerivative = gradX.Copy(),
                x2y2Derivative = gradX.Copy(),
                det = gradX.Copy(),
                trace = gradX.Copy(),
                uncleansed = gradX.Copy(),
                res = gradX.Copy();

            //
            CvInvoke.Sobel(canvas, gradX, Emgu.CV.CvEnum.DepthType.Cv32F, 1, 0, 3); // dx=1, dy=0, ksize=3
            CvInvoke.Sobel(canvas, gradY, Emgu.CV.CvEnum.DepthType.Cv32F, 0, 1, 3); // dx=0, dy=1, ksize=3
            //
            CvInvoke.Multiply(gradX, gradX, x2Derivative);
            CvInvoke.Multiply(gradY, gradY, y2Derivative);
            CvInvoke.Multiply(gradX, gradY, xyDerivative);
            //
            x2Derivative = x2Derivative.SmoothGaussian(3);
            y2Derivative = y2Derivative.SmoothGaussian(3);
            xyDerivative = xyDerivative.SmoothGaussian(3);
            // det = x2*y2 - (xy)2
            CvInvoke.Multiply(x2Derivative, y2Derivative, det);
            CvInvoke.Multiply(xyDerivative, xyDerivative, x2y2Derivative);
            CvInvoke.Subtract(det, x2y2Derivative, det);
            // trace = x2 + y2
            CvInvoke.Add(x2Derivative, y2Derivative, trace);
            // trace = k * (trace**2)
            CvInvoke.Multiply(trace, trace, trace);
            for (int y = 0; y < canvas.Height; y++)
                for (int x = 0; x < canvas.Width; x++)
                    trace[y, x] = new Gray(trace[y, x].Intensity * 0.04);
            //
            // CvInvoke.Subtract(det, trace, uncleansed);
            // clean(ref uncleansed, ref res, threshold);
            CvInvoke.Subtract(det, trace, res);

            for (int y = 0; y < canvas.Height; y++)
                for (int x = 0; x < canvas.Width; x++)
                    if (res[y, x].Intensity > threshold)
                        canvas[y, x] = new Gray(255);
                    else
                        canvas[y, x] = new Gray(0);

        }

        static public void LibApply(ref Image<Gray, byte> canvas,float threshold)
        {
            Image<Gray, float> cornerImg = new Image<Gray, float>(canvas.Size);
            CvInvoke.CornerHarris(canvas, cornerImg, 3, 3, 0.04);

            for (int y = 0; y < canvas.Height; y++)
                for (int x = 0; x < canvas.Width; x++)
                    if (cornerImg[y, x].Intensity > threshold)
                        canvas[y, x] = new Gray(255);
                    else
                        canvas[y, x] = new Gray(0);
        }
    }
}
