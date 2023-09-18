using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using EPCalipersWinUI3.Models;
using Microsoft.UI.Xaml.Documents;
using System.Diagnostics;
using EPCalipersWinUI3.ViewModels;

namespace EPCalipersWinUI3.Views
{
	public sealed partial class AboutDialog : ContentDialog
	{
		public AboutViewModel ViewModel { get; set; }

		public AboutDialog()
		{
			ViewModel = new AboutViewModel();
			this.InitializeComponent();
			ViewModel.DebugPrintAssemblyInfo();
		}
	}
}
