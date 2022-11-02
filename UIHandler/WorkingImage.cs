using System;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCvUtils.UIHandler
{
    internal class WorkingImage
    {
        public Image<Bgr, byte> canvas;
        Image<Bgr, byte> presenter;
        bool isCircular = false;

        double zoomScale = 1.0;
        int presenterY0 = 0, presenterX0 = 0;

        public WorkingImage()
        {

        }

        public void LoadNewImage(Image<Bgr, byte> img)
        {
            canvas = img.Copy();
            presenter = img.Copy();
        }

        public BitmapSource ToBitMap()
        {
            return BitmapSourceConvert.ToBitmapSource(ref presenter);
        }

        /** @zooming */
        public void SetZoom(double dy, double dx, double mod)
        {
            double preScale = zoomScale;
            zoomScale += mod;
            fixRange(ref zoomScale, 0.1, 1.0);
            if (preScale == zoomScale)
                return; // nothing to do here

            // the center
            presenterY0 = Convert.ToInt32(dy * presenter.Height) + presenterY0;
            presenterX0 = Convert.ToInt32(dx * presenter.Width) + presenterX0;

            // translate from center to top-left corner
            int nxtHeight = Convert.ToInt32(canvas.Height * zoomScale);
            int nxtWidth = Convert.ToInt32(canvas.Width * zoomScale);
            presenterY0 -= nxtHeight / 2;
            presenterX0 -= nxtWidth / 2;

            // make this safe
            fixRange(ref presenterY0, 0, canvas.Height - nxtHeight);
            fixRange(ref presenterX0, 0, canvas.Width - nxtWidth);

            //
            presenter = new Image<Bgr, byte>(
                nxtWidth,
                nxtHeight
            );
            updateZoomImage();
        }

        private void updateZoomImage()
        {
            isCircular = false;
            for (int y = 0; y < presenter.Height; y++)
                for (int x = 0; x < presenter.Width; x++)
                    presenter[y, x] = canvas[presenterY0 + y, presenterX0 + x];
        }

        private void fixRange(ref double x, double l, double r)
        {
            x = Math.Max(l, x);
            x = Math.Min(r, x);
        }

        private void fixRange(ref int x, int l, int r)
        {
            x = Math.Max(l, x);
            x = Math.Min(r, x);
        }

        /** @circular-compress */
        public void CircularCompress()
        {
            if (isCircular)
            {
                isCircular = false;
                updateZoomImage();
                return;
            }
            isCircular = true;
            int cy = presenter.Height >> 1, cx = presenter.Width >> 1;
            int r = Math.Min(cy, cx);
            r *= r;
            //
            for (int y = 0; y < presenter.Height; y++) for (int x = 0; x < presenter.Width; x++)
            {
                if (sqr(cy - y) + sqr(cx - x) < r)
                {
                    // half the circular pixel in this row
                    int half = cx - x;
                    // assume the diameter is odd
                    int cnt = half << 1 | 1;
                    // circular pixel [0] = avg(row[0:range])
                    double range = (double) presenter.Width / cnt;
                    // Console.WriteLine(String.Format("y={0} cnt={1} range={2}", y, cnt, range));
                    //
                    Bgr[] v = new Bgr[cnt];
                    for (int i = 0; i < cnt; i++)
                        v[i] = avg(y, (int)Math.Round(range * i), (int)Math.Round(range * (i + 1)));
                    for (int i = 0; i < presenter.Width; i++)
                        presenter[y, i] = new Bgr(0, 0, 0);
                    for (int i = 0; i < cnt; i++)
                        presenter[y, cx - half + i] = v[i];
                    break;
                }
                else if (x == presenter.Width - 1)
                    for (int i = 0; i < presenter.Width; i++)
                        presenter[y, i] = new Bgr(0, 0, 0);
            }
        }

        private Bgr avg(int y, int le, int ri)
        {
            int n = ri - le;
            double b = 0, g = 0, r = 0;
            if (n == 0)
                return new Bgr(0, 0, 0);
            for (int x = le; x < ri; x++)
            {
                Bgr v = presenter[y, x];
                b += v.Blue;
                g += v.Green;
                r += v.Red;
            }
            return new Bgr(
                b / n,
                g / n,
                r / n
            );
        }

        private int sqr(int x)
        {
            return x * x;
        }
    }
}
