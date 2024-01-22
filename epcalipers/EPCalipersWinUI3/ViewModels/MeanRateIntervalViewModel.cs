using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.ViewModels
{
	public partial class MeanRateIntervalViewModel: ObservableObject
	{
		public int NumberOfIntervals { get; set; }


		// TODO Create message box with results, mean interval and mean rate
		public static double Calculate(double interval, int numberOfIntervals)
		{
			return MathHelper.MeanInterval(interval, numberOfIntervals);
		}
	}
}
