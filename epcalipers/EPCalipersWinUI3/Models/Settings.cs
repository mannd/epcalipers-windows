using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.Views;
using Microsoft.UI;
using System;
using System.Diagnostics;
using Windows.Storage;
using Windows.UI;
using static EPCalipersWinUI3.Helpers.MathHelper;

namespace EPCalipersWinUI3.Models
{
    public enum Rounding
	{
		ToInt,
		ToFourPlaces,
		ToTenths,
		ToHundredths,
		None
	}

	public enum StartupPage
	{
		Main,
		Transparent
	}

	public sealed class Settings : ISettings
	{
		private readonly ApplicationDataContainer _localSettings;
		private const string _autoAlignLabelKey = "AutoAlignLabel";
		private const string _timeCaliperLabelAlignmentKey = "TimeCaliperLabelAlignmentKey";
		private const string _amplitudeCaliperLabelAlignmentKey = "AmplitudeCaliperLabelAlignmentKey";
		private const string _unselectedCaliperColorKey = "UnselectedCaliperColorKey";
		private const string _selectedCaliperColorKey = "SelectedCaliperColorKey";
		private const string _barThicknessKey = "BarThicknessKey";
		private const string _roundingKey = "RoundingKey";
		private const string _showBrugadaTriangleKey = "ShowBrugadaTriangle";
		private const string _newLabelFontSizeKey = "LabelFontSize";
		private const string _showSampleEcgAtStartUpKey = "ShowSampleEcgAtStartUp";
		private const string _adjustBarThicknessWithZoomKey = "AdjustBarThicknessWithZoom";
		private const string _adjustCaliperLabelSizeWithZoomKey = "AdjustCaliperLabelSize";
		private const string _caliperViewAlignmentKey = "CaliperViewAlignment";
		private const string _startupPageKey = "StartupPage";
		private const string _isAlwaysOnTopKey = "IsAlwaysOnTop";
		private const string _recalibrateBetweenPdfPagesKey = "RecalibrateBetweenPdfPages";
		private const string _resetZoomBetweenPdfPagesKey = "ResetZoomBetweenPdfPages";
		private const string _resetRotationBetweenPdfPagesKey = "ResetRotationBetweenPdfPages";
		private const string _clearCalipersBetweenPdfPagesKey = "ClearCalipersBetweenPdfPages";
		private const string _pdfResolutionKey = "PdfResolution";
		private const string _defaultNoteFontSizeKey = "DefaultNoteFontSize";
		private const string _defaultNoteForegroundColorKey = "DefaultNoteForegroundColor";
		private const string _defaultNoteWidthKey = "DefaultNoteWidth";
		private const string _defaultNoteHeightKey = "DefaultNoteHeight";
		private const string _allowNegativeCaliperValuesKey = "AllowNegativeCaliperValues";

		// Saved parameters not set directly by the user.
		private const string _numberOfMeanIntervalsKey = "NumberOfMeanIntervals";
		private const string _numberOfRRIntervalsKey = "NumberOfRRIntervals";
		private const string _qtcFormulaKey = "QTcFormula";
		private const string _selectedAmplitudeCalibrationRadioButtonKey = "SelectedAmplitudeCalibrationRadioButton";
		private const string _selectedTimeCalibrationRadioButtonKey = "SelectedTimeCalibrationRadioButton";
		private const string _customTimeCalibrationKey = "CustomTimeCalibration";
		private const string _customAmplitudeCalibrationKey = "CustomAmplitudeCalibration";
		private const string _numberOfMarchingCalipersKey = "NumberOfMarchingCalipers";

		private Settings()
		{
			_localSettings = ApplicationData.Current.LocalSettings;
		}

		private static readonly Lazy<Settings> lazy = new(() => new Settings());

		public static Settings Instance { get { return lazy.Value; } }

		public int NumberOfMeanIntervals
		{
			get => (int)(_localSettings.Values[_numberOfMeanIntervalsKey] ?? 3);
			set
			{
				_localSettings.Values[_numberOfMeanIntervalsKey] = value;
				Debug.Print($"numberOfMeanIntervals = {value}");
			} 
		}

		public EPCalipersPdf.PdfResolution PdfResolution
		{
			get => (EPCalipersPdf.PdfResolution)(_localSettings.Values[_pdfResolutionKey] 
				?? EPCalipersPdf.PdfResolution.High);
			set => _localSettings.Values[_pdfResolutionKey] = (int)value;
		}

		public int FontSize
		{
			get => (int)(_localSettings.Values[_newLabelFontSizeKey] ?? CaliperLabel.MediumFont);
			//set => _localSettings.Values[_newLabelFontSizeKey] = (int)value;
			set
			{
				_localSettings.Values[_newLabelFontSizeKey] = (int)value;
				Debug.Print("Font size = {0}", value);
			}
		}

		public int NumberOfRRIntervals
		{
			get => (int)(_localSettings.Values[_numberOfRRIntervalsKey] ?? 1);
			set => _localSettings.Values[_numberOfRRIntervalsKey] = value;
		}
		public int NumberOfMarchingCalipers
		{
			get => (int)(_localSettings.Values[_numberOfMarchingCalipersKey] ?? 10);
			set => _localSettings.Values[_numberOfMarchingCalipersKey] = value;
		}

		public int SelectedTimeCalibrationRadioButton
		{
			get => (int)(_localSettings.Values[_selectedTimeCalibrationRadioButtonKey] ?? 1);
			set => _localSettings.Values[_selectedTimeCalibrationRadioButtonKey] = value;
		}

		public int SelectedAmplitudeCalibrationRadioButton
		{
			get => (int)(_localSettings.Values[_selectedAmplitudeCalibrationRadioButtonKey] ?? 1);
			set => _localSettings.Values[_selectedAmplitudeCalibrationRadioButtonKey] = value;
		}

		public string CustomTimeCalibration
		{
			get => (string)_localSettings.Values[_customTimeCalibrationKey] ?? string.Empty;
			set => _localSettings.Values[_customTimeCalibrationKey ] = value;
		}
		public string CustomAmplitudeCalibration
		{
			get => (string)_localSettings.Values[_customAmplitudeCalibrationKey] ?? string.Empty;
			set => _localSettings.Values[_customAmplitudeCalibrationKey ] = value;
		}

		public QtcFormula QtcFormula
		{
			get => (QtcFormula)(_localSettings.Values[_qtcFormulaKey] ?? QtcFormula.qtcBzt);
			set => _localSettings.Values[_qtcFormulaKey] = (int)value;
		}

		public CaliperViewAlignment CaliperViewAlignment
		{
			get => (CaliperViewAlignment)(_localSettings.Values[_caliperViewAlignmentKey] ?? CaliperViewAlignment.TopLeft);
			set => _localSettings.Values[_caliperViewAlignmentKey ] = (int)value;
		}

		public double BarThickness
		{
			get => (double)(_localSettings.Values[_barThicknessKey] ?? 3.0);
			set => _localSettings.Values[_barThicknessKey] = value;
		}

		public bool AdjustBarThicknessWithZoom
		{
			get => (bool)(_localSettings.Values[_adjustBarThicknessWithZoomKey] ?? false);
			set => _localSettings.Values[_adjustBarThicknessWithZoomKey] = value;
		}

		public bool AdjustCaliperLabelSizeWithZoom
		{
			get => (bool)(_localSettings.Values[_adjustCaliperLabelSizeWithZoomKey] ?? false);
			set => _localSettings.Values[_adjustCaliperLabelSizeWithZoomKey ] = value;	
		}

		public Rounding Rounding
		{
			get => (Rounding)(_localSettings.Values[_roundingKey] ?? Rounding.ToInt);
			set => _localSettings.Values[_roundingKey] = (int)value;
		}
		public bool AutoAlignLabel
		{
			get => (bool)(_localSettings.Values[_autoAlignLabelKey] ?? true);
			set => _localSettings.Values[_autoAlignLabelKey] = value;
		}

		public bool ShowSampleEcgAtStartUp
		{
			// The default is now false, since we have a nice startup screen.
			get => (bool)(_localSettings.Values[_showSampleEcgAtStartUpKey] ?? false);
			set => _localSettings.Values[_showSampleEcgAtStartUpKey ] = value;	
		}

		public bool ShowBrugadaTriangle
		{
			get => (bool)(_localSettings.Values[_showBrugadaTriangleKey] ?? true);
			set => _localSettings.Values[_showBrugadaTriangleKey] = value;
		}

		public StartupPage StartupPage
		{
			get => (StartupPage)(_localSettings.Values[_startupPageKey] ?? StartupPage.Main);
			set => _localSettings.Values[_startupPageKey] = (int)value;
		}

		public bool IsAlwaysOnTop
		{
			get => (bool)(_localSettings.Values[_isAlwaysOnTopKey] ?? false);
			set => _localSettings.Values[_isAlwaysOnTopKey] = value;
		}

		public bool RecalibrateBetweenPdfPages
		{
			get => (bool)(_localSettings.Values[_recalibrateBetweenPdfPagesKey] ?? true);
			set => _localSettings.Values[_recalibrateBetweenPdfPagesKey] = value;
		}

		public bool ResetZoomBetweenPdfPages
		{
			get => (bool)(_localSettings.Values[_resetZoomBetweenPdfPagesKey] ?? true);
			set => _localSettings.Values[_resetZoomBetweenPdfPagesKey] = value;
		}
		public bool ResetRotationBetweenPdfPages
		{
			get => (bool)(_localSettings.Values[_resetRotationBetweenPdfPagesKey] ?? true);
			set => _localSettings.Values[_resetRotationBetweenPdfPagesKey] = value;
		}
		public bool ClearCalipersBetweenPdfPages
		{
			get => (bool)(_localSettings.Values[_clearCalipersBetweenPdfPagesKey] ?? true);
			set => _localSettings.Values[_clearCalipersBetweenPdfPagesKey] = value;
		}
		public CaliperLabelAlignment TimeCaliperLabelAlignment
		{
			get
			{
				var value = (int)(_localSettings.Values[_timeCaliperLabelAlignmentKey] ?? 0);
				var alignment = (CaliperLabelAlignment)value;
				return alignment;
			}
			set => _localSettings.Values[_timeCaliperLabelAlignmentKey] = (int)value;
		}
		public CaliperLabelAlignment AmplitudeCaliperLabelAlignment
		{
			get
			{
				var value = (int)(_localSettings.Values[_amplitudeCaliperLabelAlignmentKey] ?? 0);
				var alignment = (CaliperLabelAlignment)value;
				return alignment;
			}
			set => _localSettings.Values[_amplitudeCaliperLabelAlignmentKey] = (int)value;
		}

		public Color UnselectedCaliperColor
		{
			get
			{
				if (_localSettings.Values[_unselectedCaliperColorKey] is not string hexColor)
				{
					return Colors.Blue;
				}
				var color = GetColorFromString(hexColor);
				return color;
			}
			set
			{
				var hexColor = value.ToString();
				_localSettings.Values[_unselectedCaliperColorKey] = hexColor;
			}
		}
		public Color SelectedCaliperColor
		{
			get
			{
				if (_localSettings.Values[_selectedCaliperColorKey] is not string hexColor)
				{
					return Colors.Red;
				}
				var color = GetColorFromString(hexColor);
				return color;
			}
			set
			{
				var hexColor = value.ToString();
				_localSettings.Values[_selectedCaliperColorKey] = hexColor;
			}
		}

		public double DefaultNoteFontSize
		{
			get => (double)(_localSettings.Values[_defaultNoteFontSizeKey] ?? 14.0);
			set => _localSettings.Values[_defaultNoteFontSizeKey] = value;
		}

		public double DefaultNoteWidth
		{
			get => (double)(_localSettings.Values[_defaultNoteWidthKey] ?? 180.0);
			set => _localSettings.Values[_defaultNoteWidthKey] = value;
		}

		public double DefaultNoteHeight
		{
			get => (double)(_localSettings.Values[_defaultNoteHeightKey] ?? 80.0);
			set => _localSettings.Values[_defaultNoteHeightKey] = value;
		}

		public bool AllowNegativeCaliperValues
		{
			get => (bool)(_localSettings.Values[_allowNegativeCaliperValuesKey] ?? true);
			set => _localSettings.Values[_allowNegativeCaliperValuesKey] = value;
		}

		public Color DefaultNoteForegroundColor
		{
			get
			{
				if (_localSettings.Values[_defaultNoteForegroundColorKey] is not string hexColor)
				{
					return Colors.Black;
				}
				return GetColorFromString(hexColor);
			}
			set
			{
				var hexColor = value.ToString();
				_localSettings.Values[_defaultNoteForegroundColorKey] = hexColor;
			}
		}


		private static Color GetColorFromString(string colorHex)
		{
			var a = Convert.ToByte(colorHex.Substring(1, 2), 16);
			var r = Convert.ToByte(colorHex.Substring(3, 2), 16);
			var g = Convert.ToByte(colorHex.Substring(5, 2), 16);
			var b = Convert.ToByte(colorHex.Substring(7, 2), 16);
			return Color.FromArgb(a, r, g, b);
		}

	}

	public class FakeSettings : ISettings
	{
		public double BarThickness { get; set; } = 2.0;
		public EPCalipersPdf.PdfResolution PdfResolution { get; set; } = EPCalipersPdf.PdfResolution.High;
		public int FontSize { get; set; } = CaliperLabel.MediumFont;
		public bool AutoAlignLabel { get; set; } = false;
		public CaliperLabelAlignment TimeCaliperLabelAlignment { get; set; } = CaliperLabelAlignment.Left;
		public CaliperLabelAlignment AmplitudeCaliperLabelAlignment { get; set; } = CaliperLabelAlignment.Left;
		public Color UnselectedCaliperColor { get; set; }
		public Color SelectedCaliperColor { get; set; }
		public Rounding Rounding { get; set; } = Rounding.None;
		public bool ShowBrugadaTriangle { get; set; } = false;

		public int NumberOfMeanIntervals { get; set; } = 3;
		public int NumberOfRRIntervals { get; set; } = 1;

		public QtcFormula QtcFormula { get; set; } = QtcFormula.qtcBzt;
		public int SelectedTimeCalibrationRadioButton { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public int SelectedAmplitudeCalibrationRadioButton { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public string CustomTimeCalibration { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public string CustomAmplitudeCalibration { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public int NumberOfMarchingCalipers { get; set; } = 20;
		public bool ShowSampleEcgAtStartUp { get; set; } = false;
		public bool AdjustBarThicknessWithZoom {  get; set; } = false;
		public bool AdjustCaliperLabelSizeWithZoom { get; set; } = false;
		public CaliperViewAlignment CaliperViewAlignment { get; set; } = CaliperViewAlignment.TopLeft;
		public StartupPage StartupPage { get; set; } = StartupPage.Main;
		public bool IsAlwaysOnTop {  get; set; } = false;
		public bool RecalibrateBetweenPdfPages { get; set; } = true;
		public bool ResetZoomBetweenPdfPages { get; set; } = true;
		public bool ResetRotationBetweenPdfPages { get; set; } = true;
		public bool ClearCalipersBetweenPdfPages { get; set; } = true;
		public double DefaultNoteFontSize { get; set; } = 14.0;
		public double DefaultNoteWidth { get; set; } = 180.0;
		public double DefaultNoteHeight { get; set; } = 80.0;
		public Color DefaultNoteForegroundColor { get; set; } = Colors.Black;

		public bool AllowNegativeCaliperValues { get; set; } = true;
	}
}
