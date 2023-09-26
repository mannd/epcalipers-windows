using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.Helpers
{
	public class MessageDialog
	{
		public static ContentDialog Create(string title, string message)
		{
			var dialog = new ContentDialog();
			dialog.Title = title;
			dialog.Content = message;
			dialog.PrimaryButtonText = "OK";
			return dialog;
		}
	}
}
