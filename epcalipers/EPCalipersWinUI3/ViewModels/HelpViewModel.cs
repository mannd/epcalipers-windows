using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace EPCalipersWinUI3.ViewModels
{
	public partial class HelpViewModel : BasePageViewModel
	{
		[ObservableProperty]
		private string source;

		[ObservableProperty]
		private bool isLoading;
	}
}
