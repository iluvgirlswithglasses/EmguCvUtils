using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCvUtils.Dialog;
using EmguCvUtils.UIHandler;
using EmguCvUtils.Util;
using EmguCvUtils.Util.Transform;

namespace EmguCvUtils
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WorkingImage<Bgr> canvas = new WorkingImage<Bgr>();

        bool isCircular = false;

        /** @sys */
        public MainWindow()
        {
            InitializeComponent();
            mainWindow.KeyDown += new KeyEventHandler(onKeyDown);
        }

        private void display(BitmapSource bm)
        {
            presenter.Source = bm;
        }

        /** @on-loaded */
        private void headerLoaded(object sender, RoutedEventArgs args)
        {
            initHeader(sender as DockPanel);
        }

        private void initHeader(DockPanel header)
        {
            header.MouseLeftButtonDown += (s, e) =>
            {
                DragMove();
            };
        }

        /** @file-btn-handlers */
        private void close(object sender, RoutedEventArgs args)
        {
            Close();
        }

        private void openFile(object sender, RoutedEventArgs args)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Open Picture";

            bool? res = dialog.ShowDialog();
            if (res == true)
            {
                canvas.LoadNewImage(new Image<Bgr, byte>(dialog.FileName));
                header.Text = dialog.FileName;
                display(canvas.ToBitMap());
                Console.WriteLine("Source Image Resolution: {0}x{1}", canvas.canvas.Width, canvas.canvas.Height);
            }
        }

       private void saveFile(object sender, RoutedEventArgs args)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.DefaultExt = ".png";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string filename = dialog.FileName;
                CvInvoke.Imwrite(filename, canvas.canvas);
            }
        }

        /** @keyboard */
        private void onKeyDown(object sender, KeyEventArgs args)
        {
            // zoom
            switch (args.Key)
            {
                case Key.OemPlus:
                    zoomRequest(-0.15);
                    break;
                case Key.OemMinus:
                    zoomRequest(+0.15);
                    break;
            }
        }

        private void zoomRequest(double mod)
        {
            double dy = Mouse.GetPosition(presenter).Y / presenter.ActualHeight;
            double dx = Mouse.GetPosition(presenter).X / presenter.ActualWidth;
            canvas.SetZoom(dy, dx, mod);
            display(canvas.ToBitMap());
        }

        /** @utils-btn-handlers */
        private void circularCompress(object sender, RoutedEventArgs args)
        {
            if (isCircular)
            {
                // reset
                canvas.UpdatePresenter();
                isCircular = false;
            }
            else
            {
                // apply circular effect
                CircularCompress.Apply(ref canvas.presenter);
                isCircular = true;
            }
            display(canvas.ToBitMap());
        }

        private void openContrastEditor(object sender, RoutedEventArgs e)
        {
            ContrastWindow dialog = new ContrastWindow(ref canvas);
            dialog.ShowDialog();
            display(canvas.ToBitMap());
        }

        private void openGrayEditor(object sender, RoutedEventArgs e) {
            GrayWindow dialog = new GrayWindow(ref canvas.canvas);
            dialog.ShowDialog();
            canvas.UpdatePresenter();
            display(canvas.ToBitMap());
        }

        /** @transformation-btns */
        private void affineRotate(object sender, RoutedEventArgs e)
        {
            double deg = rotateAngle.Value;
            canvas.Rotate(deg);
            display(canvas.ToBitMap());
        }
    }
}
