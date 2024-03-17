using CommunityToolkit.Mvvm.ComponentModel;
using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Models;
using EPCalipersWinUI3.Models.Calipers;
using System.ComponentModel;
using Windows.UI;

namespace EPCalipersWinUI3.ViewModels
{
	public partial class SettingsViewModel : BasePageViewModel
	{
		private readonly ISettings _model = Settings.Instance;

		public SettingsViewModel()
		{
			AutoAlignLabel = _model.AutoAlignLabel;
			TimeCaliperLabelAlignment = (int)_model.TimeCaliperLabelAlignment;
			AmplitudeCaliperLabelAlignment = (int)_model.AmplitudeCaliperLabelAlignment;
			UnselectedCaliperColor = _model.UnselectedCaliperColor;
			SelectedCaliperColor = _model.SelectedCaliperColor;
			BarThickness = _model.BarThickness;
			Rounding = (int)_model.Rounding;
			ShowBrugadaTriangle = _model.ShowBrugadaTriangle;
			NumberOfMarchingCalipers = _model.NumberOfMarchingCalipers;
			CaliperLabelSize = CaliperLabelConvertFromSize(_model.CaliperLabelSize);
			ShowSampleEcgAtStartUp = _model.ShowSampleEcgAtStartUp;
			AdjustBarThicknessWithZoom = _model.AdjustBarThicknessWithZoom;
		}

		// TODO: Nicer to just use an indexed array to do this conversion.
		private int CaliperLabelConvertFromSize(CaliperLabelSize size)
		{
			switch (size)
			{
				case Models.Calipers.CaliperLabelSize.ExtraSmall: return 0;
				case Models.Calipers.CaliperLabelSize.Small: return 1;
				case Models.Calipers.CaliperLabelSize.Medium: return 2;
				case Models.Calipers.CaliperLabelSize.Large: return 3;
				case Models.Calipers.CaliperLabelSize.ExtraLarge: return 4;
			}
			return 2;  // Medium
		}

		private CaliperLabelSize CaliperLabelConvertToSize(int n)
		{
			switch (n)
			{
				case 0: return Models.Calipers.CaliperLabelSize.ExtraSmall;
				case 1: return Models.Calipers.CaliperLabelSize.Small;
				case 2: return Models.Calipers.CaliperLabelSize.Medium;
				case 3: return Models.Calipers.CaliperLabelSize.Large;
				case 4: return Models.Calipers.CaliperLabelSize.ExtraLarge;
			}
			return Models.Calipers.CaliperLabelSize.Medium;
		}

		protected override void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			switch (e.PropertyName)
			{
				case nameof(TimeCaliperLabelAlignment):
					_model.TimeCaliperLabelAlignment = (CaliperLabelAlignment)TimeCaliperLabelAlignment;
					break;
				case nameof(AmplitudeCaliperLabelAlignment):
					_model.AmplitudeCaliperLabelAlignment = (CaliperLabelAlignment)AmplitudeCaliperLabelAlignment;
					break;
				case nameof(UnselectedCaliperColor):
					_model.UnselectedCaliperColor = UnselectedCaliperColor;
					break;
				case nameof(SelectedCaliperColor):
					_model.SelectedCaliperColor = SelectedCaliperColor;
					break;
				case nameof(BarThickness):
					_model.BarThickness = BarThickness;
					break;
				case nameof(AutoAlignLabel):
					_model.AutoAlignLabel = AutoAlignLabel;
					break;
				case nameof(Rounding):
					_model.Rounding = (Rounding)Rounding;
					break;
				case nameof(ShowBrugadaTriangle):
					_model.ShowBrugadaTriangle = ShowBrugadaTriangle;
					break;
				case nameof(NumberOfMarchingCalipers):
					_model.NumberOfMarchingCalipers = NumberOfMarchingCalipers;
					break;
				case nameof(CaliperLabelSize):
					_model.CaliperLabelSize = CaliperLabelConvertToSize(CaliperLabelSize);
					break;
				case nameof(ShowSampleEcgAtStartUp):
					_model.ShowSampleEcgAtStartUp = ShowSampleEcgAtStartUp;
					break;
				case nameof(AdjustBarThicknessWithZoom):
					_model.AdjustBarThicknessWithZoom = AdjustBarThicknessWithZoom;
					break;
				default:
					break;
			}
		}

		[ObservableProperty]
		private bool autoAlignLabel;

		[ObservableProperty]
		private int timeCaliperLabelAlignment;

		[ObservableProperty]
		private int amplitudeCaliperLabelAlignment;

		[ObservableProperty]
		private Color unselectedCaliperColor;

		[ObservableProperty]
		private Color selectedCaliperColor;

		[ObservableProperty]
		private double barThickness;

		[ObservableProperty]
		private int rounding;

		[ObservableProperty]
		private bool showBrugadaTriangle;

		[ObservableProperty]
		private int numberOfMarchingCalipers;

		[ObservableProperty]
		private int caliperLabelSize;

		[ObservableProperty]
		private bool showSampleEcgAtStartUp;

		[ObservableProperty]
		private bool adjustBarThicknessWithZoom;
	}
}
