
using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Calipers;
using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Helpers;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using EPCalipersWinUI3.Views;

namespace EPCalipersWinUI3.ViewModels
{
	public partial class TransparentPageViewModel
	{
		private CaliperCollection _caliperCollection;
		private ICaliperView _caliperView;
		private static double _differential = 10;

		public TransparentPageViewModel(ICaliperView caliperView)
		{
			_caliperCollection = new CaliperCollection(caliperView);
			_caliperView = caliperView;
		}

		[RelayCommand]
		public void AddTimeCaliper()
		{
			var initialPosition = InitialPosition(CaliperType.Time, 200);
			var caliper = new TimeCaliper(initialPosition, _caliperView);
			caliper.SetColor(Colors.Blue);
			caliper.UnselectedColor = Colors.Blue;
			caliper.SelectedColor = Colors.Red;
			_caliperCollection.Add(caliper);
		}


		[RelayCommand]
		public void AddAmplitudeCaliper()
		{
			var initialPosition = InitialPosition(CaliperType.Amplitude, 200);
			var caliper = new AmplitudeCaliper(initialPosition, _caliperView);
			caliper.SetColor(Colors.Blue);
			caliper.UnselectedColor = Colors.Blue;
			caliper.SelectedColor = Colors.Red;
			_caliperCollection.Add(caliper);
		}

		[RelayCommand]
		public void AddAngleCaliper()
		{
			var initialPosition = InitialAnglePosition();
			var caliper = new AngleCaliper(initialPosition, _caliperView);
		//	caliper.SetColor(Colors.Blue);
			caliper.UnselectedColor = Colors.Blue;
			caliper.SelectedColor = Colors.Red;
			_caliperCollection.Add(caliper);
		}

		public void ChangeBounds()
		{
			_caliperCollection.ChangeBounds();
		}

		private CaliperPosition InitialPosition(CaliperType type, double spacing)
		{
			Point p = GetApproximateCenterOfView(_caliperView);
			double halfSpacing = spacing / 2.0;
			switch (type)
			{
				case CaliperType.Time:
					return new CaliperPosition(p.Y, p.X - halfSpacing, p.X + halfSpacing);
				case CaliperType.Amplitude:
					return new CaliperPosition(p.X, p.Y - halfSpacing, p.Y + halfSpacing);
				default:
					return new CaliperPosition(0, 0, 0);
			}
		}

		private AngleCaliperPosition InitialAnglePosition()
		{
			var apex = GetApproximateCenterOfView(_caliperView);
			double firstAngle = 0.5 * Math.PI;
			double secondAngle = 0.25 * Math.PI;
			return new AngleCaliperPosition(apex, firstAngle, secondAngle);
		}

		private Point GetApproximateCenterOfView(ICaliperView view)
		{
			// Get centerpoint of CaliperView.
			Point center = new Point((view.Bounds.Width / 2.0) + _differential, (view.Bounds.Height / 2.0) + _differential);
			_differential += 10;
			if (_differential > 100) _differential = 0;
			return center;
		}

		[RelayCommand]
		private static void ToggleTransparentWindow()
		{
			var mainWindow = (Application.Current as App)?.Window as MainWindow;
            mainWindow.Navigate(typeof(MainPage));

		}


		[RelayCommand]
		private static void Exit() => CommandHelper.ApplicationExit();
	}
}
