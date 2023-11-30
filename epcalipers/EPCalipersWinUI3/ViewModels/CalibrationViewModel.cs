using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Models.Calipers;
using Microsoft.UI.Xaml.Documents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.ViewModels
{
	public partial class CalibrationViewModel: ObservableObject
	{
		public CalibrationViewModel(CaliperType caliperType)
		{
			switch (caliperType)
			{
				case CaliperType.Time:
					FirstField = "1000 msec";
					SecondField = "1.0 sec";
					break;
				case CaliperType.Amplitude:
					FirstField = "1.0 mV";
					SecondField = "10 mm";
					break;
				default:
					throw new NotImplementedException();
			}
		}

		[ObservableProperty]
		private string firstField;

		[ObservableProperty]
		private string secondField;
	}
}
