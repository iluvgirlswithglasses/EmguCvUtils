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

namespace EmguCvUtils
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Compare(object sender, RoutedEventArgs e)
        {
            UIHandler.ImageComparationController.OfflineLibCompare(IImg0F.Text, IImg1F.Text, OImg0F.Text, OImg1F.Text);
        }

        private void Skeletonize(object sender, RoutedEventArgs e)
        {
            UIHandler.SkeletonizationController.OnlineSkeletonize(IImg0F.Text);
        }
    }
}
