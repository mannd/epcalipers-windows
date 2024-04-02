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
		public void SetColor(Color color)
		{
			if (Caliper != null)
			{
				Caliper.UnselectedColor = color;
				Caliper.UnselectFullCaliper();
			}
		}

		[ObservableProperty]
		private Color caliperColor;

	}
}
