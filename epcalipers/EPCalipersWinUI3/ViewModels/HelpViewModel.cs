using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.ViewModels
{
	public partial class HelpViewModel : ObservableObject
	{
		[ObservableProperty]
		private string source = "https://epstudiossoftware.com";
	}
}
