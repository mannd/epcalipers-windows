using epcalipers.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EPCalipersCore.Properties;

namespace epcalipers
{
    public partial class PreferencesDialog : Form
    {

        Preferences preferences;
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
