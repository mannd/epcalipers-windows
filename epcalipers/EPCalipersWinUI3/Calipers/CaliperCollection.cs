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

namespace EPCalipersWinUI3.Calipers
{
	/// <summary>
	/// Maintains a collection of calipers, sets colors, adds, deletes them, etc.
	/// </summary>
	public class CaliperCollection
	{
		private IList<Caliper> _calipers;
		private ICaliperView _caliperView;

		public CaliperCollection(ICaliperView caliperView)
		{ 
			_calipers = new List<Caliper>();
			_caliperView = caliperView;
		}

		public IList<Caliper> FilteredCalipers(CaliperType caliperType) 
			=> _calipers.Where(x => x.CaliperType == caliperType).ToList();

		public void Add(Caliper caliper)
		{
			caliper.Add(_caliperView);
			_calipers.Add(caliper);
		}

		public void RemoveAtPoint(Point point)
		{
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
			foreach (var caliper in _calipers)
			{
				caliper.Remove(_caliperView);
			}
			_calipers.Clear();
		}

		public void RemoveActiveCaliper()
		{
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
			bool caliperToggled = false;
			foreach (var caliper in _calipers)
			{
				var bar = caliper.IsNearBar(point);
				if (bar != null && !caliperToggled)
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
				}
				else
				{
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
	}
}
