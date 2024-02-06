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
	public class QtcParameters: INotifyPropertyChanged
	{
		public WindowEx Window {  get; set; }
		public Caliper Caliper { get; set; }
		public CaliperCollection CaliperCollection { get; set; }
		public int NumberOfIntervals { get; set; }
		public double RawRRInterval
		{
			get => _rawRRInterval;
			set
			{
				_rawRRInterval = value;
				OnPropertyChanged(nameof(RawRRInterval));
			}
		}
		private double _rawRRInterval;
		public double QTInterval { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
		public virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public partial class QtcViewModel: ObservableObject
	{
		// TODO: QtcMeasurement or QtcParameters must notify QtcViewModel when RR interval and number of intervals change.
		// Display the current value on the button label... 

		public QtcViewModel()
		{
			Debug.Print("reload QtcViewModel");
			QtcFormulas.Add("BazettFormula".GetLocalized());
			QtcFormulas.Add("FraminghamFormula".GetLocalized());
			QtcFormulas.Add("HodgesFormula".GetLocalized());
			QtcFormulas.Add("FridericiaFormula".GetLocalized());
			QtcFormulas.Add("AllQTcFormulas".GetLocalized());
		}

		private void OnMyPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(QtcParameters.RawRRInterval))
			{
				Debug.Print(QtcParameters.RawRRInterval.ToString());
				RrInterval = QtcParameters.RawRRInterval.ToString();
			}
		}



		public QtcParameters QtcParameters
		{
			get => _qtcParameters;
			set
			{
				_qtcParameters = value;
				QtcParameters.PropertyChanged += OnMyPropertyChanged;
			}
		}

		private QtcParameters _qtcParameters;

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
