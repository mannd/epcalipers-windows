using EPCalipersWinUI3.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Input;
using Windows.Foundation;

namespace EPCalipersWinUI3.Calipers
{
    public sealed class TimeCaliper : Caliper
    {
        private Grid _grid;  // Grid that holds the caliper.
        private double _boundsWidth;
        private double _boundsHeight;
        private readonly int margin = 10;

        public CaliperComponent LeftBar { get; set; }
        public CaliperComponent RightBar { get; set; }
        public CaliperComponent CrossBar { get; set; }

        /// <summary>
        /// Color of caliper when not selected.
        /// </summary>
        public Color UnselectedColor { get; set; } = Colors.Blue;

        /// <summary>
        /// Color of caliper when selected.
        /// </summary>
        // TODO: UnselectedColor can be changed on a per caliper basis, but not SelectedColor,
        // so maybe SelectedColor should be system-wide and not per caliper.  This may affect
        // other versions of EP Calipers too (i.e. change SelectedColor in settings, old selected color
        // may still appear when selecting caliper created before the change).
        public Color SelectedColor { get; set; } = Colors.Red;

        /// <summary>
        /// The text displayed adjacent to the caliper, showing the value in points
        /// or calibrated units.
        /// </summary>
        public string Text { get; set; }

        public CaliperComponent[] CaliperComponents { get; set; }

        /// <summary>
        /// A caliper measuring time, with vertical bars and a horizontal crossbar.
        /// </summary>
        /// <param name="boundsWidth">The width of the view containing the caliper.</param>
        /// <param name="boundsHeight">The height of the view containing the caliper.</param>
        public TimeCaliper(Grid grid)
        {
            _grid = grid;
            _boundsWidth = _grid.ActualWidth;
            _boundsHeight = _grid.ActualHeight;

            // temp set initial positions here.
            LeftBar = new CaliperComponent(CaliperComponent.Role.Vertical, 100, 0, _boundsHeight);
            RightBar = new CaliperComponent(CaliperComponent.Role.Vertical, 300, 0, _boundsHeight);
            CrossBar = new CaliperComponent(CaliperComponent.Role.HorizontalCrossBar, 300, 100, 300);
            CaliperComponents =  new[] { LeftBar, RightBar, CrossBar };
            SetThickness(2);
            SetColor(UnselectedColor);
        }

        override protected CaliperComponent[] GetCaliperComponents()
        {
            return CaliperComponents;
        }

        private void SetInitialPositionNearCorner()
        {
            //Point offset = ecgPictureBox.Location;
            //c.initialOffset = new Point(-offset.X, -offset.Y);

            //// init with Horizontal bar offsets
            //int barOffset = _initialOffset.X;
            //int crossbarOffset = _initialOffset.Y;

            //if (Direction == CaliperDirection.Vertical)
            //{
            //	barOffset = _initialOffset.Y;
            //	crossbarOffset = _initialOffset.X;
            //}

            //Bar1Position = 50 + differential + barOffset;
            //Bar2Position = 100 + differential + barOffset;
            //CrossbarPosition = 100 + differential + crossbarOffset;
            //differential += 15.0f;
            //if (differential > 80.0f)
            //{
            //	differential = 0.0f;
            //}
        }

        override public double Value()
        {
            return RightBar.Line.X1 - LeftBar.Line.X1;
		}

		public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                foreach (var component in CaliperComponents)
                {
                    component.IsSelected = value;
                }
            }
        }
        private bool _isSelected;

        public void ToggleIsSelected()
        {
            throw new NotImplementedException();
        }

        override public void Draw()
        {
            _grid.Children.Add(LeftBar.Line);
            _grid.Children.Add(RightBar.Line);
            _grid.Children.Add(CrossBar.Line);
        }

        // Note that calipers are being drawn outside of image.  Is this good or bad?
		override public void Drag(CaliperComponent component, Point delta)
		{
            if (component == LeftBar)
            {
				component.Line.X1 += delta.X;
				component.Line.X2 += delta.X;
				CrossBar.Line.X1 += delta.X;
			}
			else if (component == RightBar)
            {
				component.Line.X1 += delta.X;
				component.Line.X2 += delta.X;
				CrossBar.Line.X2 += delta.X;
			}
			else if (component == CrossBar)
            {
                LeftBar.Line.X1 += delta.X;
                LeftBar.Line.X2 += delta.X;
                RightBar.Line.X1 += delta.X;
                RightBar.Line.X2 += delta.X;
                CrossBar.Line.X1 += delta.X;
                CrossBar.Line.X2 += delta.X;
                CrossBar.Line.Y1 += delta.Y;
                CrossBar.Line.Y2 += delta.Y;
			}
		}
	}
}
