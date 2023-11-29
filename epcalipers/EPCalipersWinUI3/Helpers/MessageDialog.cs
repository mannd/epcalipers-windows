using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.Views;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.WebUI;
using WinUIEx;
using EPCalipersWinUI3;

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

		//public static Calibration GetCalibration()
		//{
		//	if (AppHelper.CalibrationWindow != null)
		//	{
		//		var frame = new Frame();
		//		AppHelper.CalibrationWindow.WindowContent = frame;
		//		frame.Navigate(typeof(CalibrationDialog));
		//		AppHelper.CalibrationWindow.Show();
		//	CalibrationWindow = new WindowEx();
		//	CalibrationWindow.Activate();
		//	CalibrationWindow.Width = 400;
		//	CalibrationWindow.Height = 400;
		//	CalibrationWindow.SetIsAlwaysOnTop(true);
		//	CalibrationWindow.Title = "Calibration";
		//	CalibrationWindow.SetTaskBarIcon(Icon.FromFile("Assets/EpCalipersLargeTemplate1.ico"));
		//	CalibrationWindow.Hide();
		//	}
		//	return new Calibration();

		//	//			//var result = await dialog.ShowAsync();

		//}

		//public static void HideCalibrationWindow()
		//{
		//	if (AppHelper.CalibrationWindow != null)
		//	{
		//		AppHelper.CalibrationWindow.Hide();
		//	}
		//}





		//			//ContentDialog dialog = new ContentDialog();

		//			//XamlRoot must be set in the case of a ContentDialog running in a Desktop app
		//			//dialog.XamlRoot = this.XamlRoot;
		//			//dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
		//			//dialog.Title = "Calibrate this caliper?";
		//			//dialog.PrimaryButtonText = "Calibrate";
		//			//dialog.CloseButtonText = "Cancel";
		//			//dialog.DefaultButton = ContentDialogButton.Primary;
		//			//dialog.Content = new CalibrationDialog();
		//			if (CalibrationWindow == null)
		//			{
		//				CalibrationWindow = new WindowEx();
		//				CalibrationWindow.Width = 400;
		//				CalibrationWindow.Height = 400;
		//				CalibrationWindow.SetIsAlwaysOnTop(true);
		//				CalibrationWindow.SetTaskBarIcon(Icon.FromFile("Assets/EpCalipersLargeTemplate1.ico"));
		//				var frame = new Frame();
		//				CalibrationWindow.WindowContent = frame;
		//				frame.Navigate(typeof(CalibrationDialog));
		//				//CalibrationWindow.WindowContent = new CalibrationDialog();
		//				CalibrationWindow.Title = "Calibration";
		//			}
		//			//window.SetIcon("/Assets/EpCalipersLargeTemplate1.ico");
		//			CalibrationWindow.Show();

		//			//var result = await dialog.ShowAsync();
	}
}
