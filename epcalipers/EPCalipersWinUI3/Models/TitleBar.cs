using EPCalipersWinUI3.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.Models
{
	public enum TitleBarRole
	{
		Default,
		AppNameOnly,
		SinglePageFile,
		MultiPageFile,
		TransparentWindow,
		Screenshot
	}

	// TODO: add in Multipage titles with page numbers.
	// TODO: store titlebars in Settings for retrieval when switching modes.
	public struct TitleBar
	{
		public string AppName { get; init; }
		public string FileName { get; set; }
		public string TransparentWindowName { get; init; }

		public string ScreenshotName { get; init; }
		public TitleBarRole Role { get; set; }

		public TitleBar(string appName, 
			string transparentWindowName,
			string screenshotName,
			string fileName = "", 
			TitleBarRole role = TitleBarRole.Default)
		{
			AppName = appName;
			TransparentWindowName = transparentWindowName;
			ScreenshotName = screenshotName;
			FileName = fileName;
			Role = role;
		}

		public string FullName
		{
			get
			{
				switch (Role)
				{
					case TitleBarRole.Default:
						return AppName;
					case TitleBarRole.SinglePageFile:
						return ConcatName(FileName);
					case TitleBarRole.TransparentWindow:
						return ConcatName(TransparentWindowName);
					case TitleBarRole.Screenshot:
						return ConcatName(ScreenshotName);
					default:
						return AppName;
				}
			}
		}

		private string ConcatName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return AppName;
			}
			else
			{
				return AppName + " - " + name;
			}
		}
	}
}
