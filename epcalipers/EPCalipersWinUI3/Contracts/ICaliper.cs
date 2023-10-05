using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.Contracts
{
	public interface ICaliper
	{
		public void ToggleIsSelected();
		/// <summary>
		/// Calipers have to be drawn, but the View provides the details on how to do it.
		/// </summary>
		public void Draw();
		public bool PointIsNearCaliper(Point p);
	}
}
