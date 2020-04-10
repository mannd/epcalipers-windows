using System.Drawing;

namespace EPCalipersCore
{
	public interface ICalipers
	{
		Calibration HorizontalCalibration { get; set; }
		Calibration VerticalCalibration { get; set; }

		int NumberOfCalipers();
		bool NoCaliperIsSelected();
		void SelectSoleCaliper();
		BaseCaliper GetActiveCaliper();
		BaseCaliper GetLoneTimeCaliper();
		void SelectCaliper(BaseCaliper c);
		void UnselectCalipersExcept(BaseCaliper c);
		bool NoTimeCaliperSelected();
		void UnselectChosenCaliper();
		Color GetChosenCaliperColor();
		void SetChosenCaliperColor(Color color);
		void ClearAllChosenComponentsExceptForChosenCaliper();
		void ClearAllChosenComponents();
	}
}
