using EPCalipersWinUI3.Models;
using EPCalipersWinUI3.Models.Calipers;
using System.Reflection.Metadata;
using Windows.UI;
using static EPCalipersWinUI3.Helpers.MathHelper;
using EPCalipersWinUI3.Views;

namespace EPCalipersWinUI3.Contracts
{ 
	public interface ISettings
	{
		public double BarThickness { get; set; }
		public PDFHandler.PdfResolution PdfResolution { get; set; }
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
		public CaliperViewAlignment CaliperViewAlignment { get; set; }
		public StartupPage StartupPage { get; set; }
		public bool IsAlwaysOnTop { get; set; }
		public bool RecalibrateBetweenPdfPages { get; set; }
		public bool ResetZoomBetweenPdfPages { get; set; }
		public bool ResetRotationBetweenPdfPages { get; set; }
		public bool ClearCalipersBetweenPdfPages { get; set; }
	}
}
