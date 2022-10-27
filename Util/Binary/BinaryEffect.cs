using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCvUtils.Util.Binary
{
    internal class BinaryEffect
    {
        static public void Apply(ref Image<Gray, byte> src, ref Image<Gray, byte> des, double threshold)
        {
            for (int y = 0; y < src.Height; y++)
            for (int x = 0; x < src.Width; x++)
            {
                if (src[y, x].Intensity < threshold)
                    des[y, x] = new Gray(0);
                else
                    des[y, x] = new Gray(255);
            }
        }
    }
}
