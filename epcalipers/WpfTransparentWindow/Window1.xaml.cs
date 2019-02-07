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

        enum Menu
        {
            Main,
            Secondary,
            Calibration,
            Qt1,
            Qt2,
            Tweak
        }
        Menu currentMenu = Menu.Main;
        Menu previousMenu = Menu.Main;

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

        private void ShowMenu(Menu menu)
        {
            switch (menu)
            {
                case Menu.Main:
                    ShowMainMenu();
                    break;
                case Menu.Secondary:
                    ShowSecondaryMenu();
                    break;
                case Menu.Calibration:
                    ShowCalibrationMenu();
                    break;
                case Menu.Tweak:
                    ShowTweakMenu();
                    break;
                case Menu.Qt1:
                    ShowQTcStep1Menu();
                    break;
                case Menu.Qt2:
                    ShowQTcStep2Menu();
                    break;
                default:
                    ShowMainMenu();
                    break;
            }
        }

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

        public void HideTweakMenu(bool hide)
        {
            Visibility visibility = Visibility.Visible;
            if (hide)
            {
                visibility = Visibility.Hidden;
            }
            CancelTweakButton.Visibility = visibility;
            TweakTextBlock.Visibility = visibility;
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
            HideTweakMenu(true);
            EnableMeasurementMenuItems(CommonCaliper.MeasurementsAllowed(canvas));
            canvas.Locked = false;
            inQTcStep1 = false;
            currentMenu = Menu.Main;
        }

        private void ShowSecondaryMenu()
        {
            HideMainMenu(true);
            HideSecondaryMenu(false);
            HideTweakMenu(true);
            HideCalibrationMenu(true);
            currentMenu = Menu.Secondary;
        }

        private void ShowCalibrationMenu()
        {
            HideMainMenu(true);
            HideSecondaryMenu(true);
            HideTweakMenu(true);
            HideCalibrationMenu(false);
            currentMenu = Menu.Calibration;
        }

        private void ShowTweakMenu()
        {
            HideMainMenu(true);
            HideSecondaryMenu(true);
            HideCalibrationMenu(true);
            HideTweakMenu(false);
            previousMenu = currentMenu;
            currentMenu = Menu.Tweak;
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

        private void CancelTweakButtonClicked(object sender, RoutedEventArgs e)
        {
            CancelTweaking();
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

        private void TweakCaliperPositionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowTweakMenu();
            TweakCaliper();
        }

        #endregion
        #region QTc
        public void ShowQTcStep1Menu()
        {
            ShowSecondaryMenu();
            MessageTextBlock.Text = "Measure one or more RR intervals";
            inQTcStep1 = true;
            currentMenu = Menu.Qt1;
        }

        private void ShowQTcStep2Menu()
        {
            ShowSecondaryMenu();
            inQTcStep1 = false;
            MessageTextBlock.Text = "Measure QT";
            currentMenu = Menu.Qt2;
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
                canvas.Focus();
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
                            MarchingCaliperMenuItem.IsChecked = c.isMarching && c.isTimeCaliper();
                            MarchingCaliperMenuItem.IsEnabled = c.isTimeCaliper();
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

        #region Keys

        private void TweakCaliper()
        {
            if (canvas.chosenComponent != CaliperComponent.NoComponent)
            {
                string componentName = canvas.GetChosenComponentName();
                string message = string.Format("Tweak {0} with arrow or ctrl-arrow key.  Right-click to tweak a different component.", 
                    componentName);
                TweakTextBlock.Text = message;
                if (!canvas.tweakingComponent)
                {
                    canvas.tweakingComponent = true;
                }
            }
            else
            {
                CancelTweaking();
            }
        }

        private void CancelTweaking()
        {
            ShowMenu(previousMenu);
            canvas.CancelTweaking();
        }

        private void canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (!canvas.tweakingComponent)
            {
                e.Handled = false;
                return;
            }
            switch (e.Key)
            {
                case Key.Left:
                    if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                    {
                        canvas.MicroMove(MovementDirection.Left);
                    }
                    else
                    {
                        canvas.Move(MovementDirection.Left);
                    }
                    e.Handled = true;
                    break;
                case Key.Right:
                    if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                    {
                        canvas.MicroMove(MovementDirection.Right);
                    }
                    else
                    {
                        canvas.Move(MovementDirection.Right);
                    }
                    e.Handled = true;
                    break;
                case Key.Up:
                    if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                    {
                        canvas.MicroMove(MovementDirection.Up);
                    }
                    else
                    {
                        canvas.Move(MovementDirection.Up);
                    }
                    e.Handled = true;
                    break;
                case Key.Down:
                    if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                    {
                        canvas.MicroMove(MovementDirection.Down);
                    }
                    else
                    {
                        canvas.Move(MovementDirection.Down);
                    }
                    e.Handled = true;
                    break;
                default:
                    Debug.Print("misc key pressed");
                    e.Handled = false;
                    break;
            }
            canvas.DrawCalipers();
        }

        //protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        //{
        //    if (!theCalipers.tweakingComponent)
        //    {
        //        return base.ProcessCmdKey(ref msg, keyData);
        //    }
        //    switch (keyData)
        //    {
        //        case Keys.Left:
        //            theCalipers.Move(MovementDirection.Left);
        //            break;
        //        case Keys.Right:
        //            theCalipers.Move(MovementDirection.Right);
        //            break;
        //        case Keys.Up:
        //            theCalipers.Move(MovementDirection.Up);
        //            break;
        //        case Keys.Down:
        //            theCalipers.Move(MovementDirection.Down);
        //            break;
        //        case Keys.Left | Keys.Control:
        //            theCalipers.MicroMove(MovementDirection.Left);
        //            break;
        //        case Keys.Right | Keys.Control:
        //            theCalipers.MicroMove(MovementDirection.Right);
        //            break;
        //        case Keys.Up | Keys.Control:
        //            theCalipers.MicroMove(MovementDirection.Up);
        //            break;
        //        case Keys.Down | Keys.Control:
        //            theCalipers.MicroMove(MovementDirection.Down);
        //            break;
        //        default:
        //            return base.ProcessCmdKey(ref msg, keyData);
        //    }
        //    ecgPictureBox.Refresh();
        //    return true;
        //}
        #endregion

    }
}
