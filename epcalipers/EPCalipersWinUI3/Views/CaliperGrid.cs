using EPCalipersWinUI3.Calipers;
using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Helpers;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments.AppointmentsProvider;
using Windows.Foundation;

namespace EPCalipersWinUI3.Views
{
    public partial class CaliperGrid : Grid, ICaliperView
	{
		public InputCursor InputCursor
		{
			get => ProtectedCursor;
			set => ProtectedCursor = value;
		}

		public Bounds Bounds => new Bounds(ActualWidth, ActualHeight);

		public void Add(Line barLine)
		{
			Children.Add(barLine);
		}

		public void Add(TextBlock textBlock)
		{
			Children.Add(textBlock);
		}

		private static double _offset = 0;
		private readonly static double _offsetIncrement = 10;
		private readonly static double _maxOffset = 100;
		public Point GetOffsettedCenter()
		{
			Point center = MathHelper.Center(Bounds);
			center = MathHelper.OffsetPoint(center, _offset);
			_offset += _offsetIncrement;
			if (_offset > _maxOffset) _offset = 0;
			return center;
		}

		public void Remove(Line barLine)
		{
			Children.Remove(barLine);
		}
	}

	public class FakeCaliperView : ICaliperView
	{
		public Bounds Bounds => new Bounds(800, 400);

		public void Add(Line line)
		{
			Debug.Print($"{line} added.");
		}

		public void Add(TextBlock textBlock)
		{
			Debug.Print($"{textBlock.Text} added.");
		}

		public Point GetOffsettedCenter()
		{
			throw new NotImplementedException();
		}

		public void Remove(Line line)
		{
			Debug.Print($"{line} removed.");
		}
	}
}
