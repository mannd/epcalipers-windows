using EPCalipersWinUI3.Models;
using EPCalipersWinUI3.Models.Calipers;
using Windows.UI;
using static EPCalipersWinUI3.Helpers.MathHelper;

namespace EPCalipersWinUI3.Contracts
{
	public interface ISettings
	{
		public double BarThickness { get; set; }
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
	}
}
