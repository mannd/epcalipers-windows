using EPCalipersWinUI3.Calipers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.Models
{
	public class Settings
	{
		private Windows.Storage.ApplicationDataContainer _localSettings;
		private readonly string _autoAlignLabelKey = "AutoAlignLabel";
		private readonly string _timeCaliperLabelAlignmentKey = "TimeCaliperLabelAlignmentKey";
		private readonly string _amplitudeCaliperLabelAlignmentKey = "AmplitudeCaliperLabelAlignmentKey";
		public Settings()
		{
			_localSettings =
				Windows.Storage.ApplicationData.Current.LocalSettings;
		}

		public bool AutoAlignLabel
		{
			get => (bool)_localSettings.Values[_autoAlignLabelKey];
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
	}
}
