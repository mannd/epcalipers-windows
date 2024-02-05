using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models;
using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.Views;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Windows.Security.Isolation;
using WinUIEx;

namespace EPCalipersWinUI3.ViewModels
{
	/// <summary>
	/// Contains all the parameters passed back and forth to calculate the QTc.
	/// </summary>
	// TODO: we are forced to send a bunch of unrelated stuff as a blob to each dialog.
	// Really need specific dialogs for each measurement...
	public class QtcParameters
	{
		public WindowEx Window {  get; set; }
		public Caliper Caliper { get; set; }
		public CaliperCollection CaliperCollection { get; set; }
		public int NumberOfIntervals { get; set; }
		public QtcMeasurement QtcMeasurement { get; set; } = new QtcMeasurement();
	}

	public partial class QtcViewModel: ObservableObject
	{
		private QtcMeasurement _model = new QtcMeasurement();

		public QtcViewModel()
		{
			Debug.Print("reload QtcViewModel");
			QtcFormulas.Add("BazettFormula".GetLocalized());
			QtcFormulas.Add("FraminghamFormula".GetLocalized());
			QtcFormulas.Add("HodgesFormula".GetLocalized());
			QtcFormulas.Add("FridericiaFormula".GetLocalized());
			QtcFormulas.Add("AllQTcFormulas".GetLocalized());
		}

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
			CalculateQTc();
		}

		[RelayCommand]
		public void CalculateQTc()
		{
			Debug.Print($"{_model.QTc}");
			// TODO: use QtcParameters to either calculate QTc or give incomplete
			// data error message.
			// need another floating window or a dialog??  Maybe replace main QTc view with a new frame containing
			// the meanRateIntervalView??, just change the page, and go back when done.  Same for QTc.
		}

		[ObservableProperty]
		private string rrInterval = "Not measured";

		[ObservableProperty]
		private string qtInterval = "Not measured";

		[ObservableProperty]
		private List<string> qtcFormulas = new List<string>();

		[ObservableProperty]
		private int selectedFormulaIndex = 0;
	}
}
