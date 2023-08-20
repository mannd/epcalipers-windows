using EPCalipersCore;
using EPCalipersCore.Properties;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfTransparentWindow
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window, IDisposable
	{
		#region Fields
		System.Windows.Point firstPoint;
		readonly Preferences preferences;
		readonly MeasureRRDialog measureRRDialog = new MeasureRRDialog();
		readonly CalibrationDialog calibrationDialog = new CalibrationDialog();
		public static RoutedCommand AddTimeCaliperCommand = new RoutedCommand();
		public static RoutedCommand AddAmplitudeCaliperCommand = new RoutedCommand();
		public static RoutedCommand AddAngleCaliperCommand = new RoutedCommand();
		public static RoutedCommand DeleteAllCalipersCommand = new RoutedCommand();
		public static RoutedCommand DeleteCaliperCommand = new RoutedCommand();
		public static RoutedCommand SetCalibrationCommand = new RoutedCommand();
		public static RoutedCommand ClearCalibrationCommand = new RoutedCommand();
		public static RoutedCommand ToggleRateIntervalCommand = new RoutedCommand();
		public static RoutedCommand MeanIntervalCommand = new RoutedCommand();
		public static RoutedCommand QTcCommand = new RoutedCommand();
		public static RoutedCommand OptionsCommand = new RoutedCommand();
		public static RoutedCommand HelpCommand = new RoutedCommand();
		public static RoutedCommand AboutCommand = new RoutedCommand();

		readonly Button[] mainMenu;
		readonly Button[] secondaryMenu;
		readonly Button[] calibrationMenu;

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
					   new ExecutedRoutedEventHandler(delegate (object sender, ExecutedRoutedEventArgs args)
					   {
						   this.Close();
					   })));
			preferences = new Preferences();
			mainMenu = new Button[] { AddButton, CalibrateButton, ClearButton, RateIntButton, MeanRateButton, QTcButton };
			secondaryMenu = new Button[] { MeasureButton, CancelButton };
			calibrationMenu = new Button[] { SetButton, ClearButton, CancelCalibrationButton };
			ShowMainMenu();
		}

		public void AddTimeCaliperCommandExecute(object sender, ExecutedRoutedEventArgs e)
		{
			CommonCaliper.AddCaliper(canvas, CaliperDirection.Horizontal, SetupCaliper);
		}

		public void AddAmplitudeCaliperCommandExecute(object sender, ExecutedRoutedEventArgs e)
		{
			CommonCaliper.AddCaliper(canvas, CaliperDirection.Vertical, SetupCaliper);
		}

		public void AddAngleCaliperCommandExecute(object sender, ExecutedRoutedEventArgs e)
		{
			CommonCaliper.AddAngleCaliper(canvas, SetupCaliper);
		}

		public void MinimizeWindowCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			this.WindowState = WindowState.Minimized;
		}

		public void DeleteCaliperCommandExecute(object sender, ExecutedRoutedEventArgs e)
		{
			canvas.DeleteSelectedCaliper();
			canvas.DrawCalipers();
		}

		public void DeleteAllCalipersCommandExecute(object sender, ExecutedRoutedEventArgs e)
		{
			canvas.DeleteAllCalipers();
			canvas.DrawCalipers();
		}

		public void SetCalibrationCommandExecute(object sender, ExecutedRoutedEventArgs e)
		{
			CommonCaliper.SetCalibration(canvas, preferences, calibrationDialog, 1, canvas.DrawCalipers, ShowMainMenu);
		}
		public void ClearCalibrationCommandExecute(object sender, ExecutedRoutedEventArgs e)
		{
			CommonCaliper.ClearCalibration(canvas, canvas.DrawCalipers, EnableMeasurementMenuItems);
		}

		public void ToggleRateIntervalCommandExecute(object sender, ExecutedRoutedEventArgs e)
		{
			canvas.HorizontalCalibration.DisplayRate = !canvas.HorizontalCalibration.DisplayRate;
			canvas.DrawCalipers();
		}

		public void MeanIntervalCommandExecute(object sender, ExecutedRoutedEventArgs e)
		{
			CommonCaliper.MeasureMeanIntervalRate(canvas, canvas.DrawCalipers, measureRRDialog, preferences);
		}

		public void QTcCommandExecute(object sender, ExecutedRoutedEventArgs e)
		{
			CommonCaliper.QTcInterval(canvas, canvas.DrawCalipers, ShowQTcStep1Menu, ShowMainMenu);
		}

		public void OptionsCommandExecute(object sender, ExecutedRoutedEventArgs e)
		{
			Debug.Print("Open Options");
		}

		public void HelpCommandExecute(object sender, ExecutedRoutedEventArgs e)
		{
            System.Windows.Forms.Help.ShowHelp(null, System.AppDomain.CurrentDomain.BaseDirectory + "epcalipers-help.chm");
		}

		public void AboutCommandExecute(object sender, ExecutedRoutedEventArgs e)
		{
			Debug.Print("Open About Box");
			//TODO: About box needs to be separate project to allow use by both types of windows.
			AboutBox aboutBox = new AboutBox();
			if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
			{
				aboutBox.AdditionalOptions = true;
			}
			aboutBox.ShowDialog();
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
			canvas.ClearAllChosenComponents();
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
			canvas.ClearAllChosenComponents();
			CommonCaliper.SelectCaliperColor(canvas, canvas.DrawCalipers);
		}

		private void TweakCaliperPositionMenuItem_Click(object sender, RoutedEventArgs e)
		{
			ShowTweakMenu();
			TweakCaliper();
			canvas.DrawCalipers();
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
			Debug.Print("Mouse down");
			var clickPoint = e.GetPosition(canvas);
			if (e.ChangedButton == MouseButton.Right)
			{
				Debug.Write("right button clicked");
				Debug.WriteLine("X={0}, Y={1}", clickPoint.X, clickPoint.Y);
				canvas.Focus();
				RightClickMenu.Visibility = Visibility.Hidden;
				canvas.SetChosenCaliper(clickPoint);
				canvas.SetChosenCaliperComponent(clickPoint);
				if (canvas.NoChosenCaliper() && canvas.TweakingComponent)
				{
					CancelTweaking();

				}
				if (!canvas.TweakingComponent)
				{
					bool pointNearCaliper = canvas.PointIsNearCaliper(clickPoint);
					// Can't disable whole context menu, or it won't disappear with another click
					TweakCaliperPositionMenuItem.IsEnabled = pointNearCaliper;
					CaliperColorMenuItem.IsEnabled = pointNearCaliper;
					MarchingCaliperMenuItem.IsEnabled = pointNearCaliper;
					if (pointNearCaliper)
					{
						BaseCaliper c = canvas.GetGrabbedCaliper(clickPoint);
						if (c != null)
						{
							MarchingCaliperMenuItem.IsChecked = c.isMarching && c.IsTimeCaliper();
							MarchingCaliperMenuItem.IsEnabled = c.IsTimeCaliper();
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
					canvas.DrawCalipers();
				}
			}
			else // if (!canvas.TweakingComponent)
			{
				firstPoint = clickPoint;
				canvas.GrabCaliperIfClicked(clickPoint);
			}
		}

		private void Canvas_MouseMove(object sender, MouseEventArgs e)
		{
			//Debug.Print("Mouse move");
			var newPoint = new System.Windows.Point(e.GetPosition(canvas).X, e.GetPosition(canvas).Y);
			float deltaX = (float)(newPoint.X - firstPoint.X);
			float deltaY = (float)(newPoint.Y - firstPoint.Y);
			if (canvas.DragGrabbedCaliper(deltaX, deltaY, firstPoint))
			{
				firstPoint = newPoint;
				canvas.DrawCalipers();
			}
		}

		private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
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
			canvas.ClearAllChosenComponentsExceptForChosenCaliper();
			if (canvas.ChosenComponent != CaliperComponent.NoComponent)
			{
				if (canvas.GetChosenCaliper() != null)
				{
					canvas.GetChosenCaliper().isTweaking = true;
				}
				string componentName = canvas.GetChosenComponentName();
				string message = string.Format("Tweak {0} with arrow or ctrl-arrow key",
					componentName);
				TweakTextBlock.Text = message;
				canvas.TweakingComponent = true;
			}
			else
			{
				CancelTweaking();
			}
			canvas.DrawCalipers();
		}

		private void CancelTweaking()
		{
			ShowMenu(previousMenu);
			canvas.CancelTweaking();
			canvas.DrawCalipers();
		}

		private void Canvas_KeyDown(object sender, KeyEventArgs e)
		{
			if (!canvas.TweakingComponent)
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
				case Key.Escape:
					CancelTweaking();
					e.Handled = true;
					break;
				default:
					Debug.Print("misc key pressed");
					e.Handled = false;
					break;
			}
			canvas.DrawCalipers();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (measureRRDialog != null) measureRRDialog.Dispose();
				if (calibrationDialog != null) calibrationDialog.Dispose();
			}
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
	public static class CustomCommands
	{
		public static readonly RoutedUICommand MinimizeWindow = new RoutedUICommand
			(
				"MinimizeWindow",
				"MinimizeWindow",
				typeof(CustomCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.Down, ModifierKeys.Windows)
				}
			);
	}
}
