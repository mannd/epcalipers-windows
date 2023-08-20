using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace EPCalipersCore
{
	// See https://referencesource.microsoft.com/#mscorlib/system/reflection/assemblyattributes.cs
    // for more Assembly attritues
	internal class AssemblyProperties
    {
        private Assembly assembly;

		#region Constructor
        public AssemblyProperties(Assembly assembly)
        {
            this.assembly = assembly;
        }
		#endregion

		#region Assembly Attribute Accessors

		public string AssemblyTitle
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

        public string AssemblyConfigurationAttribute
        {
            get
            {
                object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyConfigurationAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyConfigurationAttribute)attributes[0]).Configuration;
                 
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion ?? "Unknown version";
            }
        }

        public string AssemblyFileVersion
        {
            get
            {
                return FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion ?? "Unknown version";
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
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

        public string AssemblyCopyright
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

        public string AssemblyCompany
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
        #endregion

    }
}

