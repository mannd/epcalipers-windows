﻿using CommunityToolkit.Mvvm.ComponentModel;
using EPCalipersWinUI3.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.ViewModels
{
	public class BasePageViewModel: ObservableObject
	{
		private string _titleBarExtension = "";

		public void SetTitleBarName(string extension)
		{
			_titleBarExtension = extension;
			var appName = AppHelper.AppDisplayName;
			string titleBarName;
			if (string.IsNullOrEmpty(extension))
			{
				titleBarName = appName;
			}
			else
			{
				titleBarName = appName + " - " + extension;
			}
			if (AppHelper.AppMainWindow != null)
			{
				AppHelper.AppTitleBarText = titleBarName;
			}
		}

		public void RestoreTitleBarName()
		{
			SetTitleBarName(_titleBarExtension);
		}
	}
}