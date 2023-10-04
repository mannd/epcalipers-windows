using EPCalipersWinUI3.Models;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.Views
{
	public class CustomGrid : Grid
	{

		public InputCursor InputCursor
		{
			get => ProtectedCursor;
			set => ProtectedCursor = value;
		}

		public void Draw(TimeCaliper c, double height)
		{
            Line leftLine = new();
            var brush = new SolidColorBrush(Microsoft.UI.Colors.Blue);
			leftLine.X1 = c.LeftBar.Position;
			leftLine.Y1 = 0;
			leftLine.X2 = c.LeftBar.Position;
			leftLine.Y2 = height;
            leftLine.Stroke = brush;
            Children.Add(leftLine);
			Line rightLine = new();
			rightLine.X1 = c.RightBar.Position;
			rightLine.Y1 = 0;
			rightLine.X2 = c.RightBar.Position;
			rightLine.Y2 = height;
            rightLine.Stroke = brush;
            Children.Add(rightLine);
			Line crossbar = new();
			crossbar.X1 = c.LeftBar.Position;
			crossbar.Y1 = c.CrossBar.Position;
			crossbar.X2 = c.RightBar.Position;
			crossbar.Y2 = c.CrossBar.Position;
            crossbar.Stroke = brush;
            Children.Add(crossbar);

		}
	}
}
