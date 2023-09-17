using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EPCalipersWinUI3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.ViewModels
{
	public partial class AboutViewModel : ObservableObject
	{

		[ObservableProperty]
		private string title = $"About {AssemblyProperties.AssemblyProduct}";

		[ObservableProperty]
		private string appName = AssemblyProperties.AssemblyProduct;

		[ObservableProperty]
		private string version = String.Format(CultureInfo.CurrentCulture,
				"Version {0}", AssemblyProperties.AssemblyVersion);

		[ObservableProperty]
		private string copyright = $"{AssemblyProperties.AssemblyCopyright} {AssemblyProperties.AssemblyCompany}";

		[ObservableProperty]
		private string homePage = "EP Studios home page";

		[ObservableProperty]
		private string navigateUri = "https://www.epstudiossoftware.com";

		public void debugPrintAssemblyInfo()
		{
			Debug.WriteLine("testing");
		}
	}
}
