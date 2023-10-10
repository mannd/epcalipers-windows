using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments.AppointmentsProvider;
using Windows.Foundation;

namespace EPCalipersWinUI3.Calipers
{
	/// <summary>
	/// Maintains a collection of calipers, sets colors, adds, deletes them, etc.
	/// </summary>
	public class CaliperCollection
	{
		private IList<Caliper> _caliperCollection;
		private Grid _grid;

		public CaliperCollection(Grid grid)
		{ 
			_caliperCollection = new List<Caliper>();
			_grid = grid;
		}

		public void Add(Caliper caliper)
		{
			caliper.Add(_grid);
			_caliperCollection.Add(caliper);
		}

		public void RemoveAtPoint(Point point)
		{
			foreach (var caliper in _caliperCollection)
			{
				var component = caliper.IsNearComponent(point);
				if (component != null)
				{
					caliper.Delete(_grid);
					_caliperCollection.Remove(caliper);
					break;
				}
			}
		}

		public void Clear()
		{
			foreach (var caliper in _caliperCollection)
			{
				caliper.Delete(_grid);
			}
			_caliperCollection.Clear();
		}

		public void ToggleCaliperSelection(Point point)
		{
			foreach (var caliper in _caliperCollection)
			{
				var component = caliper.IsNearComponent(point);
				if (component != null)
				{
					// TODO: has to be handled in _calipers, because need to clear selected
					// calipers first.
					caliper.ToggleIsSelected();
					break;
				}
			}
		}
	}
}
