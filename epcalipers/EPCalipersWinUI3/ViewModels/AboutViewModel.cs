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

	// TODO: Localize strings, possibly using satellite assemblies, or just resources.
	public partial class AboutViewModel : ObservableObject
	{

		private readonly AppInfo model = new AppInfo();

		public AboutViewModel()
		{
			version = model.ProductVersion;
			title = $"About {model.ProductName}";
			copyright = $"{model.Copyright} {model.Company}";
		}

		[ObservableProperty]
		private string title;

		[ObservableProperty]
		private string appName = AssemblyProperties.AssemblyProduct;

		[ObservableProperty]
		private string version;

		[ObservableProperty]
		private string copyright;

		[ObservableProperty]
		private string homePage = "EP Studios home page";

		[ObservableProperty]
		private string navigateUri = "https://www.epstudiossoftware.com";

		public void DebugPrintAssemblyInfo()
		{
			Debug.WriteLine("testing");
			Debug.WriteLine(model.ProductName);
			Debug.WriteLine(model.ProductVersion);
			Debug.WriteLine(model.FileVersion);
			Debug.WriteLine(model.Title);
			Debug.WriteLine(model.Copyright);
			Debug.WriteLine(model.Company);
		}
	}
}
