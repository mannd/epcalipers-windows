using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EPCalipersCore
{
    // Any common static methods I can extract will go here
    public class CommonCaliper
    {
        public static DialogResult GetDialogResult(Form dialog) {
            return dialog.ShowDialog();
        }
    }
}
