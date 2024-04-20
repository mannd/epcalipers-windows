using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;

namespace EPCalipersWinUI3.Helpers
{
	public class GlobalizationHelper
	{
		public static string GetUserLanguage()
		{
			// Get the user's preferred language from system settings
			var userLanguages = ApplicationLanguages.Languages;
			if (userLanguages.Count > 0)
			{
				// Return the first language in the list (highest priority)
				return userLanguages[0];
			}

			// Fallback: Return a default language (e.g., English)
			return "en-US";
		}

		// Usage example:
		//string userLanguage = GetUserLanguage();
		//System.Diagnostics.Debug.WriteLine($"User language: {userLanguage}");

	}
}
