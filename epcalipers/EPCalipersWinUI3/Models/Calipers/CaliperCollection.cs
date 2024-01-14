using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using WinUIEx;

namespace EPCalipersWinUI3.Models.Calipers
{
	/// <summary>
	/// Maintains a collection of calipers, sets colors, adds, deletes them, etc.
	/// </summary>
	public class CaliperCollection
	{
		private readonly IList<Caliper> _calipers;
		private readonly ICaliperView _caliperView;
		private readonly ISettings _settings;
		private WindowEx _calibrationWindow;

		private Caliper _grabbedCaliper;
		private Bar _grabbedComponent;
		private Point _startingDragPoint;

		public Calibration TimeCalibration { get; set; } = Calibration.Uncalibrated;
		public Calibration AmplitudeCalibration { get; set; } = Calibration.Uncalibrated;

		// A calibrated time caliper can show interval or rate.
		public bool ShowRate { get; set; } = false; 

		public Caliper SelectedCaliper
		{
			get
			{
				foreach (var caliper in _calipers)
				{
					if (caliper.IsSelected)
					{
						return caliper;
					}
				}
				return null;
			}
		}

		public (Bar, Caliper) SelectedBarInCaliper
		{
			get
			{
				foreach (var caliper in _calipers)
				{
					var bar = caliper.IsSelectedBar;
					if (bar != null)
					{
						return (bar, caliper);
					}
				}
				return (null, null);
			}
		}

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
					angleCaliper.TimeCalibration = TimeCalibration;
					angleCaliper.AmplitudeCalibration = AmplitudeCalibration;
					break;
			}
			caliper.ShowRate = ShowRate;
			caliper.UpdateLabel();
			_calipers.Add(caliper);
		}

		public void AddCaliper(CaliperType type)
		{
			if (IsLocked) return;
			var caliper = Caliper.InitCaliper(type, _caliperView, _settings, TimeCalibration, AmplitudeCalibration);
			// TODO: Reduce caliper dependency on CaliperView:
			//  _caliperView.Add(caliper); ???
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

		public void MoveLeft()
		{
			if (IsLocked) return;
			var bar = SelectedBarInCaliper.Item1;
			var caliper = SelectedBarInCaliper.Item2;
			if (bar != null)
			{
				caliper.Drag(bar, new Point(-1.0, 0), new Point(0, 0));
			}
		}

		public void MoveRight()
		{

		}

		public void MoveUp()
		{

		}
		public void MoveDown() 
		{
		}

		public void MicroMoveLeft()
		{
		}

		public void MicroMoveRight()
		{

		}

		public void MicroMoveUp()
		{

		}
		public void MicroMoveDown() 
		{
		}
		public void GrabCaliper(Point point)
		{
			if (IsLocked) return;
			// Detect if this is near a caliper component, and if so, load it up for movement.
			(_grabbedCaliper, _grabbedComponent) = GetGrabbedCaliperAndBar(point);
			_startingDragPoint = point;
		}

		public void DragCaliperComponent(Point point)
		{
			if (_grabbedCaliper == null || _grabbedComponent == null) return;
			var delta = new Point(point.X - _startingDragPoint.X, point.Y - _startingDragPoint.Y);
			_startingDragPoint.X += delta.X;
			_startingDragPoint.Y += delta.Y;
			_grabbedCaliper.Drag(_grabbedComponent, delta, _startingDragPoint);
		}

		public void ReleaseGrabbedCaliper()
		{
			_grabbedCaliper = null;
			_grabbedComponent = null;
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
			_calipers.Clear();
		}

		public void RemoveActiveCaliper()
		{
			if (IsLocked) return;
			foreach (var caliper in _calipers)
			{
				if (caliper.IsSelected)
				{
					caliper.Remove(_caliperView);
					_calipers.Remove(caliper);
					break;  // Can only be one selected caliper, so no point checking the rest.
				}
			}
		}

		public void DeleteCaliperAt(Point point)
		{
			var caliperBar = GetGrabbedCaliperAndBar(point);
			var caliper = caliperBar.Item1 as Caliper;
			if (caliper != null)
			{
				caliper.Remove(_caliperView);
				_calipers.Remove(caliper);
			}
		}

		public void UnselectAllCalipers()
		{
			// TODO: may need to handle Esc for measurements differently.
			if (IsLocked) return;
            foreach (var caliper in _calipers)
			{
				caliper.IsSelected = false;
			}
        }

		public (Caliper, Bar) GetGrabbedCaliperAndBar(Point point)
		{
			Bar bar = null;
			var caliper = _calipers.Where(x => (bar = x.IsNearBar(point)) != null).FirstOrDefault();
			if (caliper == null) return (null, null);
			Debug.Print(caliper.ToString(), bar.ToString());
			return (caliper, bar);
		}

		//public (Caliper, Bar) GetSelectedCaliperAndBar()
		//{
		//	Bar bar = null;
		//}

		public bool PointIsNearCaliper(Point point)
		{
			if (IsLocked) return false;
			foreach (var caliper in _calipers)
			{
				var component = caliper.IsNearBar(point);
				if (component == null) Debug.Print("Null");
				else Debug.Print(component.ToString());
				if (component != null) {
					return true;
				}
			}
			return false;
		}

		public void ToggleCaliperSelection(Point point)
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
						caliper.IsSelected = false;
					}
					else
					{
						caliper.IsSelected = true;
					}
					UnselectCalipersExcept(caliper);
				}
			}
		}

		public void ToggleComponentSelection(Point point)
		{
			if (IsLocked) return;
			bool caliperToggled = false;
			foreach (var caliper in _calipers)
			{
				var bar = caliper.IsNearBar(point);
				if (bar != null && !caliperToggled)
				{
					caliperToggled = true;
					if (bar.IsSelected)
					{
						// Avoid ending up with 1 bar of a caliper unselected.
						caliper.IsSelected = false;
					}
					else
					{
						// Avoid multiple selected bars.
						caliper.IsSelected = false;
						bar.IsSelected = true;
					}
					UnselectCalipersExcept(caliper);
				}
			}
		}

		private void UnselectCalipersExcept(Caliper c)
		{
			if (IsLocked) return;
			foreach (var caliper in _calipers)
			{
				if (caliper != c)
				{
					caliper.IsSelected = false;
				}
			}
		}

		public void RefreshCalipers()
		{
			TimeCalibration.Rounding = _settings.Rounding;
			AmplitudeCalibration.Rounding = _settings.Rounding;
			foreach (var caliper in _calipers)
			{
				caliper.ApplySettings(_settings);
			}
			Debug.Print("refreshing calipers...");
		}

		public async Task SetCalibrationAsync()
		{
			ContentDialog dialog;
			SelectSoleTimeOrAmplitudeCaliper();
			switch (SelectedCaliperType)
			{
				case CaliperType.None:
					dialog = MessageHelper.CreateMessageDialog("NoCaliperSelectedTitle".GetLocalized(),
						"NoCaliperSelectedMessage".GetLocalized());
					dialog.XamlRoot = _caliperView.XamlRoot;
					await dialog.ShowAsync();
					break;
				case CaliperType.Angle:
					dialog = MessageHelper.CreateMessageDialog("AngleCaliperSelectedTitle".GetLocalized(),
						"AngleCaliperSelectedMessage".GetLocalized());
					dialog.XamlRoot = _caliperView.XamlRoot;
					await dialog.ShowAsync();
					break;
				case CaliperType.Time:
				case CaliperType.Amplitude:
					IsLocked = true;
					ShowCalibrationDialogWindow(SelectedCaliperType);
					break;
			}
		}

		private void SelectSoleTimeOrAmplitudeCaliper()
		{
			if (_calipers.Count == 1)
			{ 
				if (_calipers[0].CaliperType == CaliperType.Angle)
				{
					return;  // Don't bother with angle calipers
				}
				if (_calipers[0].IsSelected == false)
				{
					_calipers[0].IsSelected = true;
					return;
				}
			}
			return;  // 
		}

		private void ShowCalibrationDialogWindow(CaliperType caliperType)
		{
			Debug.Assert(SelectedCaliper != null);
			if (_calibrationWindow == null)
			{
				_calibrationWindow = new WindowEx();
			}
			_calibrationWindow.Height = 400;
			_calibrationWindow.Width = 400;
			_calibrationWindow.SetIsAlwaysOnTop(true);
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
			_calibrationWindow.Title = title;
			_calibrationWindow.SetTaskBarIcon(Icon.FromFile("Assets/EpCalipersLargeTemplate1.ico"));
			//Frame frame = new Frame();
			//frame.Navigate(typeof(CalibrationView));
			var calibrationView = new CalibrationView(SelectedCaliper, this)
			{
				Window = _calibrationWindow,
				CaliperType = caliperType
			};
			_calibrationWindow.Content = calibrationView;
			_calibrationWindow.Closed += OnClosed;
			_calibrationWindow.Show();
		}

		private void OnClosed(object sender, WindowEventArgs args)
		{
			IsLocked = false;
			_calibrationWindow.Content = null;
			_calibrationWindow = null;
		}

		public void ClearCalibration()
		{
			TimeCalibration = Calibration.Uncalibrated;
			AmplitudeCalibration = Calibration.Uncalibrated;
			TimeCalibration.Rounding = _settings.Rounding;
			AmplitudeCalibration.Rounding = _settings.Rounding;
			SetCalibration();
		}

		public void SetCalibration()
		{
			TimeCalibration.Rounding = _settings.Rounding;
			AmplitudeCalibration.Rounding = _settings.Rounding;
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
						angleCaliper.TimeCalibration = TimeCalibration;
						angleCaliper.AmplitudeCalibration = AmplitudeCalibration;
						angleCaliper.TriangleBaseLabel.Alignment = _settings.TimeCaliperLabelAlignment;
						angleCaliper.Calibration.Rounding = _settings.Rounding;
						angleCaliper.TriangleBaseLabel.AutoAlignLabel = _settings.AutoAlignLabel;
						angleCaliper.TriangleBaseLabel.Alignment = _settings.TimeCaliperLabelAlignment;
						angleCaliper.DrawTriangleBase();
						break;
				}
				caliper.UpdateLabel();
			}
		}

		public async Task ToggleRateInterval()
		{
			ContentDialog dialog;
			switch (TimeCalibration.Parameters.Unit)
			{
				case CalibrationUnit.Uncalibrated:
					dialog = MessageHelper.CreateMessageDialog("Time calipers not calibrated",
						"You need to calibrate a time caliper before you can measure heart rates.");
					dialog.XamlRoot = _caliperView.XamlRoot;
					await dialog.ShowAsync();
					break;
				case CalibrationUnit.Unknown:
					dialog = MessageHelper.CreateMessageDialog("Time calipers units not correct",
						"Calibration unit needs to be msec or sec to meaure heart rates.");
					dialog.XamlRoot = _caliperView.XamlRoot;
					await dialog.ShowAsync();
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
	}
}

