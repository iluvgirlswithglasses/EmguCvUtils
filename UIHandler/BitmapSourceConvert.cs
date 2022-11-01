using System;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows;
using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCvUtils.UIHandler
{
    public static class BitmapSourceConvert
    {
        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        public static BitmapSource ToBitmapSource(ref Image<Bgr, byte> image)
        {
            using (System.Drawing.Bitmap source = image.ToBitmap())
            {
                IntPtr ptr = source.GetHbitmap();

                BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(ptr);
                return bs;
            }
        }
    }
}
