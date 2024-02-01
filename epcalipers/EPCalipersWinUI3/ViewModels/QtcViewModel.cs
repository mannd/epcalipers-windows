using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.Views;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;
using Windows.Security.Isolation;
using WinUIEx;

namespace EPCalipersWinUI3.ViewModels
{
	public class QtcParameters
	{
		public WindowEx Window {  get; set; }
		public Caliper Caliper { get; set; }
		public CaliperCollection CaliperCollection { get; set; }
	}

	public partial class QtcViewModel
	{
		public Caliper caliper { get; set; }
		public WindowEx Window { get; set; }
		private MeanRateIntervalView _meanRateIntervalView;
		public QtcParameters QtcParameters { get; set; }	

		[RelayCommand]
		private void MeasureRRInterval()
		{
			Frame frame = QtcParameters.Window.Content as Frame;
			if (frame != null)
			{
				frame.Navigate(typeof(MeanRateIntervalView), QtcParameters);
			}
		}

		[RelayCommand]
		private void MeasureQTInterval()
		{
			Debug.Print("clicked measure QT interval");
		}

		public void CalculateQTc()
		{
			Debug.Print("calculating QTc...");
			// need another floating window or a dialog??  Maybe replace main QTc view with a new frame containing
			// the meanRateIntervalView??, just change the page, and go back when done.  Same for QTc.
		}
	}
}
