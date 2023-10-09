﻿using EPCalipersWinUI3.Contracts;
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
using Windows.Gaming.XboxLive.Storage;

namespace EPCalipersWinUI3.Calipers
{

    public sealed class TimeCaliper : Caliper
    {
        private Bounds _bounds;

        public CaliperComponent LeftBar { get; set; }
        public CaliperComponent RightBar { get; set; }
        public CaliperComponent CrossBar { get; set; }

        /// <summary>
        /// The text displayed adjacent to the caliper, showing the value in points
        /// or calibrated units.
        /// </summary>
        public string Text { get; set; }  // TODO: a class for the Text of a caliper

        private bool _fakeComponentLines;
    
        public CaliperComponent[] CaliperComponents { get; set; }

        /// <summary>
        /// A caliper measuring time, with vertical bars and a horizontal crossbar.
        /// </summary>
        /// <param name="boundsWidth">The width of the view containing the caliper.</param>
        /// <param name="boundsHeight">The height of the view containing the caliper.</param>
        public TimeCaliper(Bounds bounds, bool fakeComponentLines = false)
        {
            _bounds = bounds;
            _fakeComponentLines = fakeComponentLines;
            SetInitialPosition();
            CaliperComponents =  new[] { LeftBar, RightBar, CrossBar };
            SetThickness(2);
        }

        protected override CaliperComponent[] GetCaliperComponents()
        {
            return CaliperComponents;
        }

        private void SetInitialPosition()
        {
            if (_fakeComponentLines)
            {
                var fakeComponentLine = new FakeComponentLine();
                LeftBar = new CaliperComponent(CaliperComponent.Role.Vertical, 100, 0, _bounds.Height, fakeComponentLine);
                RightBar = new CaliperComponent(CaliperComponent.Role.Vertical, 300, 0, _bounds.Height, fakeComponentLine);
                CrossBar = new CaliperComponent(CaliperComponent.Role.HorizontalCrossBar, 300, 100, 300, fakeComponentLine);
            }
            else
            {
                // temp set initial positions here.
                LeftBar = new CaliperComponent(CaliperComponent.Role.Vertical, 100, 0, _bounds.Height);
                RightBar = new CaliperComponent(CaliperComponent.Role.Vertical, 300, 0, _bounds.Height);
                CrossBar = new CaliperComponent(CaliperComponent.Role.HorizontalCrossBar, 300, 100, 300);
            }
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

        public override double Value()
        {
            return RightBar.Position - LeftBar.Position;
		}


		public override CaliperComponent IsNearComponent(Point p)
		{
            foreach (var component in CaliperComponents)
            {
                if (component.IsNear(p))
                {
                    return component;
                }
            }
            return null;
		}

		public override void Add(Grid grid)
		{
            grid.Children.Add(LeftBar.GetComponent());
            grid.Children.Add(RightBar.GetComponent());
            grid.Children.Add(CrossBar.GetComponent());
		}

		public override void Delete(Grid grid)
		{
            grid.Children.Remove(LeftBar.GetComponent());
            grid.Children.Remove(RightBar.GetComponent());
            grid.Children.Remove(CrossBar.GetComponent());
		}


		public override void Drag(CaliperComponent component, Point delta)
		{
            if (component == LeftBar)
            {
				component.X1 += delta.X;
				component.X2 += delta.X;
				CrossBar.X1 += delta.X;
			}
			else if (component == RightBar)
            {
				component.X1 += delta.X;
				component.X2 += delta.X;
				CrossBar.X2 += delta.X;
			}
			else if (component == CrossBar)
            {
                LeftBar.X1 += delta.X;
                LeftBar.X2 += delta.X;
                RightBar.X1 += delta.X;
                RightBar.X2 += delta.X;
                CrossBar.X1 += delta.X;
                CrossBar.X2 += delta.X;
                CrossBar.Y1 += delta.Y;
                CrossBar.Y2 += delta.Y;
			}
		}
	}
}
