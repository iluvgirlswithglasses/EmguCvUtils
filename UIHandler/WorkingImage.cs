using System;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCvUtils.UIHandler
{
    internal class WorkingImage
    {
        public Image<Bgr, byte> canvas;
        Image<Bgr, byte> presenter;

        double zoomScale = 1.0;
        int presenterY0 = 0, presenterX0 = 0;

        public WorkingImage()
        {

        }

        public void LoadNewImage(Image<Bgr, byte> img)
        {
            canvas = img;
            presenter = img;
        }

        public BitmapSource ToBitMap()
        {
            return BitmapSourceConvert.ToBitmapSource(ref presenter);
        }

        /** @zooming */
        public void SetZoom(double dy, double dx, double mod)
        {
            zoomScale += mod;
            fixRange(ref zoomScale, 0.1, 1.0);

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
    }
}
