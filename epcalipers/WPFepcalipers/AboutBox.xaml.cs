using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Globalization;

namespace WPFepcalipers
{
	/// <summary>
	/// Interaction logic for AboutBox.xaml
	/// </summary>
	public partial class AboutBox : Window
	{
		public AboutBox()
		{
			InitializeComponent();
			AdditionalInitiation();
			Debug.WriteLine(AssemblyProperties.AssemblyConfigurationAttribute);
		}

		private void AdditionalInitiation()
		{

            this.Title = String.Format(CultureInfo.CurrentCulture, 
				"About {0}", AssemblyProperties.AssemblyTitle);
            this.AppName.Text = AssemblyProperties.AssemblyProduct;
            this.Copyright.Text = AssemblyProperties.AssemblyCopyright;
			Run run = new(AssemblyProperties.AssemblyCompany);
			Hyperlink hyperlink = new(run)
			{
				NavigateUri = new Uri("https://www.epstudiossoftware.com")
			};
			hyperlink.RequestNavigate += Hyperlink_RequestNavigate;
			this.Company.Text = null;
			this.Company.Inlines.Add(hyperlink);
            this.DescriptionText.Text = AssemblyProperties.AssemblyDescription;
			this.Version.Text = String.Format(CultureInfo.CurrentCulture, 
				"Version {0}", AssemblyProperties.AssemblyVersion);
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("OK");
			this.Close();

		}

		private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			try
			{
				var destinationurl = "https://www.epstudiossoftware.com/";
				var sInfo = new System.Diagnostics.ProcessStartInfo(destinationurl)
				{
					UseShellExecute = true,
				};
				System.Diagnostics.Process.Start(sInfo);
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
	}
}
