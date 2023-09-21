using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Devices;

namespace EPCalipersWinUI3.Models
{
    public enum ComponentDirection
    {
        Horizontal,
        Vertical,
        Angle
    }


    /// <summary>
    /// One of the bars of a caliper.
    /// </summary>
    public class CaliperComponent
    {
		public CaliperComponent(ComponentDirection direction = ComponentDirection.Horizontal,
            int position = 0, bool isSelected = false , bool isChosen = false)
		{
			Direction = direction;
			Position = position;
			IsSelected = isSelected;
			IsChosen = isChosen;
		}

        public ComponentDirection Direction { get; set; }
		public int Position { get; set; }
        public bool IsSelected { get; set; }
        public bool IsChosen { get; set; }

        public void Move(int distance)
        {
            Position += distance;
        }

    }
}
