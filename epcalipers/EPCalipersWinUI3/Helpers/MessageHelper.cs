using EPCalipersWinUI3.Models.Calipers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

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

		public static async Task ShowExceptionDialog(Exception ex, string path)
		{
			if (ex == null) return;

			string details = $"File: {path ?? "(unknown)"}\n\nException: {ex.GetType().FullName}\nMessage: {ex.Message}\n\nStackTrace:\n{ex.StackTrace}";

			// Use a read-only TextBlock for display so newlines and wrapping render correctly.
			var detailsBlock = new TextBlock
			{
				Text = details,
				TextWrapping = Microsoft.UI.Xaml.TextWrapping.Wrap,
				Height = 300,
				HorizontalAlignment = HorizontalAlignment.Stretch
			};

			// Use a ScrollViewer to enable vertical scrolling and disable horizontal scrolling.
			var scroll = new ScrollViewer
			{
				Content = detailsBlock,
				VerticalScrollMode = ScrollMode.Enabled,
				VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
				HorizontalScrollMode = ScrollMode.Disabled,
				HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
				Height = 300
			};

			var dialog = new ContentDialog
			{
				Title = "Error opening file",
				Content = scroll,
				PrimaryButtonText = "Copy Details",
				CloseButtonText = "OK",
			};

			// Ensure we have a XamlRoot to show the dialog.
			var mainWindow = AppHelper.AppMainWindow;
			if (mainWindow?.Content != null)
			{
				dialog.XamlRoot = mainWindow.Content.XamlRoot;
			}

			var result = await dialog.ShowAsync();

			if (result == ContentDialogResult.Primary)
			{
				var dp = new Windows.ApplicationModel.DataTransfer.DataPackage();
				dp.SetText(details);
				Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dp);
			}
		}
	}
}
