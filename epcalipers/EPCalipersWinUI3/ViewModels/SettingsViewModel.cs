using CommunityToolkit.Mvvm.ComponentModel;
using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models;
using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.Views;
using Microsoft.Windows.ApplicationModel.DynamicDependency;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI;

namespace EPCalipersWinUI3.ViewModels
{
	public partial class SettingsViewModel : BasePageViewModel
	{
		private readonly ISettings _model = Settings.Instance;

		private readonly List<int> _fontSizes = new List<int>
		{
			CaliperLabel.ExtraSmallFont,
			CaliperLabel.SmallFont,
			CaliperLabel.MediumFont,
			CaliperLabel.LargeFont,
			CaliperLabel.ExtraLargeFont
		};

		public SettingsViewModel()
		{
			var pdfHelper = new EPCalipersPdfCore.PdfHelper();
			SupportsPdfs = pdfHelper.SupportsPdfs;

			AutoAlignLabel = _model.AutoAlignLabel;
			TimeCaliperLabelAlignment = (int)_model.TimeCaliperLabelAlignment;
			AmplitudeCaliperLabelAlignment = (int)_model.AmplitudeCaliperLabelAlignment;
			UnselectedCaliperColor = _model.UnselectedCaliperColor;
			SelectedCaliperColor = _model.SelectedCaliperColor;
			BarThickness = _model.BarThickness;
			Rounding = (int)_model.Rounding;
			PdfResolution = (int)_model.PdfResolution;
			ShowBrugadaTriangle = _model.ShowBrugadaTriangle;
			NumberOfMarchingCalipers = _model.NumberOfMarchingCalipers;
			FontSize =_model.FontSize;
			ShowSampleEcgAtStartUp = _model.ShowSampleEcgAtStartUp;
			AdjustBarThicknessWithZoom = _model.AdjustBarThicknessWithZoom;
			AdjustCaliperLabelSizeWithZoom = _model.AdjustCaliperLabelSizeWithZoom;
			CaliperViewAlignment = (int)_model.CaliperViewAlignment;
			StartupPage = (int)_model.StartupPage;
			IsAlwaysOnTop = _model.IsAlwaysOnTop;
			RecalibrateBetweenPdfPages = _model.RecalibrateBetweenPdfPages;
			ResetZoomBetweenPdfPages = _model.ResetZoomBetweenPdfPages;
			ResetRotationBetweenPdfPages = _model.ResetRotationBetweenPdfPages;
			ClearCalipersBetweenPdfPages = _model.ClearCalipersBetweenPdfPages;
			DefaultNoteFontSize = _model.DefaultNoteFontSize;
			DefaultNoteForegroundColor = _model.DefaultNoteForegroundColor;
			DefaultNoteWidth = _model.DefaultNoteWidth;
			DefaultNoteHeight = _model.DefaultNoteHeight;

			ViewItems = new List<string>() { "MainWindow".GetLocalized(), "TransparentWindow".GetLocalized() };
			Alignments = new List<string>() 
			{
				"Top".GetLocalized(),
				"Bottom".GetLocalized(),
				"Left".GetLocalized(),
				"Right".GetLocalized() 
			};
			RoundingItems = new List<string>()
			{
				"Integer".GetLocalized(),
				"FourPlaces".GetLocalized(),
				"Tenths".GetLocalized(),
				"Hundredths".GetLocalized(),
				"NoRounding".GetLocalized()
			};
			FontSizeItems = new List<string>()
			{
					"ExtraSmall".GetLocalized(),
					"Small".GetLocalized(),
					"Medium".GetLocalized(),
					"Large".GetLocalized(),
					"ExtraLarge".GetLocalized()
			};
			PdfResolutionItems = new List<string>()
			{
				"Low".GetLocalized(),
				"High".GetLocalized()
			};
			ImagePositionItems = new List<string>()
			{
				"TopLeft".GetLocalized(),
				"Center".GetLocalized()
			};
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
				case nameof(PdfResolution):
					_model.PdfResolution = (EPCalipersPdf.PdfResolution)PdfResolution;
					break;
				case nameof(FontSize):
					_model.FontSize = FontSize;
					break;
				case nameof(ShowSampleEcgAtStartUp):
					_model.ShowSampleEcgAtStartUp = ShowSampleEcgAtStartUp;
					break;
				case nameof(AdjustBarThicknessWithZoom):
					_model.AdjustBarThicknessWithZoom = AdjustBarThicknessWithZoom;
					break;
				case nameof(AdjustCaliperLabelSizeWithZoom):
					_model.AdjustCaliperLabelSizeWithZoom = AdjustCaliperLabelSizeWithZoom;
					break;
				case nameof(CaliperViewAlignment):
					_model.CaliperViewAlignment = (CaliperViewAlignment)CaliperViewAlignment;
					break;
				case nameof(StartupPage):
					_model.StartupPage = (StartupPage)StartupPage;
					break;
				case nameof(IsAlwaysOnTop):
					_model.IsAlwaysOnTop = IsAlwaysOnTop;
					break;
				case nameof(RecalibrateBetweenPdfPages):
					_model.RecalibrateBetweenPdfPages = RecalibrateBetweenPdfPages;
					break;
				case nameof(ResetZoomBetweenPdfPages):
					_model.ResetZoomBetweenPdfPages = ResetZoomBetweenPdfPages;
					break;
				case nameof(ResetRotationBetweenPdfPages):
					_model.ResetRotationBetweenPdfPages = ResetRotationBetweenPdfPages;
					break;
				case nameof(ClearCalipersBetweenPdfPages):
					_model.ClearCalipersBetweenPdfPages = ClearCalipersBetweenPdfPages;
					break;
				case nameof(DefaultNoteFontSize):
					_model.DefaultNoteFontSize = DefaultNoteFontSize;
					break;
				case nameof(DefaultNoteForegroundColor):
					_model.DefaultNoteForegroundColor = DefaultNoteForegroundColor;
					break;
				case nameof(DefaultNoteWidth):
					_model.DefaultNoteWidth = DefaultNoteWidth;
					break;
				case nameof(DefaultNoteHeight):
					_model.DefaultNoteHeight = DefaultNoteHeight;
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
		private int pdfResolution;

		[ObservableProperty]
		private bool showBrugadaTriangle;

		[ObservableProperty]
		private int numberOfMarchingCalipers;

		[ObservableProperty]
		private int caliperLabelSize;

		[ObservableProperty]
		private int fontSize;

		[ObservableProperty]
		private bool showSampleEcgAtStartUp;

		[ObservableProperty]
		private bool adjustBarThicknessWithZoom;

		[ObservableProperty]
		private bool adjustCaliperLabelSizeWithZoom;

		[ObservableProperty]
		private int caliperViewAlignment;

		[ObservableProperty]
		private int startupPage;

		[ObservableProperty]
		private bool isAlwaysOnTop;

		[ObservableProperty]
		private bool recalibrateBetweenPdfPages;

		[ObservableProperty]
		private bool resetZoomBetweenPdfPages;

		[ObservableProperty]
		private bool resetRotationBetweenPdfPages;

		[ObservableProperty]
		private bool clearCalipersBetweenPdfPages;

		[ObservableProperty]
		private List<string> viewItems;

		[ObservableProperty]
		private List<string> alignments;

		[ObservableProperty]
		private List<string> roundingItems;

		[ObservableProperty]
		private List<string> pdfResolutionItems;

		[ObservableProperty]
		private List<string> fontSizeItems;

		[ObservableProperty]
		private List<string> imagePositionItems;

		[ObservableProperty]
		private bool supportsPdfs;

		[ObservableProperty]
		private double defaultNoteFontSize;

		[ObservableProperty]
		private Color defaultNoteForegroundColor;

		[ObservableProperty]
		private double defaultNoteWidth;

		[ObservableProperty]
		private double defaultNoteHeight;
	}
}
