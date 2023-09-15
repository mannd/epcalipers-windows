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

namespace EPCalipersWinUI3.Views
{
	public sealed partial class AboutDialog : ContentDialog
	{

		public AboutDialog()
		{
			this.InitializeComponent();
			AdditionalInitiation();
		}

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
		}

		private void AdditionalInitiation()
		{
			Title = $"About {AssemblyProperties.AssemblyProduct}";
			AppName.Text = AssemblyProperties.AssemblyProduct;
			SetVersion();
			Copyright.Text = $"{AssemblyProperties.AssemblyCopyright} {AssemblyProperties.AssemblyCompany}";
			Debug.Write(Copyright.Text);
			//SetConfiguration();
			//DescriptionText.Text = AssemblyProperties.AssemblyDescription;
		}

		//private void SetCompany()
		//{
		//	Company.Text = AssemblyProperties.AssemblyCompany;
		//	//Run run = new(AssemblyProperties.AssemblyCompany);
		//	//Hyperlink hyperlink = new(run)
		//	//{
		//	//	NavigateUri = new Uri("https://www.epstudiossoftware.com")
		//	//};
		//	//hyperlink.RequestNavigate += Hyperlink_RequestNavigate;
		//	//Company.Text = null;
		//	//Company.Inlines.Add(hyperlink);
		//}

		//private void SetConfiguration()
		//{
		//	string? configuration = AssemblyProperties.AssemblyConfigurationAttribute;
		//	if (configuration == null || configuration == "")
		//	{
		//		Configuration.Text = "Generic configuration";
		//	}
		//	else
		//	{
		//		Configuration.Text = configuration;
		//	}
		//}

		private void SetVersion(bool detailed = false)
		{
			if (detailed)
			{
				this.Version.Text = String.Format(CultureInfo.CurrentCulture,
					"Version {0} ({1})", AssemblyProperties.AssemblyVersion, AssemblyProperties.AssemblyFileVersion);
				return;
			}
			this.Version.Text = String.Format(CultureInfo.CurrentCulture,
				"Version {0}", AssemblyProperties.AssemblyVersion);
		}
	}
}
