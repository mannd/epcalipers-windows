using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPCalipersWinUI3.Models;
using EPCalipersWinUI3.Calipers;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using Windows.Storage.Streams;

namespace EPCalipersWinUI3.ViewModels
{
	public partial class SettingsViewModel: ObservableObject
	{
		private Settings _model = new Settings();

		public SettingsViewModel() {
			TimeCaliperLabelAlignment = (int)_model.TimeCaliperLabelAlignment;
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
				default:
					break;
			}
		}

		[ObservableProperty]
		private bool autoPositionLabel;

		[ObservableProperty]
		private int timeCaliperLabelAlignment;

		[ObservableProperty]
		private int amplitudeCaliperLabelAlignment;
	}
}
