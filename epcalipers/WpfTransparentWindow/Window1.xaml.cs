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
using EPCalipersCore;
using EPCalipersCore.Properties;
using System.Drawing;

namespace WpfTransparentWindow
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        System.Windows.Point firstPoint;
        Preferences preferences;

        public Window1()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close,
                       new ExecutedRoutedEventHandler(delegate (object sender, ExecutedRoutedEventArgs args) { this.Close(); })));
            preferences = new Preferences();
        }

        public void DragWindow(object sender, MouseButtonEventArgs args)
        {
            DragMove();
        }

        public void ButtonClicked(object sender, RoutedEventArgs args)
        {
            Debug.Write("button clicked.");
            Caliper c = new Caliper();
            c.Direction = CaliperDirection.Horizontal;
            if (c.Direction == CaliperDirection.Horizontal)
            {
                c.CurrentCalibration = canvas.HorizontalCalibration;
            }
            else
            {
                c.CurrentCalibration = canvas.VerticalCalibration;
            }
            SetupCaliper(c);
        }

        private void SetupCaliper(Caliper c)
        {
            c.LineWidth = preferences.LineWidth;
            c.UnselectedColor = preferences.CaliperColor;
            c.SelectedColor = preferences.HighlightColor;
            c.CaliperColor = c.UnselectedColor;
            c.rounding = preferences.RoundingParameter();
            c.SetInitialPosition();
            canvas.AddCaliper(c);
            canvas.DrawCalipers();
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var clickPoint = e.GetPosition(canvas);
            if (e.ChangedButton == MouseButton.Right)
            {
                Debug.Write("right button clicked");
                Debug.WriteLine("X={0}, Y={1}", clickPoint.X, clickPoint.Y);
                return;
            }
            if (e.ClickCount == 2)
            {
                Debug.Print("Double Click!"); //handle the double click event here...
                if (canvas.DeleteCaliperIfClicked(clickPoint))
                {
                    Debug.Print("Trying to delete caliper...");
                    canvas.DrawCalipers();
                }
            }
            else
            {
                firstPoint = clickPoint;
                canvas.GrabCaliperIfClicked(clickPoint);
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            var newPoint = new System.Windows.Point(e.GetPosition(canvas).X, e.GetPosition(canvas).Y);
            float deltaX = (float)(newPoint.X - firstPoint.X);
            float deltaY = (float)(newPoint.Y - firstPoint.Y);
            if (canvas.DragGrabbedCaliper(deltaX, deltaY, firstPoint))
            {
                firstPoint = newPoint;
                canvas.DrawCalipers();
            }
        }

        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Debug.Print("Mouse up");
            if (canvas.ReleaseGrabbedCaliper(e.ClickCount))
            {
                canvas.DrawCalipers();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            canvas.DrawCalipers();
        }

    }
}
