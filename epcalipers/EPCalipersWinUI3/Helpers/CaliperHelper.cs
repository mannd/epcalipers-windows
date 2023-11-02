﻿using EPCalipersWinUI3.Calipers;
using EPCalipersWinUI3.Contracts;
using Microsoft.UI;
using System;
using System.Collections.Generic;
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
		}

		public void AddTimeCaliper()
		{
			var caliper = Caliper.InitCaliper(CaliperType.Time, _caliperView);
			FinalizeCaliper(caliper);
		}

		public void AddAmplitudeCaliper()
		{
			var caliper = Caliper.InitCaliper(CaliperType.Amplitude, _caliperView);
			FinalizeCaliper(caliper);
		}

		public void AddAngleCaliper()
		{
			var caliper = Caliper.InitCaliper(CaliperType.Angle, _caliperView);
			FinalizeCaliper(caliper);
		}

		private void FinalizeCaliper(Caliper c)
		{
			if (c == null) return;
			c.UnselectedColor = Colors.Blue;
			c.SelectedColor = Colors.Red;
			c.IsSelected = false;
			_caliperCollection.Add(c);
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
	}
}
