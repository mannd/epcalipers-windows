using EPCalipersWinUI3.Contracts;
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
		private readonly CaliperCollection _caliperCollection;
		private Caliper _grabbedCaliper;
		private Bar _grabbedComponent;
		private Point _startingDragPoint;

		public CaliperHelper(CaliperCollection caliperCollection)
		{
			_caliperCollection = caliperCollection;
			Calibration.DefaultUnit = "points".GetLocalized();
			Calibration.DefaultBpm = "bpm".GetLocalized();
		}

		public bool PointIsNearCaliper(Point point)
		{
			return _caliperCollection.PointIsNearCaliper(point);
		}

		public void AddTimeCaliper()
		{
			_caliperCollection.AddCaliper(CaliperType.Time);
		}

		public void AddAmplitudeCaliper()
		{
			_caliperCollection.AddCaliper(CaliperType.Amplitude);
		}

		public void AddAngleCaliper()
		{
			_caliperCollection.AddCaliper(CaliperType.Angle);
		}

		public void RefreshCalipers()
		{
			_caliperCollection.RefreshCalipers();
		}

		public void DeleteAllCalipers()
		{
			_caliperCollection.Clear();
		}

		public void DeleteSelectedCaliper()
		{
			_caliperCollection.RemoveActiveCaliper();
		}

		public void DeleteCaliperAt(Point point)
		{
			_caliperCollection.DeleteCaliperAt(point);

		}

		public void UnselectAllCalipers()
		{
			_caliperCollection.UnselectAllCalipers();
		}

		public void ToggleCaliperSelection(Point point)
		{
			_caliperCollection.ToggleCaliperSelection(point);
		}

		public void ToggleComponentSelection(Point point)
		{
			_caliperCollection.ToggleComponentSelection(point);
		}
		public void RemoveAtPoint(Point point)
		{
			_caliperCollection.RemoveAtPoint(point);
		}

		public void MoveLeft()
		{
			_caliperCollection.MoveLeft();
			//_caliperCollection.Move(Bar bar, MovementDirection direction, double distance);
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
			_caliperCollection.MoveLeft();
			//_caliperCollection.MicroMove(Bar bar, MicroMovementDirection direction, double distance);
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

