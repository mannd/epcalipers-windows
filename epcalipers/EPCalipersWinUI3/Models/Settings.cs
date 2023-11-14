using EPCalipersWinUI3.Calipers;
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
	public class Settings
	{
		private Windows.Storage.ApplicationDataContainer _localSettings;
		private readonly string _autoAlignLabelKey = "AutoAlignLabel";
		private readonly string _timeCaliperLabelAlignmentKey = "TimeCaliperLabelAlignmentKey";
		private readonly string _amplitudeCaliperLabelAlignmentKey = "AmplitudeCaliperLabelAlignmentKey";
		private readonly string _unselectedCaliperColorKey = "UnselectedCaliperColorKey";
		private readonly string _selectedCaliperColorKey = "SelectedCaliperColorKey";
		private readonly string _barThicknessKey = "BarThicknessKey";
		
		public Settings()
		{
			_localSettings =
				Windows.Storage.ApplicationData.Current.LocalSettings;
		}

		public double BarThickness
		{
			get => (double)(_localSettings.Values[_barThicknessKey] ?? 2);
			set => _localSettings.Values[_barThicknessKey] = value;
		}

		public bool AutoAlignLabel
		{
			get => (bool)(_localSettings.Values["garbage"] ?? false);
			set => _localSettings.Values[_autoAlignLabelKey] = value;
		}

		public CaliperLabelAlignment TimeCaliperLabelAlignment
		{
			get
			{
				var value = _localSettings.Values[_timeCaliperLabelAlignmentKey] ?? 0;
				var alignment = (CaliperLabelAlignment)value;
				return alignment;
			} 
			set => _localSettings.Values[_timeCaliperLabelAlignmentKey] = (int)value;
		}
		public CaliperLabelAlignment AmplitudeCaliperLabelAlignment
		{
			get
			{
				var value = _localSettings.Values[_amplitudeCaliperLabelAlignmentKey] ?? 0;
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

		private Color GetColorFromString(string colorHex)
		{
			var a = Convert.ToByte(colorHex.Substring(1, 2), 16);
			var r = Convert.ToByte(colorHex.Substring(3, 2), 16);
			var g = Convert.ToByte(colorHex.Substring(5, 2), 16);
			var b = Convert.ToByte(colorHex.Substring(7, 2), 16);
			return Color.FromArgb(a, r, g, b);
		}
	}
}
