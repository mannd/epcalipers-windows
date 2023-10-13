using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace EPCalipersWinUI3.Contracts
{
	public interface IBarLine
	{
		double X1 { get; set; }

		double X2 { get; set; }

		double Y1 { get; set; }

		double Y2 { get; set; }

		Color Color { set; }

		double Width { set; get; }
		Line GetLine();
	}
}
