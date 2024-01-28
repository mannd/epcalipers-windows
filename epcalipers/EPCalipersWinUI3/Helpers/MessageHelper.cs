using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;

namespace EPCalipersWinUI3.Helpers
{
	public class MessageHelper
	{
		public static ContentDialog CreateMessageDialog(string title, string message)
		{
			Debug.Print(message);
			var dialog = new ContentDialog();
			dialog.Title = title;
			dialog.Content = message;
			dialog.PrimaryButtonText = "OK".GetLocalized();
			return dialog;
		}

		public static ContentDialog CreateErrorDialog(string message)
		{
			var title = "Error".GetLocalized();
			return CreateMessageDialog(title, message);
		}
	}
}
