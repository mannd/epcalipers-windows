using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EPCalipersWinUI3.Models
{
	public class AppInfo
	{
		readonly Assembly assembly;

		public AppInfo(Assembly assembly = null)
		{
			if (assembly == null)
			{
				this.assembly = Assembly.GetExecutingAssembly();
			}
			else
				this.assembly = assembly;
		}

		public string ProductVersion =>
				FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion ?? "Unknown product version";

		public string FileVersion =>
				FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion ?? "Unknown file version";

		public string Title
		{
			get
			{
				object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				if (attributes.Length > 0)
				{
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
					if (!String.IsNullOrEmpty(titleAttribute.Title))
					{
						return titleAttribute.Title;
					}
				}
				return System.IO.Path.GetFileNameWithoutExtension(assembly.Location);
			}
		}

		public string ProductName
		{
			get
			{
				object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyProductAttribute)attributes[0]).Product;
			}
		}

		public string Copyright
		{
			get
			{
				object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}
		public string CpuArchitecture => AssemblyProperties.CpuArchitecture;

		public string Company
		{
			get
			{
				object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCompanyAttribute)attributes[0]).Company;
			}
		}

		public string SaneProductVersion
		{
			get
			{
				return GetUntilOrOriginal(ProductVersion);
			}
		}

		private static string GetUntilOrOriginal(string text, string stopAt = "+")
		{
			if (!String.IsNullOrWhiteSpace(text))
			{
				int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);
				if (charLocation > 0)
				{
					return text[..charLocation];
				}
			}
			return text;
		}

	}
}