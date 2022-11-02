using System;
using System.Windows;
using EmguCvUtils.UIHandler;
using static EmguCvUtils.Effect.ContrastEffect;

namespace EmguCvUtils.Dialog
{
    /// <summary>
    /// Interaction logic for ContrastWindow.xaml
    /// </summary>
    public partial class ContrastWindow : Window
    {
        WorkingImage canvas;

        public ContrastWindow(ref WorkingImage c)
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
            Apply(ref canvas.canvas, intensity.Value, bias.Value);
            canvas.UpdatePresenter();
            Close();
        }
    }
}
