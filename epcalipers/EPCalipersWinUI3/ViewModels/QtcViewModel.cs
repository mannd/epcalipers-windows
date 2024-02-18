using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WinUIEx;
using static EPCalipersWinUI3.Helpers.MathHelper;

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
		public Measurement RRMeasurement {  get; set; }
		public Measurement QTMeasurement {  get; set; }
		public IntervalMeasured IntervalMeasured { get; set; }

		// TODO: refactor this away, use settings to set
		public int NumberOfIntervals = 1;

		// TODO: do we still need to notify for any events in this class?
		// maybe if CaliperCollection changes or Calibration changes?
		public event PropertyChangedEventHandler PropertyChanged;
		public virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public partial class QtcViewModel: ObservableObject
	{
		// TODO: localize
		public static string NotMeasured { get; set; } = "Not measured";
		public XamlRoot XamlRoot { get; set; }
		public QtcViewModel()
		{
			QtcFormulas = new();
			QtcFormulas.Add(QtcFormula.qtcBzt, "BazettFormula".GetLocalized());
			QtcFormulas.Add(QtcFormula.qtcFrm, "FraminghamFormula".GetLocalized());
			QtcFormulas.Add(QtcFormula.qtcHdg, "HodgesFormula".GetLocalized());
			QtcFormulas.Add(QtcFormula.qtcFrd, "FridericiaFormula".GetLocalized());
			QtcFormulas.Add(QtcFormula.qtcAll, "AllQTcFormulas".GetLocalized());

			QtcFormulaNames = QtcFormulas.Values.ToList();
		}

		[RelayCommand]
		public void FormulaComboBoxLoaded()
		{
			// TODO: store this from session to session
			SelectedFormulaIndex = 1;
		}

		[RelayCommand]
		public void ResetRRInterval()
		{
			_qtcParameters.RRMeasurement = new Measurement();
			UpdateIntervals();
		}

		[RelayCommand]
		public void ResetQTInterval()
		{
			_qtcParameters.QTMeasurement = new Measurement();
			UpdateIntervals();
		}

		[RelayCommand]
		public void ResetIntervals()
		{
			ResetRRInterval();
			ResetQTInterval();
		}

		public void UpdateIntervals()
		{
			UpdateRRInterval();
			UpdateQTInterval();
			CheckCanCalculate();
		}

		public void UpdateRRInterval()
		{
			var rrMeasurement = QtcParameters.RRMeasurement;
			RrInterval = NotMeasured;
			if (rrMeasurement.Unit == Unit.None)
			{
				return;
			}
			var calibration = QtcParameters.CaliperCollection.TimeCalibration;
			var formattedRRMeasurement = calibration?.GetFormattedMeasurement(rrMeasurement.Value);
			if (formattedRRMeasurement != null)
			{
				RrInterval = formattedRRMeasurement;
			}
		}
		public void UpdateQTInterval()
		{
			var qtMeasurement = QtcParameters.QTMeasurement;
			QtInterval = NotMeasured;
			if (qtMeasurement.Unit == Unit.None)
			{
				return;
			}
			var calibration = QtcParameters.CaliperCollection.TimeCalibration;
			var formattedQTMeasurement = calibration?.GetFormattedMeasurement(qtMeasurement.Value);
			if (formattedQTMeasurement != null)
			{
				QtInterval = formattedQTMeasurement;
			}
		}

		private void CheckCanCalculate()
		{
			var rrUnit = QtcParameters.RRMeasurement.Unit;
			var qtUnit = QtcParameters.QTMeasurement.Unit;
			var rrIsMeasured = rrUnit != Unit.None;
			var qtIsMeasured = qtUnit != Unit.None;
			CanCalculate = rrIsMeasured && qtIsMeasured;
		}

		private void OnMyPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			//if (e.PropertyName == nameof(QtcParameters.RRMeasurement))
			//{
			//	// set RRInterval
			//	var rrMeasurement = QtcParameters.RRMeasurement;
			//	var calibration = QtcParameters.CaliperCollection.TimeCalibration;
			//	if (calibration == null)
			//	{
			//		RrInterval = "Not measured.";
			//		return;
			//	}
			//	var formattedRRMeasurement = calibration.GetFormattedMeasurement(rrMeasurement.Value);
			//	if (formattedRRMeasurement != null)
			//	{
			//		RrInterval = formattedRRMeasurement;
			//	}
			//	else
			//	{
			//		RrInterval = "Not measured.";
			//	}

			//}
			//if (e.PropertyName == nameof(QtcParameters.QTMeasurement))
			//{
			//	// set QTInterval
			//}
		}

	

		public QtcParameters QtcParameters
		{
			get => _qtcParameters;
			set
			{
				if (_qtcParameters != value)
				{
					_qtcParameters = value;
					QtcParameters.PropertyChanged += OnMyPropertyChanged;
				}
			}
		}
		private QtcParameters _qtcParameters;

		[RelayCommand]
		private void MeasureRRInterval()
		{
			QtcParameters.IntervalMeasured = IntervalMeasured.RR;
			Frame frame = QtcParameters.Window.Content as Frame;
			if (frame != null)
			{
				frame.Navigate(typeof(MeanRateIntervalView), QtcParameters);
			}
		}

		[RelayCommand]
		private void MeasureQTInterval()
		{
			QtcParameters.IntervalMeasured = IntervalMeasured.QT;
			Frame frame = QtcParameters.Window.Content as Frame;
			if (frame != null)
			{
				frame.Navigate(typeof(MeasureIntervalView), QtcParameters);
			}
		}

		[RelayCommand]
		public async Task CalculateQTc()
		{
			// TODO: use QtcParameters to either calculate QTc or give incomplete
			// TODO: Inactivate Calculate button until RR and QT are measured.
			var calculator = new QtcCalculator((QtcFormula)SelectedFormulaIndex);
			var result = calculator.Calculate(
				QtcParameters.RRMeasurement, 
				QtcParameters.QTMeasurement, 
				QtcParameters.CaliperCollection.TimeCalibration);
			if (result != null)
			{
				Debug.Print(result);
			}
			var dialog = MessageHelper.CreateMessageDialog("QTc", result);
			dialog.XamlRoot = XamlRoot;
			await dialog.ShowAsync();
		}

		[ObservableProperty]
		private string rrInterval = NotMeasured;

		[ObservableProperty]
		private string qtInterval = NotMeasured;

		[ObservableProperty]
		private Dictionary<QtcFormula, string> qtcFormulas;

		[ObservableProperty]
		private List<string> qtcFormulaNames;

		[ObservableProperty]
		private int selectedFormulaIndex;

		[ObservableProperty]
		private bool canCalculate;
	}
}
