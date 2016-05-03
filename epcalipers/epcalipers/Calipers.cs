using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epcalipers
{
    // This class manages the set of calipers on the screen
    public class Calipers
    {
        List<Caliper> calipers = new List<Caliper>();
        bool Locked { get; set; }
        Caliper ActiveCaliper { get; set; }

        Caliper grabbedCaliper;
        bool crossbarGrabbed = false;
        bool bar1Grabbed = false;
        bool bar2Grabbed = false;
        bool caliperWasDragged = false;


        public Calipers()
        {
            Locked = false;
            ActiveCaliper = null;
            grabbedCaliper = null;
            
        }

        public void Draw(Graphics g, RectangleF rect)
        {
            foreach (var c in calipers)
            {
                c.Draw(g, rect);
            } 
        }

        public void addCaliper(Caliper c)
        {
            calipers.Add(c);
        }

        public void deleteCaliper(Caliper c)
        {
            calipers.Remove(c);
        }

        // this is the highlighted/selected caliper used for calibration or measurements
        public Caliper GetActiveCaliper()
        {
            Caliper c = null;
            for (int i = calipers.Count -1; i >= 0 && c == null; i--)
            {
                if (calipers[i].IsSelected)
                {
                    c = calipers[i];
                }
            }
            return c;
        }

        public bool NoCaliperIsSelected()
        {
            return GetActiveCaliper() == null;
        }

        public void SelectCaliperIfNoneSelected()
        {
            if (calipers.Count > 0 && NoCaliperIsSelected())
            {
                SelectCaliper(calipers[calipers.Count - 1]);
            }
        }

        public void SelectCaliper(Caliper c)
        {
            c.CaliperColor = c.SelectedColor;
            c.IsSelected = true;
        }

        public void UnselectCaliper(Caliper c)
        {
            c.CaliperColor = c.UnselectedColor;
            c.IsSelected = false;
        }

        public bool ToggleCaliperIfClicked(Point point)
        {
            bool caliperToggled = false;
            for (int i = calipers.Count - 1; i >= 0; i--)
            {
                if (calipers[i].PointNearCaliper(point) && !caliperToggled)
                {
                    caliperToggled = true;
                    if (calipers[i].IsSelected)
                    {
                        UnselectCaliper(calipers[i]);
                    }
                    else
                    {
                        SelectCaliper(calipers[i]);
                    }
                }
                else
                {
                    UnselectCaliper(calipers[i]);
                }
            }
            return caliperToggled;
        }

        public bool DeleteCaliperIfClicked(Point point)
        {
            bool deleted = false;
            for (int i = calipers.Count - 1; i >= 0; i--)
            {
                if (calipers[i].PointNearCaliper(point) && !deleted)
                {
                    deleteCaliper(calipers[i]);
                    deleted = true;
                }
            }
            return deleted;
        }

        // This shortens the caliper grabbing process c/w the other versions of EP Calipers
        public void GrabCaliperIfClicked(Point point)
        {
            Caliper caliper = null;
            foreach (var c in calipers)
            {
                if (c.PointNearCrossbar(point) && caliper == null)
                {
                    crossbarGrabbed = true;
                    caliper = c;
                }
                else if (c.PointNearBar(point, c.Bar1Position) && caliper == null)
                {
                    bar1Grabbed = true;
                    caliper = c;
                }
                else if (c.PointNearBar(point, c.Bar2Position) && caliper == null)
                {
                    bar2Grabbed = true;
                    caliper = c;
                }
            }
            grabbedCaliper = caliper;
        }

        // below may not be needed
        private Caliper getGrabbedCaliper(Point point)
        {
            Caliper caliper = null;
            foreach (var c in calipers)
            {
                if (c.PointNearCaliper(point) && caliper == null) {
                    caliper = c;
                }
            }
            return caliper;
        }

        public bool DragGrabbedCaliper(float deltaX, float deltaY)
        {
            bool needsRefresh = false;
            if (grabbedCaliper != null)
            {
                PointF delta = new PointF(deltaX, deltaY);
                if (grabbedCaliper.Direction == CaliperDirection.Vertical)
                {
                    float tmp = delta.X;
                    delta.X = delta.Y;
                    delta.Y = tmp;
                }
                if (crossbarGrabbed)
                {
                    grabbedCaliper.Bar1Position += delta.X;
                    grabbedCaliper.Bar2Position += delta.X;
                    grabbedCaliper.CrossbarPosition += delta.Y;
                }
                else if (bar1Grabbed)
                {
                    grabbedCaliper.Bar1Position += delta.X;
                }
                else if (bar2Grabbed)
                {
                    grabbedCaliper.Bar2Position += delta.X;
                }
                needsRefresh = true;
            }
            return needsRefresh;
        }

        public void ReleaseGrabbedCaliper()
        {
            grabbedCaliper = null;
            bar1Grabbed = false;
            bar2Grabbed = false;
            crossbarGrabbed = false;
        }

    
    }
}
