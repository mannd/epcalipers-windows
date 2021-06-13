using EPCalipersCore.Properties;
using System;
using System.Windows.Forms;

namespace epcalipers
{
	public partial class PreferencesDialog : Form
	{

		readonly Preferences preferences;
		public PreferencesDialog()
		{
			InitializeComponent();
			preferences = new Preferences();
		}

		private void PreferencesDialog_Load(object sender, EventArgs e)
		{
			propertyGrid1.SelectedObject = preferences;

		}

		public void Save()
		{
			preferences.Save();
		}
	}
}
