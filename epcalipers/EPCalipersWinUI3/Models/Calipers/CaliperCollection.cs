using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using WinUIEx;

namespace EPCalipersWinUI3.Models.Calipers
{

	/// <summary>
	/// Maintains a collection of calipers, sets colors, adds, deletes them, etc.
	/// </summary>
	public class CaliperCollection : INotifyPropertyChanged
	{
		private static readonly int _maxNumberIntervals = 10;
		private static readonly double _delta = 1.0;
		private static readonly double _microDelta = 0.2;
		private static Point _leftMovement = new(-_delta, 0);
		private static Point _rightMovement = new(_delta, 0);
		private static Point _upMovement = new(0, -_delta);
		private static Point _downMovement = new(0, _delta);
		private static Point _leftMicroMovement = new(-_microDelta, 0);
		private static Point _rightMicroMovement = new(_microDelta, 0);
		private static Point _upMicroMovement = new(0, -_microDelta);
		private static Point _downMicroMovement = new(0, _microDelta);

		private readonly IList<Caliper> _calipers;
		private readonly ICaliperView _caliperView;
		private readonly ISettings _settings;

		private WindowEx _calibrationWindow;
		private WindowEx _meanRateIntervalWindow;
		private WindowEx _colorWindow;
		private WindowEx _measureQtcWindow;

		private Caliper _grabbedCaliper;
		private Bar _grabbedBar;
		private Point _startingDragPoint;

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public bool CaliperSelectionChanged
		{
			get => _caliperSelectionChanged;
			set
			{
				_caliperSelectionChanged = value;
				OnPropertyChanged(nameof(CaliperSelectionChanged));
			}
		}
		private bool _caliperSelectionChanged;

		private async Task ShowMessage(string title, string message)
		{
			var dialog = MessageHelper.CreateMessageDialog(title, message);
			dialog.XamlRoot = _caliperView.XamlRoot;
			await dialog.ShowAsync();
		}

		public Calibration TimeCalibration { get; set; } = Calibration.Uncalibrated;
		public Calibration AmplitudeCalibration { get; set; } = Calibration.Uncalibrated;
		public AngleCalibration AngleCalibration { get; set; } = AngleCalibration.Uncalibrated;

		// A calibrated time caliper can show interval or rate.
		public bool ShowRate { get; set; } = false;

		public Caliper SelectedCaliper
		{
			get => _selectedCaliper;
			set
			{
				if (_selectedCaliper != value)
				{
					_selectedCaliper = value;
				}
				OnPropertyChanged(nameof(SelectedCaliper));
			}
		}
		private Caliper _selectedCaliper;

		public CaliperType SelectedCaliperType => SelectedCaliper?.CaliperType ?? CaliperType.None;

		/// <summary>
		/// If the caliper collection is locked, then calipers can not be added, deleted, selected or
		/// unselected.  They can be moved, however.  This allows calibration to focus on one caliper.
		/// </summary>
		public bool IsLocked { get; set; }

		public CaliperCollection(ICaliperView caliperView, ISettings settings = null, string defaultUnit = "points", string defaultBpm = "bpm")
		{
			_calipers = new List<Caliper>();
			_caliperView = caliperView;
			_settings = settings ?? Settings.Instance;
			Calibration.DefaultUnit = defaultUnit;
			Calibration.DefaultBpm = defaultBpm;
		}


		public IList<Caliper> FilteredCalipers(CaliperType caliperType)
			=> _calipers.Where(x => x.CaliperType == caliperType).ToList();

		// TODO: Only used in testing so far?  Redundant?
		public void Add(Caliper caliper)
		{
			if (IsLocked) return;
			caliper.AddToView(_caliperView);
			switch (caliper.CaliperType)
			{
				case CaliperType.Time:
					caliper.Calibration = TimeCalibration;
					break;
				case CaliperType.Amplitude:
					caliper.Calibration = AmplitudeCalibration;
					break;
				case CaliperType.Angle:
					var angleCaliper = caliper as AngleCaliper;
					angleCaliper.AngleCalibration = AngleCalibration;
					break;
			}
			caliper.ShowRate = ShowRate;
			caliper.UpdateLabel();
			_calipers.Add(caliper);
		}

		public void AddCaliper(CaliperType type)
		{
			if (IsLocked) return;
			var caliper = Caliper.InitCaliper(type, _caliperView, _settings, TimeCalibration, AmplitudeCalibration, AngleCalibration);
			caliper.AddToView(_caliperView);
			caliper.ShowRate = ShowRate;
			caliper.UpdateLabel();
			_calipers.Add(caliper);
		}

		public void RemoveAtPoint(Point point)
		{
			if (IsLocked) return;
			foreach (var caliper in _calipers)
			{
				var bar = caliper.IsNearBar(point);
				if (bar != null)
				{
					caliper.Remove(_caliperView);
					_calipers.Remove(caliper);
					break;
				}
			}
		}

		private void Move(Point delta)
		{
			if (SelectedCaliper == null) return;
			Caliper caliper = SelectedCaliper;
			Bar bar = caliper.SelectedBar;
			if (bar != null)
			{
				if (caliper.CaliperType == CaliperType.Angle && (bar.BarRole == Bar.Role.LeftAngle || bar.BarRole == Bar.Role.RightAngle))
				{
					AngleCaliper angleCaliper = caliper as AngleCaliper;
					double distance = -delta.X / 2.0;  // gives more fine grained angle caliper movement
					bar.Angle += MathHelper.DegreesToRadians(distance);
					bar.SetAngleBarPosition(new Point(bar.X1, bar.Y1), bar.Angle);
					angleCaliper.DrawTriangleBase();
					angleCaliper.CaliperLabel.Text = angleCaliper.Calibration.GetText(angleCaliper.Value);
					angleCaliper.CaliperLabel.SetPosition();
				}
				else
				{
					caliper.Drag(bar, delta, caliper.Position);
				}
			}
		}

		public void MoveLeft()
		{
			Move(_leftMovement);
		}

		public void MoveRight()
		{
			Move(_rightMovement);
		}

		public void MoveUp()
		{
			Move(_upMovement);

		}
		public void MoveDown()
		{
			Move(_downMovement);
		}

		public void MicroMoveLeft()
		{
			Move(_leftMicroMovement);
		}

		public void MicroMoveRight()
		{
			Move(_rightMicroMovement);
		}

		public void MicroMoveUp()
		{
			Move(_upMicroMovement);
		}
		public void MicroMoveDown()
		{
			Move(_downMicroMovement);
		}

		public void GrabCaliper(Point point)
		{
			// Detect if this is near a caliper bar, and if so, load it up for movement.
			(_grabbedCaliper, _grabbedBar) = GetCaliperAndBarAt(point);
			_startingDragPoint = point;
		}

		public void DragCaliperBar(Point point)
		{
			if (_grabbedCaliper == null || _grabbedBar == null) return;
			var delta = new Point(point.X - _startingDragPoint.X, point.Y - _startingDragPoint.Y);
			_startingDragPoint.X += delta.X;
			_startingDragPoint.Y += delta.Y;
			_grabbedCaliper.Drag(_grabbedBar, delta, _startingDragPoint);
		}

		public void ReleaseGrabbedCaliper()
		{
			_grabbedCaliper = null;
			_grabbedBar = null;
		}

		public void ChangeBounds()
		{
			foreach (var caliper in _calipers)
			{
				caliper.ChangeBounds();
			}
		}

		public void Clear()
		{
			if (IsLocked) return;
			foreach (var caliper in _calipers)
			{
				caliper.Remove(_caliperView);
			}
			SelectedCaliper = null;
			_calipers.Clear();
		}

		public void RemoveActiveCaliper()
		{
			if (IsLocked) return;
			foreach (var caliper in _calipers)
			{
				if (caliper.IsSelected)
				{
					SelectedCaliper = null;
					caliper.Remove(_caliperView);
					_calipers.Remove(caliper);
					break;  // Can only be one selected caliper, so no point checking the rest.
				}
			}
		}

		public void DeleteCaliperAt(Point point)
		{
			var caliperAndBar = GetCaliperAndBarAt(point);
			if (caliperAndBar.Item1 is Caliper caliper)
			{
				SelectedCaliper = null;
				caliper.Remove(_caliperView);
				_calipers.Remove(caliper);
			}
		}

		public bool ToggleMarchingCaliper(Point point)
		{
			if (IsLocked) return false;
			bool isMarching = false;
			foreach (var caliper in _calipers)
			{
				if (caliper.IsNearBar(point) != null)
				{
					if (caliper.CaliperType != CaliperType.Time) break;
					if (caliper is TimeCaliper timeCaliper)
					{
						if (timeCaliper.IsMarching)
						{
							RemoveMarchingCaliper(timeCaliper);
							timeCaliper.IsMarching = false;
						}
						else
						{
							AddMarchingCaliper(timeCaliper);
							timeCaliper.IsMarching = true;
							isMarching = true;
						}
					}
				}
			}
			return isMarching;
		}

		private void AddMarchingCaliper(TimeCaliper timeCaliper)
		{
			timeCaliper.AddMarchingCaliper();
		}

		private void RemoveMarchingCaliper(TimeCaliper timeCaliper)
		{
			timeCaliper.RemoveMarchingCaliper();
		}

		public Color CurrentCaliperColorAt(Point point)
		{
			var caliper = GetCaliperAt(point);
			if (caliper == null) return Microsoft.UI.Colors.Black;
			return caliper.UnselectedColor;
		}

		public void SetCurrentCaliperColor(Point point, Color color)
		{
			var caliper = GetCaliperAt(point);
			if (caliper == null) return;
			caliper.UnselectedColor = color;
			caliper.UnselectFullCaliper();
		}

		public void UnselectAllCalipers()
		{
			// TODO: may need to handle Esc for measurements differently.
			if (IsLocked) return;
			foreach (var caliper in _calipers)
			{
				caliper.UnselectFullCaliper();
				SelectedCaliper = null;
			}
		}

		public (Caliper, Bar) GetCaliperAndBarAt(Point point)
		{
			Bar bar = null;
			var caliper = _calipers.Where(x => (bar = x.IsNearBar(point)) != null).FirstOrDefault();
			if (caliper == null) return (null, null);
			return (caliper, bar);
		}

		public Caliper GetCaliperAt(Point point)
		{
			return GetCaliperAndBarAt(point).Item1 ?? null;
		}

		// New stuff

		public void NewToggleCaliperSelection(Point point)
		{
			if (IsLocked) return;
			bool caliperToggled = false;
			foreach (var caliper in _calipers)
			{
				if (caliper.IsNearBar(point) != null && !caliperToggled)
				{
					caliperToggled = true;
					if (caliper.IsSelected)
					{
						caliper.UnselectFullCaliper();
						SelectedCaliper = null;
					}
					else
					{
						caliper.SelectFullCaliper();
						SelectedCaliper = caliper;
						CaliperSelectionChanged = true;
					}
					NewUnselectCalipersExcept(caliper);
				}
			}
		}

		public void NewToggleComponentSelection(Point point)
		{
			if (IsLocked) return;
			bool caliperToggled = false;
			foreach (var caliper in _calipers)
			{
				var bar = caliper.IsNearBar(point);
				if (bar != null && !caliperToggled)
				{
					caliperToggled = true; // process stopped after caliper found.
					if (bar.IsSelected)
					{
						if (caliper.Selection == CaliperSelection.Full) // go from full to partial selection
						{
							caliper.SelectPartialCaliper(bar);
							SelectedCaliper = caliper;
						}
						else // toggle off partial selection
						{
							caliper.UnselectFullCaliper();
							SelectedCaliper = null;
						}
					}
					else // toggle on partial selection
					{
						caliper.SelectPartialCaliper(bar);
						SelectedCaliper = caliper;
					}
					NewUnselectCalipersExcept(caliper);  // make sure other calipers are unselected
					CaliperSelectionChanged = true;
				}
			}
		}

		private void NewUnselectCalipersExcept(Caliper c)
		{
			foreach (var caliper in _calipers)
			{
				if (caliper != c)
				{
					caliper.UnselectFullCaliper();
				}
			}
		}

		public void RefreshCalipers()
		{
			TimeCalibration.Rounding = _settings.Rounding;
			AmplitudeCalibration.Rounding = _settings.Rounding;
			AngleCalibration.Rounding = _settings.Rounding;
			foreach (var caliper in _calipers)
			{
				caliper.ApplySettings(_settings);
			}
			Debug.Print("refreshing calipers...");
		}

		public async Task SetCalibrationAsync()
		{
			SelectSoleTimeOrAmplitudeCaliper();
			switch (SelectedCaliperType)
			{
				case CaliperType.None:
					await ShowMessage("NoCaliperSelectedTitle".GetLocalized(),
						"NoCaliperSelectedMessage".GetLocalized());
					break;
				case CaliperType.Angle:
					await ShowMessage("AngleCaliperSelectedTitle".GetLocalized(),
						"AngleCaliperSelectedMessage".GetLocalized());
					break;
				case CaliperType.Time:
				case CaliperType.Amplitude:
					IsLocked = true;
					ShowCalibrationDialogWindow(SelectedCaliperType);
					break;
			}
		}

		/// <summary>
		/// If the only caliper in collection is a time or amplitude caliper, select it, otherwise do nothing.
		/// Used to simplify caliper calibration.
		/// </summary>
		private void SelectSoleTimeOrAmplitudeCaliper()
		{
			if (_calipers.Count == 1)
			{
				if (_calipers.First().CaliperType == CaliperType.Angle)
				{
					return;  // Don't bother with angle calipers
				}
				_calipers.First().SelectFullCaliper();
				SelectedCaliper = _calipers.First();
			}
			return;  // 
		}

		/// <summary>
		/// If only 1 time caliper in collection, select it, otherwise do nothing.
		/// Used to simplify mean interval and QTc measurements.
		/// </summary>
		private void SelectSoleTimeCaliper()
		{
			var timeCalipers = FilteredCalipers(CaliperType.Time);
			if (timeCalipers.Count == 1)
			{
				timeCalipers.First().SelectFullCaliper();
				NewUnselectCalipersExcept(timeCalipers.First());
			}
		}

		private void ShowCalibrationDialogWindow(CaliperType caliperType)
		{
			Debug.Assert(SelectedCaliper != null);
			if (_calibrationWindow == null)
			{
				_calibrationWindow = new WindowEx();
			}
			string title;
			switch (caliperType)
			{
				case CaliperType.Time:
					title = "Time caliper calibration";
					break;
				case CaliperType.Amplitude:
					title = "Amplitude caliper calibration";
					break;
				default:
					title = "Calibration";
					break;
			}
			SetupFloatingWindow(_calibrationWindow, 400, 400, title);
			_calibrationWindow.PersistenceId = "CalibrationWindowID";
			var calibrationView = new CalibrationView(SelectedCaliper, this)
			{
				Window = _calibrationWindow,
				CaliperType = caliperType
			};
			_calibrationWindow.Content = calibrationView;
			_calibrationWindow.Closed += OnCalibrationWindowClosed;
			_calibrationWindow.Show();
		}

		private void OnClosed(object sender, WindowEventArgs args)
		{
			IsLocked = false;
			if (_calibrationWindow != null)
			{
				_calibrationWindow.Content = null;
			}
			if (_meanRateIntervalWindow != null)
			{
				_meanRateIntervalWindow.Content = null;
			}
			if (_colorWindow != null)
			{
				_colorWindow.Content = null;
			}
			if (_measureQtcWindow != null)
			{
				_measureQtcWindow.Content = null;
			}
			_calibrationWindow = null;
			_meanRateIntervalWindow = null;
			_colorWindow = null;
			_measureQtcWindow = null;
		}

		private void OnCalibrationWindowClosed(object sender, WindowEventArgs args)
		{
			IsLocked = false;
			if (_calibrationWindow != null)
			{
				_calibrationWindow.Content = null;
			}
			_calibrationWindow = null;
		}
		private void OnMeanRateIntervalWindowClosed(object sender, WindowEventArgs args)
		{
			if (_meanRateIntervalWindow != null)
			{
				_meanRateIntervalWindow.Content = null;
			}
			_meanRateIntervalWindow = null;
		}
		private void OnColorWindowClosed(object sender, WindowEventArgs args)
		{
			if (_colorWindow != null)
			{
				_colorWindow.Content = null;
			}
			_colorWindow = null;
		}
		private void OnMeasureQtcWindowClosed(object sender, WindowEventArgs args)
		{
			if (_measureQtcWindow != null)
			{
				_measureQtcWindow.Content = null;
			}
			_measureQtcWindow = null;
		}

		public void ClearCalibration()
		{
			TimeCalibration = Calibration.Uncalibrated;
			AmplitudeCalibration = Calibration.Uncalibrated;
			AngleCalibration = AngleCalibration.Uncalibrated;
			SetCalibration();
		}

		public void SetCalibration()
		{
			ShowRate = false;
			TimeCalibration.Rounding = _settings.Rounding;
			AmplitudeCalibration.Rounding = _settings.Rounding;
			AngleCalibration.Rounding = _settings.Rounding;
			foreach (var caliper in _calipers)
			{
				switch (caliper.CaliperType)
				{
					case CaliperType.Time:
						caliper.Calibration = TimeCalibration;
						break;
					case CaliperType.Amplitude:
						caliper.Calibration = AmplitudeCalibration;
						break;
					case CaliperType.Angle:
						// TODO: refactor
						AngleCaliper angleCaliper = caliper as AngleCaliper;
						angleCaliper.AngleCalibration.TimeCalibration = TimeCalibration;
						angleCaliper.AngleCalibration.AmplitudeCalibration = AmplitudeCalibration;
						angleCaliper.DrawTriangleBase();
						break;
				}
				caliper.UpdateLabel();
			}
		}

		public async Task ToggleRateInterval()
		{
			switch (TimeCalibration.Parameters.Unit)
			{
				case CalibrationUnit.Uncalibrated:
					await ShowMessage("Time calipers not calibrated",
						"You need to calibrate a time caliper before you can measure heart rates.");
					break;
				case CalibrationUnit.Unknown:
				case CalibrationUnit.None:
					await ShowMessage("Time calipers units not correct",
						"Calibration unit needs to be msec or sec to meaure heart rates.");
					break;
				default:
					ShowRate = !ShowRate;
					var timeCalipers = FilteredCalipers(CaliperType.Time);
					foreach (var caliper in timeCalipers) // Brugada triangle base doesn't toggle rate/interval.
					{
						caliper.ShowRate = ShowRate;
						caliper.UpdateLabel();
					}
					break;
			}
		}

		public async Task MeanRateInterval()
		{
			if (SelectedCaliper == null 
				|| SelectedCaliper.CaliperType != CaliperType.Time 
				|| SelectedCaliper.Calibration == Calibration.Uncalibrated)
			{
				await ShowMessage("HowToMeasureMeanIntervalTitle".GetLocalized(),
					"HowToMeasureMeanIntervalMessage".GetLocalized());
			}
			else
			{
				// show dialog, get number of interval
				ShowMeanRateIntervalDialog(SelectedCaliper);
				//var numberIntervals = 5;
				//var meanInterval = Caliper.MeanInterval(SelectedCaliper.Value, numberIntervals);
			}
		}

		public void ShowMeanRateIntervalDialog(Caliper caliper)
		{
			Debug.Assert(SelectedCaliper != null);
			if (_meanRateIntervalWindow == null)
			{
				_meanRateIntervalWindow = new WindowEx();
			}
			SetupFloatingWindow(_meanRateIntervalWindow, 400, 400, "Mean interval and rate");
			_meanRateIntervalWindow.PersistenceId = "MeanRateIntervalWindowID";
			var meanRateIntervalView = new MeanRateIntervalView(caliper, this)
			{
				Window = _meanRateIntervalWindow
			};
			_meanRateIntervalWindow.Content = meanRateIntervalView;
			_meanRateIntervalWindow.Closed += OnMeanRateIntervalWindowClosed;
			_meanRateIntervalWindow.Show();
		}

		private static void SetupFloatingWindow(WindowEx window, int width, int height, string title)
		{
			window.Height = window.MinHeight = height;
			window.Width = window.MinWidth = width;
			window.SetIsAlwaysOnTop(true);
			window.CenterOnScreen();
			window.ExtendsContentIntoTitleBar = true;
			// TODO: need to do more work if we want the titlebar to show title and icon, otherwise we
			// just have a blank title bar
			// NOTE the titlebar api only seems to work for the main window of the app, so below doesn't work.
			//Grid titleBar = new Grid()
			//{
			//	Height = 32,
			//	Background = new SolidColorBrush(Colors.CornflowerBlue),
			//	HorizontalAlignment = HorizontalAlignment.Stretch
			//};

			//// Add a text block to the title bar element
			//TextBlock titleText = new TextBlock()
			//{
			//	Text = "Custom Title Bar App",
			//	VerticalAlignment = VerticalAlignment.Center,
			//	Margin = new Thickness(12, 0, 0, 0),
			//	Foreground = new SolidColorBrush(Colors.White)
			//};
			//titleBar.Children.Add(titleText);

			//// Set the custom title bar element as the draggable region of the window
			//window.SetTitleBar(titleBar);
			//window.SetTaskBarIcon(Icon.FromFile("Assets/EpCalipersLargeTemplate1.ico"));
			//window.Title = title;
		}

		public async Task MeasureQtc()
		{
			if (SelectedCaliper == null || SelectedCaliper.CaliperType != CaliperType.Time)
			{
				await ShowMessage("HowToMeasureMeanIntervalTitle".GetLocalized(),
					"HowToMeasureMeanIntervalMessage".GetLocalized());
			}
			else
			{
				// show dialog, get number of interval
				ShowMeasureQtcDialog();
				//var numberIntervals = 5;
				//var meanInterval = Caliper.MeanInterval(SelectedCaliper.Value, numberIntervals);
			}
		}
		public void ShowMeasureQtcDialog()
		{
			Debug.Assert(SelectedCaliper != null);
			if (_measureQtcWindow == null)
			{
				_measureQtcWindow = new WindowEx();
			}
			SetupFloatingWindow(_measureQtcWindow, 400, 400, "Measure QTc");
			//_measureQtcWindow.PersistenceId = "MeanRateIntervalWindowID";
			var qtcView = new QtcView()
			{
				Window = _measureQtcWindow
			};
			Frame frame = new Frame();
			//frame.Content = qtcView;
			frame.Navigate(typeof(QtcView));
			_measureQtcWindow.Content = frame;
			_measureQtcWindow.Closed += OnMeasureQtcWindowClosed;
			_measureQtcWindow.Show();
		}

		public void ShowColorDialog(Point point)
		{
			var caliper = GetCaliperAt(point);
			if (caliper == null) return;
			if (_colorWindow == null)
			{
				_colorWindow = new WindowEx();
			}
			SetupFloatingWindow(_colorWindow, 400, 550, "ColorWindowTitle".GetLocalized());
			_colorWindow.PersistenceId = "ColorWindowID";
			var colorView = new ColorView(caliper)
			{
				Window = _colorWindow
			};
			_colorWindow.Content = colorView;
			_colorWindow.Closed += OnColorWindowClosed;
			_colorWindow.Show();
		}
	}
}

