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
        MeasureRRDialog measureRRDialog = new MeasureRRDialog();
        CalibrationDialog calibrationDialog = new CalibrationDialog();

        Button[] mainMenu;
        Button[] secondaryMenu;

        #region Window
        public Window1()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close,
                       new ExecutedRoutedEventHandler(delegate (object sender, ExecutedRoutedEventArgs args) { this.Close(); })));
            preferences = new Preferences();
            mainMenu = new Button[] { AddButton, CalibrateButton, ClearButton, RateIntButton, MeanRateButton, QTcButton };
            secondaryMenu = new Button[] { MeasureButton, CancelButton };
            ShowMainMenu();
        }

        public void DragWindow(object sender, MouseButtonEventArgs args)
        {
            DragMove();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            canvas.DrawCalipers();
        }

        #endregion
        #region Buttons

        public void HideMainMenu(bool hide)
        {
            Visibility visibility = Visibility.Visible;
            if (hide)
            {
                visibility = Visibility.Hidden;
            }
            foreach (Button b in mainMenu)
            {
                b.Visibility = visibility;
            }
        }

        public void HideSecondaryMenu(bool hide)
        {
            Visibility visibility = Visibility.Visible;
            if (hide)
            {
                visibility = Visibility.Hidden;
            }
            foreach (Button b in secondaryMenu)
            {
                b.Visibility = visibility;
            }
            MessageTextBlock.Visibility = visibility;
        }

        public void AddButtonClicked(object sender, RoutedEventArgs args)
        {
            CommonCaliper.PickAndAddCaliper(canvas, SetupCaliper);
        }

        private void SetupCaliper(Caliper c)
        {
            c.Setup(preferences);
            canvas.AddCaliper(c);
            canvas.DrawCalipers();
        }

        private void ShowMainMenu()
        {
            HideMainMenu(false);
            HideSecondaryMenu(true);
            EnableMeasurementMenuItems(CommonCaliper.MeasurementsAllowed(canvas));
        }

        private void ShowSecondaryMenu()
        {
            HideMainMenu(true);
            HideSecondaryMenu(false);
        }

        private void EnableMeasurementMenuItems(bool enable)
        {
            
            RateIntButton.IsEnabled = enable;
            MeanRateButton.IsEnabled = enable;
            QTcButton.IsEnabled = enable;
        }


        private void CalibrateButtonClicked(object sender, RoutedEventArgs e)
        {
            Debug.Print("Calibrate clicked");
            CommonCaliper.SetCalibration(canvas, preferences, calibrationDialog, 1, canvas.DrawCalipers, ShowMainMenu);
        }

        private void ClearButtonClicked(object sender, RoutedEventArgs e)
        {
            Debug.Print("Clear clicked.");
            CommonCaliper.ClearCalibration(canvas, canvas.DrawCalipers, EnableMeasurementMenuItems);
        }

        private void RateIntButtonClicked(object sender, RoutedEventArgs e)
        {
            Debug.Print("Rate/Int clicked");
            canvas.HorizontalCalibration.DisplayRate = !canvas.HorizontalCalibration.DisplayRate;
            canvas.DrawCalipers();
        }

        private void MeanRateButtonClicked(object sender, RoutedEventArgs e)
        {
            Debug.Print("Mean rate clicked");
        }

        private void QTcButtonClicked(object sender, RoutedEventArgs e)
        {
            Debug.Print("QTc clicked");
            ShowSecondaryMenu();
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            Debug.Print("Cancel clicked");
            ShowMainMenu();
        }

        private void MeasureButtonClicked(object sender, RoutedEventArgs e)
        {
            Debug.Print("Measure clicked");
        }
        #endregion
        #region Mouse
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
        #endregion

    }
}
