using Windows.Foundation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EPCalipersWinUI3.Calipers
{
    public enum CaliperLabelPosition
    {
        Top,
        Bottom,
        Left,
        Right,
    }

    public class CaliperLabel
    {
        public string Text { get; set; }
        public CaliperLabelPosition Position { get; set;}
        public bool AutoPosition { get; set;}

        private TextBlock _textBlock;
        private Rectangle _boundingBox;

        public CaliperLabel() 
        {
            _textBlock = new TextBlock();
            _boundingBox = new Rectangle();
        }

        public void DrawLabel(string s)
        {
            Debug.WriteLine(s);
        }



    }
}
