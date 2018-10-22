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
        Point firstPoint;

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
            Debug.Write("button clicked.");
            var myLine = new Line
            {
                Stroke = System.Windows.Media.Brushes.Black,

                X1 = 100,
                X2 = 140,  // 150 too far
                Y1 = 200,
                Y2 = 200,

                StrokeThickness = 1
            };

            canvas.Children.Add(myLine);

          
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPoint = new Point(e.GetPosition(canvas).X, e.GetPosition(canvas).Y);

            if (e.ChangedButton == MouseButton.Right)
            {
                Debug.Write("right button clicked");
                Debug.WriteLine("X={0}, Y={1}", clickPoint.X, clickPoint.Y);
                return;
            }
            Debug.Write("left mouse clicked over canvas.");
            Debug.WriteLine("X={0}, Y={1}", clickPoint.X, clickPoint.Y);
            firstPoint = clickPoint;
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point newPoint = new Point(e.GetPosition(canvas).X, e.GetPosition(canvas).Y);
            double deltaX = newPoint.X - firstPoint.X;
            double detalY = newPoint.Y - firstPoint.Y;
        }
    }
}
