using CommunityToolkit.Mvvm.ComponentModel;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models;
using System;
using System.Diagnostics;

namespace EPCalipersWinUI3.ViewModels
{
	public partial class AboutViewModel : ObservableObject
	{

		private readonly AppInfo model = new();

		public AboutViewModel()
		{
			Version = model.FileVersion;
			Title = string.Format("AboutEPCalipersDialogTitle".GetLocalized(), model.ProductName);
			Copyright = $"{model.Copyright} {model.Company}";
			ProductVersion = model.ProductVersion;
			SaneProductVersion = model.SaneProductVersion;
		}

		[ObservableProperty]
		private string title;

		[ObservableProperty]
		private string appName = AssemblyProperties.AssemblyProduct;

		[ObservableProperty]
		private string version;

		[ObservableProperty]
		private string productVersion;

		[ObservableProperty]
		private string saneProductVersion;

		[ObservableProperty]
		private string copyright;

		[ObservableProperty]
		private string homePage = "HomePageButtonTitle".GetLocalized();

		[ObservableProperty]
		private string navigateUri = "https://www.epstudiossoftware.com";

		public void DebugPrintAssemblyInfo()
		{
			Debug.WriteLine(model.ProductName);
			Debug.WriteLine(model.SaneProductVersion);
			Debug.WriteLine(model.ProductVersion);
			Debug.WriteLine(model.FileVersion);
			Debug.WriteLine(model.Title);
			Debug.WriteLine(model.Copyright);
			Debug.WriteLine(model.Company);
			Debug.WriteLine(model.CpuArchitecture);
		}
	}
}
