using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPCalipersWinUI3.Calipers;
using Microsoft.UI.Xaml.Shapes;

namespace EPCalipersWinUI3.Contracts
{
    public interface ICaliperView
    {
        public Bounds Bounds { get; }

		public void Add(Line line);

		public void Remove(Line line);
    }
}
