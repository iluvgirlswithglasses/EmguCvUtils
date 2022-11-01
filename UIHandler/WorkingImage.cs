using System;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCvUtils.UIHandler
{
    internal class WorkingImage
    {
        public Image<Bgr, byte> canvas;
        Image<Bgr, byte> zoomed;

        double zoomScale = 1.0;
        int zoomY = 0, zoomX = 0;

        public WorkingImage()
        {

        }

        public void LoadNewImage(Image<Bgr, byte> img)
        {
            canvas = img;
            zoomed = img;
        }

        public BitmapSource ToBitMap()
        {
            return BitmapSourceConvert.ToBitmapSource(ref zoomed);
        }

        /** @zooming */
        public void SetZoom(double dy, double dx, double mod)
        {
            if (!inRange(zoomScale + mod, 0.0, 1.0))
                return;
            zoomScale += mod;
            zoomScale = Math.Max(0.0, zoomScale);

            // the center
            zoomY = Convert.ToInt32(dy * zoomed.Height) + zoomY;
            zoomX = Convert.ToInt32(dx * zoomed.Width) + zoomX;

            // translate the center to top-left coner
            int nxtHeight = Convert.ToInt32(canvas.Height * zoomScale);
            int nxtWidth = Convert.ToInt32(canvas.Width * zoomScale);
            zoomY -= nxtHeight / 2;
            zoomX -= nxtWidth / 2;

            // make this safe
            zoomY = Math.Max(0, zoomY);
            zoomX = Math.Max(0, zoomX);

            //
            if (zoomY + nxtHeight > canvas.Height)
                zoomY = canvas.Height - nxtHeight;
            if (zoomX + nxtWidth > canvas.Width)
                zoomX = canvas.Width - nxtWidth;
            //
            zoomed = new Image<Bgr, byte>(
                nxtWidth,
                nxtHeight
            );
            Console.WriteLine("{0}, {1}, {2}, {3}", zoomY, zoomX, zoomed.Height, zoomed.Width);
            updateZoomImage();
        }

        private void updateZoomImage()
        {
            for (int y = 0; y < zoomed.Height; y++)
                for (int x = 0; x < zoomed.Width; x++)
                    zoomed[y, x] = canvas[zoomY + y, zoomX + x];
        }

        private bool isSafe(double dy, double dx, double mod)
        {
            return inRange(zoomScale + mod, 0.0, 1.0) && inRange(dy, 0.0, 1.0) && inRange(dx, 0.0, 1.0);
        }

        private bool inRange(double x, double l, double r)
        {
            return l <= x && x <= r;
        }
    }
}
