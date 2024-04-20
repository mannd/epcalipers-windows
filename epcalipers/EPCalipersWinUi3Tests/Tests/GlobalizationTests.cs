using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using EPCalipersWinUI3.Helpers;
using Windows.Globalization;

namespace EPCalipersWinUi3Tests.Tests
{
	public class GlobalizationTests
	{
		[Fact]
		public void TestGlobalization()
		{
			string language = GlobalizationHelper.GetUserLanguage();
			Assert.Equal("en-US", language);

			// Below gives exception.
			// Set the desired language (e.g., "fr-FR" for French) 
			//ApplicationLanguages.PrimaryLanguageOverride = "fr-FR";
			//language = GlobalizationHelper.GetUserLanguage();
			//Assert.Equal("fr-FR", language);
		}
	}
}
