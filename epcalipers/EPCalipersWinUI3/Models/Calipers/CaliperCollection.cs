using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments.AppointmentsProvider;
using Windows.Foundation;
using EPCalipersWinUI3.Contracts;
using System.Diagnostics;
using EPCalipersWinUI3.Models;
using EPCalipersWinUI3.Helpers;
using Microsoft.UI.Xaml;
using EPCalipersWinUI3.Views;
using Microsoft.UI.Xaml.Hosting;
using WinUIEx;

namespace EPCalipersWinUI3.Models.Calipers
{
	/// <summary>
	/// Maintains a collection of calipers, sets colors, adds, deletes them, etc.
	/// </summary>
	public class CaliperCollection
	{
		private IList<Caliper> _calipers;
		private ICaliperView _caliperView;

		public ICalibration TimeCalibration { get; set; }
		public ICalibration AmplitudeCalibration { get; set; }

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

		public CaliperType SelectedCaliperType => SelectedCaliper?.CaliperType ?? CaliperType.None;

		/// <summary>
		/// If the caliper collection is locked, then calipers can not be added, deleted, selected or
		/// unselected.  They can be moved, however.  This allows calibration to focus on one caliper.
		/// </summary>
		public bool IsLocked { get; set; }

		public CaliperCollection(ICaliperView caliperView)
		{
			_calipers = new List<Caliper>();
			_caliperView = caliperView;
		}

		public IList<Caliper> FilteredCalipers(CaliperType caliperType)
			=> _calipers.Where(x => x.CaliperType == caliperType).ToList();

		public void Add(Caliper caliper)
		{
			if (IsLocked) return;
			caliper.Add(_caliperView);
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

		public (Caliper, Bar) GetGrabbedCaliperAndBar(Point point)
		{
			Bar bar = null;
			var caliper = _calipers.Where(x => (bar = x.IsNearBar(point)) != null).FirstOrDefault();
			if (caliper == null) return (null, null);
			Debug.Print(caliper.ToString(), bar.ToString());
			return (caliper, bar);
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

		private void UnselectCalipersExcept(Caliper c)
		{
			if (IsLocked) return;
			foreach (var caliper in _calipers)
			{
				if (caliper != c)
				{
					// NB.  c.IsSelected = false doesn't work.  
					// Not sure why? Maybe because we are passing it a variable
					// from a foreach loop (in ToggleCaliperSelection()).
					caliper.IsSelected = false;
				}
			}
		}

		public void RefreshCalipers(ISettings settings)
		{
			foreach (var caliper in _calipers)
			{
				caliper.ApplySettings(settings);
			}
		}

		// TODO: possibly move this to CaliperHelper
		public async Task SetCalibrationAsync(XamlRoot xamlRoot)
		{
			ContentDialog dialog;
			switch (SelectedCaliperType)
			{
				case CaliperType.None:
					dialog = MessageDialog.Create("NoCaliperSelectedTitle".GetLocalized(),
						"NoCaliperSelectedMessage".GetLocalized());
					dialog.XamlRoot = xamlRoot;
					await dialog.ShowAsync();
					break;
				case CaliperType.Angle:
					dialog = MessageDialog.Create("AngleCaliperSelectedTitle".GetLocalized(),
						"AngleCaliperSelectedMessage".GetLocalized());
					dialog.XamlRoot = xamlRoot;
					await dialog.ShowAsync();
					break;
				case CaliperType.Time:
					IsLocked = true;
					ShowCalibrationDialogWindow(CaliperType.Time);
					break;
				case CaliperType.Amplitude:
					IsLocked = true;
					ShowCalibrationDialogWindow(CaliperType.Amplitude);
					break;
			}
		}

		private void ShowCalibrationDialogWindow(CaliperType caliperType)
		{
			WindowEx calibrationWindow = new WindowEx();
			calibrationWindow.Height = 400;
			calibrationWindow.Width = 400;
			calibrationWindow.SetIsAlwaysOnTop(true);
			//var title = string.Format("{0] caliper calibration")
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
			calibrationWindow.Title = title;
			calibrationWindow.SetTaskBarIcon(Icon.FromFile("Assets/EpCalipersLargeTemplate1.ico"));
			//Frame frame = new Frame();
			//frame.Navigate(typeof(CalibrationView));
			var calibrationView = new CalibrationView(caliperType);
			calibrationView.Window = calibrationWindow;
			calibrationView.CaliperCollection = this;
			calibrationView.CaliperType = caliperType;
			calibrationWindow.Content = calibrationView;
			calibrationWindow.Closed += OnClosed;
			calibrationWindow.Show();
		}

		private void OnClosed(object sender, WindowEventArgs args)
		{
			IsLocked = false;
		}

		public void ClearCalibration()
		{
			foreach (var caliper in _calipers)
			{
				caliper.ClearCalibration();
			}
		}
	}
}

