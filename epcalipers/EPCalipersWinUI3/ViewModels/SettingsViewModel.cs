using CommunityToolkit.Mvvm.ComponentModel;
using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Models;
using EPCalipersWinUI3.Models.Calipers;
using System.ComponentModel;
using Windows.UI;

namespace EPCalipersWinUI3.ViewModels
{
	public partial class SettingsViewModel : ObservableObject
	{
		private readonly ISettings _model = Settings.Instance;

		public SettingsViewModel() {
			AutoAlignLabel = _model.AutoAlignLabel;
			TimeCaliperLabelAlignment = (int)_model.TimeCaliperLabelAlignment;
			AmplitudeCaliperLabelAlignment = (int)_model.AmplitudeCaliperLabelAlignment;
			UnselectedCaliperColor = _model.UnselectedCaliperColor;
			SelectedCaliperColor = _model.SelectedCaliperColor;
			BarThickness = _model.BarThickness;
			Rounding = (int)_model.Rounding;
			ShowBrugadaTriangle = _model.ShowBrugadaTriangle;
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
	}
}
