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

        public Calipers()
        {
            Locked = false;
            ActiveCaliper = null;
            
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

        public Caliper GetActiveCaliper()
        {
            Caliper c = null;
            for (int i = calipers.Count -1; i >= 0 && c != null; i--)
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
    }
}
