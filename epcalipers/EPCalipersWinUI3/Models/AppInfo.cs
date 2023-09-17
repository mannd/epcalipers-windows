using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.Models
{
	public class AppInfo
	{
		Assembly assembly;

		public AppInfo(Assembly assembly = null)
		{
			if (assembly == null)
			{
				this.assembly = Assembly.GetExecutingAssembly();
			} else 
				this.assembly = assembly;
		}

    public string AssemblyVersion
    {
        get
        {
            return FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion ?? "Unknown version";
        }
    }

	}
}
