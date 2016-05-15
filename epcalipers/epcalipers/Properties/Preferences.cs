using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epcalipers.Properties
{
    public class Preferences
    {
        private Settings settings;

        public Preferences()
        {
            settings = new Settings();
            caliperColor = settings.CaliperColor;
        }

        private Color caliperColor;
        [Browsable(true),
            ReadOnly(false),
            Description("Unselected caliper color"),
            DisplayName("Caliper color")]
        public Color CaliperColor
        {
            get { return caliperColor; }
            set { caliperColor = value; }
        }

        public void SavePreferences()
        {
            settings.CaliperColor = caliperColor;
            settings.Save();
        }

    }
}
