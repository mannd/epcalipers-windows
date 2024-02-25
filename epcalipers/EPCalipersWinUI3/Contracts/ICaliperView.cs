using EPCalipersWinUI3.Models.Calipers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;

namespace EPCalipersWinUI3.Contracts
{
	public interface ICaliperView
	{
		public Bounds Bounds { get; }

		public void Add(Line line);

		public void Add(TextBlock textBlock);

		public void Remove(TextBlock textBlock);

		public void Remove(Line line);

		public Point GetOffsettedCenter();

		public XamlRoot XamlRoot { get; }

		public void UpdateLayout();
	}
}
