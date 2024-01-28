using CommunityToolkit.Mvvm.ComponentModel;
using EPCalipersWinUI3.Models.Calipers;
using System.ComponentModel;
using Windows.UI;

namespace EPCalipersWinUI3.ViewModels
{

	public partial class ColorViewModel : ObservableObject
	{
		public Caliper Caliper { get; set; }

		public ColorViewModel(Caliper caliper)
		{
			Caliper = caliper;
			CaliperColor = Caliper.UnselectedColor;
		}
		protected override void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.PropertyName == nameof(CaliperColor))
			{
				Caliper.UnselectedColor = CaliperColor;
				Caliper.IsSelected = false;  // Forces color to be changed even if already unselected.
			}
		}

		[ObservableProperty]
		private Color caliperColor;

	}
}
