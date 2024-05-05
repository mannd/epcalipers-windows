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
using EPCalipersWinUI3.Models;

namespace EPCalipersWinUI3.ViewModels
{
	/// <summary>
	/// Contains all the parameters passed back and forth to calculate the QTc.
	/// </summary>
	public class QtcParameters
	{
		public WindowEx Window {  get; set; }
		public Caliper Caliper { get; set; }
		public CaliperCollection CaliperCollection { get; set; }
		public Measurement RRMeasurement {  get; set; }
		public Measurement QTMeasurement {  get; set; }
		public IntervalMeasured IntervalMeasured { get; set; }
	}

	// BUG: Don't allow certain menu items to appear while calibration dialog is open.
	// Opening new files, settings, help, changing view, etc. causes bugs.

	// BUG: Don't select full caliper automatically if one bar of sole caliper is selected.  This allows fine tuning
	// of caliper movement with keyboard shortcuts during calibration.  Can selected bar be changed
	// during calibration?

	// BUG: Consider inhibiting inactive menu items during calibration.

	// NB: QTc calculation always rounds to 1 decimal place, since clinically anything
	// more than that is not realistic.
	public partial class QtcViewModel: ObservableObject
	{
		public static string NotMeasured { get; set; } = "Not measured".GetLocalized();
		public XamlRoot XamlRoot { get; set; }
		public QtcViewModel(QtcParameters qtcParameters)
		{
			QtcFormulas = new()
			{
				{ QtcFormula.qtcBzt, "BazettFormula".GetLocalized() },
				{ QtcFormula.qtcFrm, "FraminghamFormula".GetLocalized() },
				{ QtcFormula.qtcHdg, "HodgesFormula".GetLocalized() },
				{ QtcFormula.qtcFrd, "FridericiaFormula".GetLocalized() },
				{ QtcFormula.qtcAll, "AllQTcFormulas".GetLocalized() }
			};
			QtcFormulaNames = QtcFormulas.Values.ToList();
			_qtcParameters = qtcParameters;
			if (_qtcParameters != null)
			{
				var caliperCollection = qtcParameters.CaliperCollection;
				if (caliperCollection != null)
				{
					caliperCollection.PropertyChanged += OnMyPropertyChanged;
					caliperCollection.TimeCalibration.PropertyChanged += OnMyPropertyChanged;
				}
			}
		}

		[RelayCommand]
		public void FormulaComboBoxLoaded()
		{
			SelectedFormulaIndex = (int)Settings.Instance.QtcFormula;
		}

		[RelayCommand]
		public void ResetRRInterval()
		{
			_qtcParameters.RRMeasurement = new Measurement();
			UpdateRRInterval();
			CheckCanCalculate();
		}

		[RelayCommand]
		public void ResetQTInterval()
		{
			_qtcParameters.QTMeasurement = new Measurement();
			UpdateQTInterval();
			CheckCanCalculate();
		}

		[RelayCommand]
		public void ResetIntervals()
		{
			ResetRRInterval();
			ResetQTInterval();
		}

		public void UpdateIntervals()
		{
			if (_qtcParameters.IntervalMeasured == IntervalMeasured.RR)
			{
				UpdateRRInterval();
			}
			if (_qtcParameters.IntervalMeasured == IntervalMeasured.QT)
			{
				UpdateQTInterval();
			}
			_qtcParameters.IntervalMeasured = IntervalMeasured.None;
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
			CheckCanCalculate();
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
			CheckCanCalculate();
		}

		private void CheckCanCalculate()
		{
			var rrUnit = QtcParameters.RRMeasurement.Unit;
			var qtUnit = QtcParameters.QTMeasurement.Unit;
			var rrIsMeasured = rrUnit != Unit.None;
			var qtIsMeasured = qtUnit != Unit.None;
			CanCalculate = rrIsMeasured && qtIsMeasured
				&& MathHelper.QtcCalculator.CanCalculate(QtcParameters.RRMeasurement,
				QtcParameters.QTMeasurement);
		}

		private void OnMyPropertyChanged(object sender, PropertyChangedEventArgs e)
		{ 
			if (e.PropertyName == nameof(CaliperCollection.TimeCalibration))
			{
				if (QtcParameters != null)
				{
					ResetIntervals();
				}
			}
		}

		public QtcParameters QtcParameters
		{
			get => _qtcParameters;
			set
			{
				if (_qtcParameters != value)
				{
					_qtcParameters = value;
				}
			}
		}
		private QtcParameters _qtcParameters;

		[RelayCommand]
		private void MeasureRRInterval()
		{
			QtcParameters.IntervalMeasured = IntervalMeasured.RR;
			Frame frame = QtcParameters.Window.Content as Frame;
			frame?.Navigate(typeof(MeanRateIntervalView), QtcParameters);
		}

		[RelayCommand]
		private void MeasureQTInterval()
		{
			QtcParameters.IntervalMeasured = IntervalMeasured.QT;
			Frame frame = QtcParameters.Window.Content as Frame;
			frame?.Navigate(typeof(MeasureIntervalView), QtcParameters);
		}

		[RelayCommand]
		public async Task CalculateQTc()
		{
			Settings.Instance.QtcFormula = (QtcFormula)SelectedFormulaIndex;
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
