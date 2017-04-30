﻿using epcalipers.Properties;
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
        public bool Locked { get; set; }
        // this is the (sole) caliper that is selected/highlighted
        /// TODO: check - replaced by GetActiveCaliper()?
        private Caliper ActiveCaliper { get; set; }
        public Calibration HorizontalCalibration { get; set; }
        public Calibration VerticalCalibration { get; set; }

        // for caliper movement
        private Caliper grabbedCaliper;
        private bool crossbarGrabbed = false;
        private bool bar1Grabbed = false;
        private bool bar2Grabbed = false;
        private bool caliperWasDragged = false;

        // for color and tweaking
        private Caliper chosenCaliper;
        private CaliperComponent chosenComponent;
        private bool tweakingComponent;
        // FIXME: change to lower value (0.5?) after debugging
        private float tweakDistance = 1;
 

        public Calipers()
        {
            Locked = false;
            ActiveCaliper = null;
            grabbedCaliper = null;
            HorizontalCalibration = new Calibration(CaliperDirection.Horizontal);
            VerticalCalibration = new Calibration(CaliperDirection.Vertical);
            chosenCaliper = null;
            chosenComponent = CaliperComponent.NoComponent;
            tweakingComponent = false;
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

        public void deleteAllCalipers()
        {
            calipers.Clear();
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

        // returns true if caliper selected changes
        public bool SelectCaliperIfNoneSelected()
        {
            bool selectionMade = false;
            if (calipers.Count > 0 && NoCaliperIsSelected())
            {
                SelectCaliper(calipers[calipers.Count - 1]);
                selectionMade = true;
            }
            return selectionMade;
        }

        public bool NoTimeCaliperSelected()
        {
            return (calipers.Count < 1 ||
                NoCaliperIsSelected() ||
                GetActiveCaliper().Direction == CaliperDirection.Vertical) ||
                GetActiveCaliper().isAngleCaliper;
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

        public void SelectSoleCaliper()
        {
            if (calipers.Count != 1)
            {
                return;
            }
            SelectCaliper(calipers[0]);
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
            if (Locked)
            {
                return false; ;
            }
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
                else if (c.PointNearBar1(point) && caliper == null)
                {
                    bar1Grabbed = true;
                    caliper = c;
                }
                else if (c.PointNearBar2(point) && caliper == null)
                {
                    bar2Grabbed = true;
                    caliper = c;
                }
            }
            grabbedCaliper = caliper;
        }

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

        public void SetChosenCaliper(Point point)
        {
            chosenCaliper = getGrabbedCaliper(point);
        }

        // assumes chosenCaliper is not selected (e.g. highlighted)
        public void SetChosenCaliperColor(Color color)
        {
            if (chosenCaliper == null)
            {
                return;
            }
            chosenCaliper.CaliperColor = color;
            chosenCaliper.UnselectedColor = color;
        }

        public void UnselectChosenCaliper()
        {
            if (chosenCaliper == null)
            {
                return;
            }
            UnselectCaliper(chosenCaliper);
        }

        public Color GetChosenCaliperColor()
        {
            if (chosenCaliper == null)
            {
                // shouldn't happen, but just in case
                return Color.Blue;
            }
            return chosenCaliper.UnselectedColor;
        }

        public void SetChosenCaliperComponent(Point point)
        {
            if (chosenCaliper == null)
            {
                chosenComponent = CaliperComponent.NoComponent;
            }
            else
            {
                chosenComponent = GetCaliperComponent(chosenCaliper, point);
            }
        }

        private CaliperComponent GetCaliperComponent(Caliper caliper, Point point)
        {
            if (caliper == null)
            {
                return CaliperComponent.NoComponent;
            }
            if (caliper.PointNearBar1(point))
            {
                return caliper.Direction == CaliperDirection.Horizontal ? CaliperComponent.LeftBar : CaliperComponent.UpperBar;
            }
            else if (caliper.PointNearBar2(point))
            {
                return caliper.Direction == CaliperDirection.Horizontal ? CaliperComponent.RightBar : CaliperComponent.LowerBar;
            }
            else if (caliper.PointNearCrossbar(point))
            {
                return caliper.isAngleCaliper ? CaliperComponent.Apex : CaliperComponent.CrossBar;
            }
            else
            {
                return CaliperComponent.NoComponent;
            }
            
        }



        public bool DragGrabbedCaliper(float deltaX, float deltaY, PointF location)
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
                    grabbedCaliper.MoveCrossbar(delta);
                }
                else if (bar1Grabbed)
                {
                    grabbedCaliper.MoveBar1(delta, location);
                }
                else if (bar2Grabbed)
                {
                    grabbedCaliper.MoveBar2(delta, location);
                }
                needsRefresh = true;
                caliperWasDragged = true;
            }
            return needsRefresh;
        }

        public bool ReleaseGrabbedCaliper(int clickCount)
        {
            bool needsRefresh = false;
            if (grabbedCaliper != null)
            {
                if (!caliperWasDragged && !Locked)
                {
                    if (clickCount == 1)
                    {
                        ToggleCaliperState(grabbedCaliper);
                        needsRefresh = true;
                    }
                }
                grabbedCaliper = null;
                bar1Grabbed = false;
                bar2Grabbed = false;
                crossbarGrabbed = false;
                caliperWasDragged = false;
            }
            return needsRefresh;
        }

        private void ToggleCaliperState(Caliper c)
        {
            if (c == null)
            {
                return;
            }
            if (c.IsSelected)
            {
                UnselectCaliper(c);
            }
            else
            {
                SelectCaliper(c);
            }
            UnselectCalipersExcept(c);
        }

        public void UnselectCalipersExcept(Caliper c)
        {
            foreach (var caliper in calipers)
            {
                if (caliper != c)
                {
                    UnselectCaliper(caliper);
                }
            }
        }

        public int NumberOfCalipers()
        {
            return calipers.Count;
        }

        public bool PointIsNearCaliper(Point p)
        {
            bool pointNearCaliper = false;
            foreach(var c in calipers)
            {
                if (c.PointNearCaliper(p)) {
                    pointNearCaliper = true;
                    break;
                }
            }
            return pointNearCaliper;
        }

        public bool updateCalibration(double zoomFactor)
        {
            if (HorizontalCalibration.Calibrated ||
                VerticalCalibration.Calibrated)
            {
                HorizontalCalibration.CurrentZoom = zoomFactor;
                VerticalCalibration.CurrentZoom = zoomFactor;
            }
            return NumberOfCalipers() > 0;
        }

        public Caliper getLoneTimeCaliper()
        {
            Caliper c = null;
            int n = 0;
            if (calipers.Count > 0)
            {
                foreach (Caliper caliper in calipers)
                {
                    if (caliper.Direction == CaliperDirection.Horizontal
                        && !caliper.isAngleCaliper)
                    {
                        c = caliper;
                        n++;
                    }
                }
            }
            if (n == 1)
            {
                return c;
            }
            else
            {
                return null;
            }
        }

        public void showHandles(bool value)
        {
            foreach (Caliper c in calipers)
            {
                c.hasHandles = value;
            }
        }

        public void UpdatePreferences(Preferences p)
        {
            foreach (Caliper caliper in calipers)
            {
                caliper.LineWidth = p.LineWidth;
                //caliper.UnselectedColor = p.CaliperColor;
                caliper.SelectedColor = p.HighlightColor;
                if (caliper.IsSelected)
                {
                    caliper.CaliperColor = caliper.SelectedColor;
                }
                else
                {
                    //caliper.CaliperColor = caliper.UnselectedColor;
                }
                caliper.RoundMsecRate = p.RoundMsecRate;
            }
        }

    
    }
}
