﻿using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Models.Calipers;
using Microsoft.UI;
using System;
using System.Diagnostics;
using Windows.Storage;
using Windows.UI;

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
	public sealed class Settings : ISettings
	{
		private readonly ApplicationDataContainer _localSettings;
		private readonly string _autoAlignLabelKey = "AutoAlignLabel";
		private readonly string _timeCaliperLabelAlignmentKey = "TimeCaliperLabelAlignmentKey";
		private readonly string _amplitudeCaliperLabelAlignmentKey = "AmplitudeCaliperLabelAlignmentKey";
		private readonly string _unselectedCaliperColorKey = "UnselectedCaliperColorKey";
		private readonly string _selectedCaliperColorKey = "SelectedCaliperColorKey";
		private readonly string _barThicknessKey = "BarThicknessKey";
		private readonly string _roundingKey = "RoundingKey";
		private readonly string _showBrugadaTriangle = "ShowBrugadaTriangle";

		// Saved parameters not set directly by the user.
		private readonly string _numberOfMeanIntervalsKey = "NumberOfMeanIntervals";
		private readonly string _numberOfRRIntervalsKey = "NumberOfRRIntervals";

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

		public int NumberOfRRIntervals
		{
			get => (int)(_localSettings.Values[_numberOfRRIntervalsKey] ?? 1);
			set
			{
				_localSettings.Values[_numberOfRRIntervalsKey] = value;
				Debug.Print($"numberOfRRIntervals = {value}");
			} 
		}

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
		// TODO: default for show Brugada triangle should probably be false.  Set true for testing.
		public bool ShowBrugadaTriangle
		{
			get => (bool)(_localSettings.Values[_showBrugadaTriangle] ?? true);
			set => _localSettings.Values[_showBrugadaTriangle] = value;
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
		public bool AutoAlignLabel { get; set; } = false;
		public CaliperLabelAlignment TimeCaliperLabelAlignment { get; set; } = CaliperLabelAlignment.Left;
		public CaliperLabelAlignment AmplitudeCaliperLabelAlignment { get; set; } = CaliperLabelAlignment.Left;
		public Color UnselectedCaliperColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public Color SelectedCaliperColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public Rounding Rounding { get; set; } = Rounding.None;
		public bool ShowBrugadaTriangle { get; set; } = false;

		public int NumberOfMeanIntervals { get; set; } = 3;
		public int NumberOfRRIntervals { get; set; } = 1;
	}
}
