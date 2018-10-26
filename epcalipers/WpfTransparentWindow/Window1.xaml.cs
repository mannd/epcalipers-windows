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
        #region Fields
        System.Windows.Point firstPoint;
        Preferences preferences;
        MeasureRRDialog measureRRDialog = new MeasureRRDialog();
        CalibrationDialog calibrationDialog = new CalibrationDialog();

        Button[] mainMenu;
        Button[] secondaryMenu;
        Button[] calibrationMenu;

        bool inQTcStep1 = false;
        #endregion
        #region Window
        public Window1()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close,
                       new ExecutedRoutedEventHandler(delegate (object sender, ExecutedRoutedEventArgs args) { this.Close(); })));
            preferences = new Preferences();
            mainMenu = new Button[] { AddButton, CalibrateButton, ClearButton, RateIntButton, MeanRateButton, QTcButton };
            secondaryMenu = new Button[] { MeasureButton, CancelButton };
            calibrationMenu = new Button[] { SetButton, ClearButton, CancelCalibrationButton };
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

        public void HideCalibrationMenu(bool hide)
        {
            Visibility visibility = Visibility.Visible;
            if (hide)
            {
                visibility = Visibility.Hidden;
            }
            foreach (Button b in calibrationMenu)
            {
                b.Visibility = visibility;
            }
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
            HideCalibrationMenu(true);
            EnableMeasurementMenuItems(CommonCaliper.MeasurementsAllowed(canvas));
            canvas.Locked = false;
            inQTcStep1 = false;
        }

        private void ShowSecondaryMenu()
        {
            HideMainMenu(true);
            HideSecondaryMenu(false);
            HideCalibrationMenu(true);
        }

        private void ShowCalibrationMenu()
        {
            HideMainMenu(true);
            HideSecondaryMenu(true);
            HideCalibrationMenu(false);
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
            ShowCalibrationMenu();
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
            CommonCaliper.MeasureMeanIntervalRate(canvas, canvas.DrawCalipers, measureRRDialog, preferences);
        }

        private void QTcButtonClicked(object sender, RoutedEventArgs e)
        {
            Debug.Print("QTc clicked");
            CommonCaliper.QTcInterval(canvas, canvas.DrawCalipers, ShowQTcStep1Menu);
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            Debug.Print("Cancel clicked");
            ShowMainMenu();
        }

        private void CancelCalibrationButtonClicked(object sender, RoutedEventArgs e)
        {
            Debug.Print("Cancel calibration clicked");
            ShowMainMenu();
        }

        private void SetButtonClicked(object sender, RoutedEventArgs e)
        {
            Debug.Print("Set calibration clicked");
            CommonCaliper.SetCalibration(canvas, preferences, calibrationDialog, 1, canvas.DrawCalipers, ShowMainMenu);
        }

        private void MeasureButtonClicked(object sender, RoutedEventArgs e)
        {
            Debug.Print("Measure clicked");
            if (inQTcStep1)
            {
                CommonCaliper.MeasureRRForQTc(canvas, measureRRDialog, ShowMainMenu, ShowQTcStep2Menu, preferences); 
            }
            else // in QTc step 2
            {
                CommonCaliper.MeasureQTc(canvas, ShowMainMenu, ShowQTcStep2Menu, preferences);
            }
        }

        #endregion
        #region Right click menu
        private void MarchingCaliperMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Debug.Print("Marching caliper menu item clicked");
            if (canvas.MarchCaliper())
            {
                canvas.DrawCalipers();
                MarchingCaliperMenuItem.IsChecked = true;
            }
            else
            {
                MarchingCaliperMenuItem.IsChecked = false;
            }
        }

        private void CaliperColorMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Debug.Print("Color menu item clicked");
            CommonCaliper.SelectCaliperColor(canvas, canvas.DrawCalipers);
        }
        #endregion
        #region QTc
        public void ShowQTcStep1Menu()
        {
            ShowSecondaryMenu();
            MessageTextBlock.Text = "Measure one or more RR intervals";
            inQTcStep1 = true;
        }

        private void ShowQTcStep2Menu()
        {
            inQTcStep1 = false;
            MessageTextBlock.Text = "Measure QT";
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
                RightClickMenu.Visibility = Visibility.Hidden;
                canvas.SetChosenCaliper(clickPoint);
                canvas.SetChosenCaliperComponent(clickPoint);
                if (canvas.NoChosenCaliper() && canvas.tweakingComponent)
                {
                    CancelTweaking();
                }
                if (!canvas.tweakingComponent)
                {
                    bool pointNearCaliper = canvas.PointIsNearCaliper(clickPoint);
                    // Can't disable whole context menu, or it won't disappear with another click
                    TweakCaliperPositionMenuItem.IsEnabled = pointNearCaliper;
                    CaliperColorMenuItem.IsEnabled = pointNearCaliper;
                    MarchingCaliperMenuItem.IsEnabled = pointNearCaliper;
                    if (pointNearCaliper)
                    {
                        BaseCaliper c = canvas.getGrabbedCaliper(clickPoint);
                        if (c != null)
                        {
                            MarchingCaliperMenuItem.IsChecked = c.isMarching;
                        }
                    }
                    else
                    {
                        MarchingCaliperMenuItem.IsChecked = false;
                    }
                    RightClickMenu.Visibility = Visibility.Visible;
                }
                else
                {
                    TweakCaliper();
                }

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

        private void CancelTweaking()
        {

        }

        private void TweakCaliper()
        {

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
