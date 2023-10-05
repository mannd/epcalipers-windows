using EPCalipersWinUI3.Models;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

		public void Draw(TimeCaliper c)
		{
			Line leftLine = c.LeftBar.Line;
            var brush = new SolidColorBrush(Microsoft.UI.Colors.Blue);
            leftLine.Stroke = brush;
            Children.Add(leftLine);
			Line rightLine = c.RightBar.Line;
            rightLine.Stroke = brush;
            Children.Add(rightLine);
			Line crossbar = c.CrossBar.Line;
            crossbar.Stroke = brush;
            Children.Add(crossbar);
		}
		public void Draw(TimeCaliper c, double increment)
		{
			c.RightBar.Line.X1 += increment;
			c.RightBar.Line.X2 += increment;
			c.CrossBar.Line.X2 += increment;
		}

		public void Drag(TimeCaliper c, double x)
		{
			c.RightBar.Line.X1 = x;
			c.RightBar.Line.X2 = x;
			c.CrossBar.Line.X2 = x;
		}
	}
}
