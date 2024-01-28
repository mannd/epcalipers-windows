using System.Drawing;

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
