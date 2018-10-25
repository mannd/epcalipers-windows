using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using EPCalipersCore;

namespace WpfTransparentWindow
{
    class CalipersCanvas: Canvas, ICalipers
    {
        BaseCalipers calipers = new BaseCalipers();
        public Calibration HorizontalCalibration {
            get
            {
                return calipers.HorizontalCalibration;
            }
            set
            {
                calipers.HorizontalCalibration = value;
            }
        }
        public Calibration VerticalCalibration {
            get
            {
                return calipers.VerticalCalibration;
            }
            set
            {
                calipers.VerticalCalibration = value;
            }
        }

        public CalipersCanvas(): base()
        {

        }

        public void AddCaliper(BaseCaliper c)
        {
            calipers.addCaliper(c);
        }

        public void DrawCalipers()
        {
            // This ensures deleted caliper disappears
            Children.Clear();
            foreach(BaseCaliper c in calipers.GetCalipers())
            {
                DrawCaliper(c);
            }
        }

        private void DrawCaliper(BaseCaliper c)
        {
            c.Draw(this);
        }

        public void GrabCaliperIfClicked(System.Windows.Point point) 
        {
            calipers.GrabCaliperIfClicked(ConvertPoint(point));
        }

        private System.Drawing.Point ConvertPoint(System.Windows.Point point)
        {
            return new System.Drawing.Point((int)point.X, (int)point.Y);
        }

        public bool DragGrabbedCaliper(float deltaX, float deltaY, System.Windows.Point location)
        {
            return calipers.DragGrabbedCaliper(deltaX, deltaY, new System.Drawing.PointF((float)location.X, (float)location.Y));
        }

        public bool ReleaseGrabbedCaliper(int clickCount)
        {
            return calipers.ReleaseGrabbedCaliper(clickCount);
        }

        public bool DeleteCaliperIfClicked(System.Windows.Point point)
        {
            return calipers.DeleteCaliperIfClicked(ConvertPoint(point));
        }

        public int NumberOfCalipers()
        {
            return calipers.NumberOfCalipers();
        }

        public bool NoCaliperIsSelected()
        {
            return calipers.NoCaliperIsSelected();
        }

        public void SelectSoleCaliper()
        {
            calipers.SelectSoleCaliper();
        }

        public BaseCaliper GetActiveCaliper()
        {
            return calipers.GetActiveCaliper();
        }

        public BaseCaliper GetLoneTimeCaliper()
        {
            return calipers.GetLoneTimeCaliper();
        }

        public void SelectCaliper(BaseCaliper c)
        {
            calipers.SelectCaliper(c);
        }

        public void UnselectCalipersExcept(BaseCaliper c)
        {
            calipers.UnselectCalipersExcept(c);
        }
    }
}
