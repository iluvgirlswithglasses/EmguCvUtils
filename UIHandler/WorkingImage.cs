using System;
using System.Windows.Media.Imaging;
using Emgu.CV;
using EmguCvUtils.Util.Transform;

namespace EmguCvUtils.UIHandler
{
    public class WorkingImage<Color> where Color: struct, Emgu.CV.IColor
    {
        // src: what to write to system
        public Image<Color, byte> src;
        // canvas: what to work on
        public Image<Color, byte> canvas;
        // presenter: what is shown to the user
        public Image<Color, byte> presenter;

        double zoomScale = 1.0;
        int presenterY0 = 0, presenterX0 = 0;

        public WorkingImage()
        {

        }

        public void LoadNewImage(Image<Color, byte> img)
        {
            src = img.Copy();
            canvas = img.Copy();
            presenter = img.Copy();
        }

        public void Reload()
        {
            canvas = src.Copy();
            presenter = src.Copy();
        }

        public BitmapSource ToBitMap()
        {
            return BitmapSourceConvert.ToBitmapSource<Color>(ref presenter);
        }

        /** @zooming */
        public void SetZoom(double dy, double dx, double mod)
        {
            double preScale = zoomScale;
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
            presenter = new Image<Color, byte>(
                nxtWidth,
                nxtHeight
            );
            UpdatePresenter();
        }

        /** @rotation */
        public void Rotate(double deg)
        {
            canvas = new AffineRotation<Color>().CreateDeg(ref src, deg);
            SetZoom(presenter.Height >> 1, presenter.Width >> 1, 0.0);
        }

        /** @utils */
        public void UpdatePresenter()
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
