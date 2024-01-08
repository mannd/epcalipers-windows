using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models.Calipers;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
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

		public Bounds Bounds => new(ActualWidth, ActualHeight);

		public void Add(Line line)
		{
			if (line != null) Children.Add(line);
		}
		public void Remove(Line line)
		{
			Children.Remove(line);
		}
		public void Add(TextBlock textBlock)
		{
			Children.Add(textBlock);
		}
		public void Remove(TextBlock textBlock)
		{
			Children.Remove(textBlock);
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

	}

	public class FakeCaliperView : ICaliperView
	{
		public Bounds Bounds => new(800, 400);

		public void Add(Line line)
		{
			Debug.Print($"{line} added.");
		}

		public void Add(TextBlock textBlock)
		{
			Debug.Print($"{textBlock.Text} added.");
		}

		public void Remove(TextBlock textBlock)
		{
			Debug.Print($"{textBlock.Text} removed.");
		}

		public Point GetOffsettedCenter()
		{
			throw new NotImplementedException();
		}

		public void Remove(Line line)
		{
			Debug.Print($"{line} removed.");
		}

		public XamlRoot XamlRoot
		{
			get { return null; }
		}

	}
}
