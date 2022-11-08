using System;
using System.Windows;
using Emgu.CV.Structure;
using EmguCvUtils.UIHandler;
using static EmguCvUtils.Effect.ContrastEffect;

namespace EmguCvUtils.Dialog
{
    /// <summary>
    /// Interaction logic for ContrastWindow.xaml
    /// </summary>
    public partial class ContrastWindow : Window
    {
        WorkingImage<Bgr> canvas;

        public ContrastWindow(ref WorkingImage<Bgr> c)
        {
            canvas = c;
            InitializeComponent();
        }

        private void close(object sender, RoutedEventArgs args)
        {
            Close();
        }

        private void apply(object sender, RoutedEventArgs args)
        {
            Apply(ref canvas.src, intensity.Value, bias.Value);
            canvas.Reload();
            Close();
        }
    }
}
