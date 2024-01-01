﻿using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Models;
using EPCalipersWinUI3.Models.Calipers;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace EPCalipersWinUI3.Helpers
{
    public class CaliperHelper
	{
		private ICaliperView _caliperView;
		private CaliperCollection _caliperCollection;
		private Caliper _grabbedCaliper;
		private Bar _grabbedComponent;
		private Point _startingDragPoint;

		public CaliperHelper(ICaliperView caliperView, CaliperCollection caliperCollection)
		{
			_caliperView = caliperView;
			_caliperCollection = caliperCollection;
			Calibration.DefaultUnit = "points".GetLocalized();
			Calibration.DefaultBpm = "bpm".GetLocalized();
		}

		public void AddTimeCaliper(ISettings settings)
		{
			_caliperCollection.AddCaliper(CaliperType.Time);
		}

		public void AddAmplitudeCaliper(ISettings settings)
		{
			_caliperCollection.AddCaliper(CaliperType.Amplitude);
		}

		public void AddAngleCaliper(ISettings settings)
		{
			_caliperCollection.AddCaliper(CaliperType.Angle);
		}

		public void RefreshCalipers(ISettings settings)
		{
			_caliperCollection.RefreshCalipers(settings);
		}

		public void DeleteAllCalipers()
		{
			_caliperCollection.Clear();
		}

		public void DeleteSelectedCaliper()
		{
			_caliperCollection.RemoveActiveCaliper();
		}

		public void ToggleCaliperSelection(Point point)
		{
			_caliperCollection.ToggleCaliperSelection(point);
		}
		public void RemoveAtPoint(Point point)
		{
			_caliperCollection.RemoveAtPoint(point);
		}

		public void GrabCaliper(Point point)
		{
			// Detect if this is near a caliper component, and if so, load it up for movement.
			(_grabbedCaliper, _grabbedComponent) = _caliperCollection.GetGrabbedCaliperAndBar(point);
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
			_caliperCollection.ChangeBounds();
		}

		public CaliperType GetSelectedCaliperType()
		{
			return _caliperCollection.SelectedCaliperType;
		}

		public async Task SetCalibrationAsync()
		{
			await _caliperCollection.SetCalibrationAsync();
		}

		public void ClearCalibration()
		{
			_caliperCollection.ClearCalibration();
		}

		public async Task ToggleRateInterval()
		{
			await _caliperCollection.ToggleRateInterval();

		}
	}
}

