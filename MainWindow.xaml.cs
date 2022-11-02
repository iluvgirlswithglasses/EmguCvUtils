using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCvUtils.Dialog;
using EmguCvUtils.UIHandler;
using EmguCvUtils.Util;

namespace EmguCvUtils
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WorkingImage canvas = new WorkingImage();

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
            }
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

        private void openContrastEditor(object sender, RoutedEventArgs e)
        {
            ContrastWindow dialog = new ContrastWindow(ref canvas);
            dialog.ShowDialog();
            display(canvas.ToBitMap());
        }
    }
}
