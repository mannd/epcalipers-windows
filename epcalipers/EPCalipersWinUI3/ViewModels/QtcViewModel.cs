﻿using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.ViewModels
{
	public partial class QtcViewModel
	{
		public Caliper caliper {  get; set; }

		[RelayCommand]
		private void MeasureRRInterval()
		{
			Debug.Print("clicked measure RR interval");
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