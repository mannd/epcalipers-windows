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
using System.Reflection;

namespace EPCalipersCore
{
	/// <summary>
	/// Interaction logic for AboutBox.xaml
	/// </summary>
	public partial class AboutBox : Window
	{
		private AssemblyProperties assemblyProperties = 
			new AssemblyProperties(Assembly.GetEntryAssembly());

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
			Debug.WriteLine(assemblyProperties.AssemblyConfigurationAttribute);
		}

		private void AdditionalInitiation()
		{
			Title = String.Format(CultureInfo.CurrentCulture,
				"About {0}", assemblyProperties.AssemblyTitle);
			AppName.Text = assemblyProperties.AssemblyProduct;
#if DEBUG
			SetVersion(true);
#else
			SetVersion();
#endif
			Copyright.Text = assemblyProperties.AssemblyCopyright;
			SetCompany();
			SetConfiguration();
			DescriptionText.Text = assemblyProperties.AssemblyDescription;
		}

		private void SetCompany()
		{
			Run run = new Run(assemblyProperties.AssemblyCompany);
			Hyperlink hyperlink = new Hyperlink(run)
			{
				NavigateUri = new Uri("https://www.epstudiossoftware.com")
			};
			hyperlink.RequestNavigate += Hyperlink_RequestNavigate;
			Company.Text = null;
			Company.Inlines.Add(hyperlink);
		}

		private void SetConfiguration()
		{
			string configuration = assemblyProperties.AssemblyConfigurationAttribute;
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
					"Version {0} ({1})", assemblyProperties.AssemblyVersion, assemblyProperties.AssemblyFileVersion);
				return;
			}
			this.Version.Text = String.Format(CultureInfo.CurrentCulture,
				"Version {0}", assemblyProperties.AssemblyVersion);

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
