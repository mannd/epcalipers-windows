using EPCalipersWinUI3.Models;
using EPCalipersWinUI3.Models.Calipers;
using System.Reflection.Metadata;
using Windows.UI;
using static EPCalipersWinUI3.Helpers.MathHelper;

namespace EPCalipersWinUI3.Contracts
{
	public interface ISettings
	{
		public double BarThickness { get; set; }
		public int FontSize { get; set; }
		public bool AutoAlignLabel { get; set; }
		public CaliperLabelAlignment TimeCaliperLabelAlignment { get; set; }
		public CaliperLabelAlignment AmplitudeCaliperLabelAlignment { get; set; }
		public Color UnselectedCaliperColor { get; set; }
		public Color SelectedCaliperColor { get; set; }
		public Rounding Rounding { get; set; }
		public bool ShowBrugadaTriangle { get; set; }
		public int NumberOfMeanIntervals { get; set; }
		public int NumberOfRRIntervals { get; set; }
		public QtcFormula QtcFormula { get; set; }
		public int SelectedTimeCalibrationRadioButton { get; set; }
		public int SelectedAmplitudeCalibrationRadioButton {  get; set; }
		public string CustomTimeCalibration {  get; set; }
		public string CustomAmplitudeCalibration { get; set; }
		public int NumberOfMarchingCalipers { get; set; }
		public bool ShowSampleEcgAtStartUp {  get; set; }
		public bool AdjustBarThicknessWithZoom { get; set; }
		public bool AdjustCaliperLabelSizeWithZoom { get; set; }
	}
}
