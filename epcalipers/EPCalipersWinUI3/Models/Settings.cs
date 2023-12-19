using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Models.Calipers;
using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;

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
    public sealed class Settings: ISettings
	{
		private ApplicationDataContainer _localSettings;
		private readonly string _autoAlignLabelKey = "AutoAlignLabel";
		private readonly string _timeCaliperLabelAlignmentKey = "TimeCaliperLabelAlignmentKey";
		private readonly string _amplitudeCaliperLabelAlignmentKey = "AmplitudeCaliperLabelAlignmentKey";
		private readonly string _unselectedCaliperColorKey = "UnselectedCaliperColorKey";
		private readonly string _selectedCaliperColorKey = "SelectedCaliperColorKey";
		private readonly string _barThicknessKey = "BarThicknessKey";
		private readonly string _roundingKey = "RoundingKey";
		
		private Settings()
		{
			_localSettings = ApplicationData.Current.LocalSettings;
		}

		private static readonly Lazy<Settings> lazy =
		new Lazy<Settings>(() => new Settings());

		public static Settings Instance { get { return lazy.Value; } }

		public double BarThickness
		{
			get => (double)(_localSettings.Values[_barThicknessKey] ?? 2.0);
			set => _localSettings.Values[_barThicknessKey] = value;
		}
		public Rounding Rounding
		{
			get => (Rounding)(_localSettings.Values[_roundingKey] ?? Rounding.ToInt);
			set => _localSettings.Values[_roundingKey] = (int)value;
		}

		public bool AutoAlignLabel
		{
			get => (bool)(_localSettings.Values[_autoAlignLabelKey] ?? false);
			set => _localSettings.Values[_autoAlignLabelKey] = value;
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
				var hexColor = _localSettings.Values[_unselectedCaliperColorKey] as string;
				if (hexColor == null)
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
				var hexColor = _localSettings.Values[_selectedCaliperColorKey] as string;
				if (hexColor == null)
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
		private Color GetColorFromString(string colorHex)
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
		public bool AutoAlignLabel { get; set; } = false;
		public CaliperLabelAlignment TimeCaliperLabelAlignment {  get; set; } = CaliperLabelAlignment.Left;
		public CaliperLabelAlignment AmplitudeCaliperLabelAlignment { get; set;} = CaliperLabelAlignment.Left;
		public Color UnselectedCaliperColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public Color SelectedCaliperColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public Rounding Rounding { get; set; } = Rounding.None;
	}
}
