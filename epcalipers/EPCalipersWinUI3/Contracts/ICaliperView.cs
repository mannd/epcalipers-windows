using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPCalipersWinUI3.Calipers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;

namespace EPCalipersWinUI3.Contracts
{
    public interface ICaliperView
    {
        public Bounds Bounds { get; }

		public void Add(Line line);

        public void Add(CaliperLabel caliperLabel);

		//public void Remove(TextBlock textBlock);

		public void Remove(Line line);

		public Point GetOffsettedCenter();
    }
}
