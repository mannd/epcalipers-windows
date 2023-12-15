using EPCalipersWinUI3.Models;
using EPCalipersWinUI3.Models.Calipers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace EPCalipersWinUI3.Contracts
{
    public interface ISettings
	{
		public double BarThickness { get; set; }
		public bool AutoAlignLabel { get; set; }
		public CaliperLabelAlignment TimeCaliperLabelAlignment { get; set; }	
		public CaliperLabelAlignment AmplitudeCaliperLabelAlignment { get; set; }	
		public Color UnselectedCaliperColor { get; set; }	
		public Color SelectedCaliperColor { get; set; }
		public Rounding Rounding { get; set; }
	}
}
