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
using System.Windows.Shapes;

namespace WpfTransparentWindow
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close,
                       new ExecutedRoutedEventHandler(delegate (object sender, ExecutedRoutedEventArgs args) { this.Close(); })));
        }

        public void DragWindow(object sender, MouseButtonEventArgs args)
        {
            DragMove();
        }

        public void ButtonClicked(object sender, RoutedEventArgs args)
        {
            //CrazyWindow win2 = new CrazyWindow();
            //win2.ShowDialog();
            Debug.Write("button clicked.");
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.Write("mouse clicked over image.");
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.Write("mouse clicked over grid.");

        }
    }
}
