﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCvUtils.UIHandler;
using EmguCvUtils.Util.Detector;
using static EmguCvUtils.Effect.ContrastEffect;

namespace EmguCvUtils.Dialog
{
    /// <summary>
    /// Interaction logic for GrayWindow.xaml
    /// </summary>
    public partial class GrayWindow : Window
    {
        Image<Bgr, byte> src;
        WorkingImage<Gray> canvas = new WorkingImage<Gray>();

        public GrayWindow(ref Image<Bgr, byte> c)
        {
            src = c;
            InitializeComponent();
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

        private void presenterLoaded(object sender, RoutedEventArgs args)
        {
            reload(null, null);
        }

        /** @file-btn-handlers */
        private void close(object sender, RoutedEventArgs args)
        {
            Close();
        }

        private void reload(object sender, RoutedEventArgs args)
        {
            canvas.LoadNewImage(src.Copy().Convert<Gray, byte>());
            display(canvas.ToBitMap());
        }

        private void write(object sender, RoutedEventArgs args)
        {
            double thresh = writeThreshold.Value;
            for (int y = 0; y < canvas.canvas.Height; y++)
                for (int x = 0; x < canvas.canvas.Width; x++)
                    if (canvas.canvas[y, x].Intensity >= thresh)
                        src[y, x] = new Bgr(0, canvas.canvas[y, x].Intensity, 0);
            Close();
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

        /** @util-btns listeners */
        private void applyXSobel(object sender, RoutedEventArgs args)
        {
            SobelOperator.Apply(ref canvas.canvas);
            canvas.UpdatePresenter();
            display(canvas.ToBitMap());
        }

        private void applyXYSobel(object sender, RoutedEventArgs args)
        {
            SobelOperator.Apply(ref canvas.canvas, 1);
            canvas.UpdatePresenter();
            display(canvas.ToBitMap());
        }
    }
}
