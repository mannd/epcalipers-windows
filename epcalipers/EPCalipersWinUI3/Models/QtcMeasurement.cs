using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models.Calipers;

namespace EPCalipersWinUI3.Models
{
	public class QtcMeasurement: IntervalMeasurement
	{
		public double QTInterval { get; set; }
		public MathHelper.QtcFormula QtcFormula { get; set; }
		public string QTc => "QTc = ????";

	}

	public class IntervalMeasurement: INotifyPropertyChanged
	{
		public double RRInterval
		{
			get => _rrInterval;
			set
			{
				_rrInterval = value;
				OnPropertyChanged(nameof(RRInterval));
				
			}
		}
		private double _rrInterval;
		public int NumberOfRRIntervals { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
		public virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
