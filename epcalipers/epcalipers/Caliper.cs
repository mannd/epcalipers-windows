using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPCalipersCore;
using EPCalipersCore.Properties;

namespace epcalipers
{
    public class Caliper: BaseCaliper
    {
        public Caliper(): base()
        {
        }

        public virtual void Draw(Graphics g, RectangleF rect)
        {
            Brush brush = new SolidBrush(CaliperColor);
            Pen pen = new Pen(brush, LineWidth);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            if (Direction == CaliperDirection.Horizontal)
            {
                CrossbarPosition = Math.Min(CrossbarPosition, rect.Size.Height - DELTA);
                CrossbarPosition = Math.Max(CrossbarPosition, DELTA);
                Bar1Position = Math.Min(Bar1Position, rect.Size.Width - DELTA);
                Bar2Position = Math.Max(Bar2Position, DELTA);
                g.DrawLine(pen, Bar1Position, 0.0f, Bar1Position, rect.Size.Height);
                g.DrawLine(pen, Bar2Position, 0.0f, Bar2Position, rect.Size.Height);
                g.DrawLine(pen, Bar2Position, CrossbarPosition, Bar1Position, CrossbarPosition);
            }
            else
            {
                CrossbarPosition = Math.Min(CrossbarPosition, rect.Size.Width - DELTA);
                CrossbarPosition = Math.Max(CrossbarPosition, DELTA);
                Bar1Position = Math.Min(Bar1Position, rect.Size.Height - DELTA);
                Bar2Position = Math.Max(Bar2Position, DELTA);
                g.DrawLine(pen, 0.0f, Bar1Position, rect.Size.Width, Bar1Position);
                g.DrawLine(pen, 0.0f, Bar2Position, rect.Size.Width, Bar2Position);
                g.DrawLine(pen, CrossbarPosition, Bar2Position, CrossbarPosition, Bar1Position);
            }
            if (isMarching && isTimeCaliper())
            {
                drawMarchingCalipers(g, brush, rect);
            }
            CaliperText(g, brush);
            pen.Dispose();
            brush.Dispose();
        }

        protected void CaliperText(Graphics g, Brush brush)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            string text = Measurement();
            SizeF sizeOfString = g.MeasureString(text, TextFont);
            float stringWidth = sizeOfString.Width;
            float stringHeight = sizeOfString.Height;
            float firstBarPosition = Bar2Position > Bar1Position ? Bar1Position : Bar2Position;
            float center = firstBarPosition + (Math.Abs(Bar2Position - Bar1Position) / 2);
            if (Direction == CaliperDirection.Horizontal)
            {
                g.DrawString(text, TextFont, brush, center - stringWidth / 2, CrossbarPosition - 30);
            }
            else
            {
                g.DrawString(text, TextFont, brush, CrossbarPosition + 5, center - stringHeight / 2);
            }
        }

        private void drawMarchingCalipers(Graphics g, Brush brush, RectangleF rect)
        {
            // note that pen width < 1 (e.g. 0) will always just draw as width of 1
            Pen pen = new Pen(brush, LineWidth - 1.0f);
            float difference = Math.Abs(Bar1Position - Bar2Position);
            if (difference < minDistanceForMarch)
            {
                return;
            }
            float greaterBar = Math.Max(Bar1Position, Bar2Position);
            float lesserBar = Math.Min(Bar1Position, Bar2Position);
            float[] biggerBars = new float[maxMarchingCalipers];
            float[] smallerBars = new float[maxMarchingCalipers];
            float point = greaterBar + difference;
            int index = 0;
            while (point < rect.Size.Width && index < maxMarchingCalipers)
            {
                biggerBars[index] = point;
                point += difference;
                index++;
            }
            int maxBiggerBars = index;
            index = 0;
            point = lesserBar - difference;
            while (point > 0 && index < maxMarchingCalipers)
            {
                smallerBars[index] = point;
                point -= difference;
                index++;
            }
            int maxSmallerBars = index;
            // draw them
            int i = 0;
            while (i < maxBiggerBars)
            {
                g.DrawLine(pen, biggerBars[i], 0, biggerBars[i], rect.Size.Height);
                i++;
            }
            i = 0;
            while (i < maxSmallerBars)
            {
                g.DrawLine(pen, smallerBars[i], 0, smallerBars[i], rect.Size.Height);
                i++;
            }
            pen.Dispose();
        }
    }
}
