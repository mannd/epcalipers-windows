using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;

namespace WPFepcalipers
{
	/// <summary>
	/// Interaction logic for AboutBox.xaml
	/// </summary>
	public partial class AboutBox : Window
	{
		// If set, more detailed info appears in AboutBox.
		public bool AdditionalOptions
		{
			get => _additionalOptions;
			set
			{
				SetVersion(value);
				_additionalOptions = value;
			}
		}
		private bool _additionalOptions;

		public AboutBox()
		{
			InitializeComponent();
			AdditionalInitiation();
			Debug.WriteLine(AssemblyProperties.AssemblyConfigurationAttribute);
		}

		private void AdditionalInitiation()
		{
			Title = String.Format(CultureInfo.CurrentCulture,
				"About {0}", AssemblyProperties.AssemblyTitle);
			AppName.Text = AssemblyProperties.AssemblyProduct;
#if DEBUG
			SetVersion(true);
#else
			SetVersion();
#endif
			Copyright.Text = AssemblyProperties.AssemblyCopyright;
			SetCompany();
			SetConfiguration();
			DescriptionText.Text = AssemblyProperties.AssemblyDescription;
		}

		private void SetCompany()
		{
			Run run = new(AssemblyProperties.AssemblyCompany);
			Hyperlink hyperlink = new(run)
			{
				NavigateUri = new Uri("https://www.epstudiossoftware.com")
			};
			hyperlink.RequestNavigate += Hyperlink_RequestNavigate;
			Company.Text = null;
			Company.Inlines.Add(hyperlink);
		}

		private void SetConfiguration()
		{
			string? configuration = AssemblyProperties.AssemblyConfigurationAttribute;
			if (configuration == null || configuration == "")
			{
				Configuration.Text = "Generic configuration";
			}
			else
			{
				Configuration.Text = configuration;
			}
		}

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
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}
}
