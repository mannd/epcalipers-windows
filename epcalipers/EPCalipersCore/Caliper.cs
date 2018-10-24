using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Globalization;
using System.Diagnostics;
using EPCalipersCore.Properties;

namespace EPCalipersCore
{
    using MBrush = System.Windows.Media.Brush;
    using DBrush = System.Drawing.Brush;

    public class Caliper : BaseCaliper
    {
        Line bar1Line = new Line();
        Line bar2Line = new Line();
        Line crossbarLine = new Line();
        TextBlock textBlock = new TextBlock();

        public Caliper() : base()
        {
        }

        public override void Draw(Canvas canvas)
        {
            var brush = new SolidColorBrush(ConvertColor(CaliperColor));
            canvas.Children.Remove(bar1Line);
            canvas.Children.Remove(bar2Line);
            canvas.Children.Remove(crossbarLine);
            if (Direction == CaliperDirection.Horizontal)
            {
                CrossbarPosition = (float)Math.Min(CrossbarPosition, canvas.ActualHeight - DELTA);
                CrossbarPosition = Math.Max(CrossbarPosition, DELTA);
                Bar1Position = (float)Math.Min(Bar1Position, canvas.ActualWidth - DELTA);
                Bar2Position = Math.Max(Bar2Position, DELTA);
                MakeLine(ref bar1Line, Bar1Position, Bar1Position, 0, canvas.ActualHeight);
                bar1Line.StrokeThickness = LineWidth;
                bar1Line.Stroke = brush;
                canvas.Children.Add(bar1Line);
                MakeLine(ref bar2Line, Bar2Position, Bar2Position, 0, canvas.ActualHeight);
                bar2Line.StrokeThickness = LineWidth;
                bar2Line.Stroke = brush;
                canvas.Children.Add(bar2Line);
                MakeLine(ref crossbarLine, Bar2Position, Bar1Position, CrossbarPosition, CrossbarPosition);
                crossbarLine.StrokeThickness = LineWidth;
                crossbarLine.Stroke = brush;
                canvas.Children.Add(crossbarLine);
            }
            else
            {
                CrossbarPosition = (float)Math.Min(CrossbarPosition, canvas.ActualWidth - DELTA);
                CrossbarPosition = Math.Max(CrossbarPosition, DELTA);
                Bar1Position = (float)Math.Min(Bar1Position, canvas.ActualHeight - DELTA);
                Bar2Position = Math.Max(Bar2Position, DELTA);
                MakeLine(ref bar1Line, 0, canvas.ActualWidth, Bar1Position, Bar1Position);
                bar1Line.StrokeThickness = LineWidth;
                bar1Line.Stroke = brush;
                canvas.Children.Add(bar1Line);
                MakeLine(ref bar2Line, 0, canvas.ActualWidth, Bar2Position, Bar2Position);
                bar2Line.StrokeThickness = LineWidth;
                bar2Line.Stroke = brush;
                canvas.Children.Add(bar2Line);
                MakeLine(ref crossbarLine, CrossbarPosition, CrossbarPosition, Bar2Position, Bar1Position);
                crossbarLine.StrokeThickness = LineWidth;
                crossbarLine.Stroke = brush;
                canvas.Children.Add(crossbarLine);
            }
            if (isMarching && isTimeCaliper())
            {
                DrawMarchingCalipers(canvas, brush);
            }
            CaliperText(canvas, brush);
        }

        protected void MakeLine(ref Line line, double X1, double X2, double Y1, double Y2)
        {
            line.X1 = X1; line.X2 = X2; line.Y1 = Y1; line.Y2 = Y2;
        }

        protected void CaliperText(Canvas canvas, MBrush brush)
        {
            canvas.Children.Remove(textBlock);
            string text = Measurement();
            double stringWidth = 100;
            double stringHeight = 20;
            float firstBarPosition = Bar2Position > Bar1Position ? Bar1Position : Bar2Position;
            float center = firstBarPosition + (Math.Abs(Bar2Position - Bar1Position) / 2);
            textBlock.FontFamily = new System.Windows.Media.FontFamily("Helvetica");
            textBlock.FontSize = 14;
            textBlock.Text = text;
            textBlock.TextAlignment = System.Windows.TextAlignment.Center;
            textBlock.MinWidth = stringWidth;
            textBlock.MinHeight = stringHeight;
            //textBlock.Background = new SolidColorBrush(ConvertColor(System.Drawing.Color.Gray));
            textBlock.Foreground = brush;
            if (Direction == CaliperDirection.Horizontal)
            {
                Canvas.SetLeft(textBlock, center - stringWidth / 2);
                Canvas.SetTop(textBlock, CrossbarPosition - 20);
            }
            else
            {
                Canvas.SetLeft(textBlock, CrossbarPosition + 5);
                Canvas.SetTop(textBlock, center - stringHeight / 2);
            }
            canvas.Children.Add(textBlock);
        }

        private void DrawMarchingCalipers(Canvas canvas, MBrush brush)
        {
            float difference = Math.Abs(Bar1Position - Bar2Position);
            if (difference < minDistanceForMarch)
            {
                return;
            }
            float[] biggerBars, smallerBars;
            int maxBiggerBars, maxSmallerBars;
            CreateMarchingBars(canvas.ActualWidth, difference, out biggerBars, 
                out smallerBars, out maxBiggerBars, out maxSmallerBars);
            // draw them
            int i = 0;
            while (i < maxBiggerBars)
            {
                Line line = new Line();
                MakeLine(ref line, biggerBars[i], biggerBars[i], 0, canvas.ActualHeight);
                line.StrokeThickness = LineWidth / 2.0;
                line.Stroke = brush;
                //                g.DrawLine(pen, biggerBars[i], 0, biggerBars[i], rect.Size.Height);
                canvas.Children.Add(line);
                i++;
            }
            i = 0;
            while (i < maxSmallerBars)
            {
                Line line = new Line();
                MakeLine(ref line, smallerBars[i], smallerBars[i], 0, canvas.ActualHeight);
                line.StrokeThickness = LineWidth / 2.0;
                canvas.Children.Add(line);
                i++;
            }
        }

        private void CreateMarchingBars(double width, float difference, out float[] biggerBars, 
            out float[] smallerBars, out int maxBiggerBars, out int maxSmallerBars)
        {
            float greaterBar = Math.Max(Bar1Position, Bar2Position);
            float lesserBar = Math.Min(Bar1Position, Bar2Position);
            biggerBars = new float[maxMarchingCalipers];
            smallerBars = new float[maxMarchingCalipers];
            float point = greaterBar + difference;
            int index = 0;
            while (point < width && index < maxMarchingCalipers)
            {
                biggerBars[index] = point;
                point += difference;
                index++;
            }
            maxBiggerBars = index;
            index = 0;
            point = lesserBar - difference;
            while (point > 0 && index < maxMarchingCalipers)
            {
                smallerBars[index] = point;
                point -= difference;
                index++;
            }
            maxSmallerBars = index;
        }

        protected System.Windows.Media.Color ConvertColor(System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public override void Draw(Graphics g, RectangleF rect)
        {
            DBrush brush = new SolidBrush(CaliperColor);
            var pen = new System.Drawing.Pen(brush, LineWidth);
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
                DrawMarchingCalipers(g, brush, rect);
            }
            CaliperText(g, brush);
            pen.Dispose();
            brush.Dispose();
        }

        protected void CaliperText(Graphics g, DBrush brush)
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

        private void DrawMarchingCalipers(Graphics g, DBrush brush, RectangleF rect)
        {
            // note that pen width < 1 (e.g. 0) will always just draw as width of 1
            var pen = new System.Drawing.Pen(brush, LineWidth - 1.0f);
            float difference = Math.Abs(Bar1Position - Bar2Position);
            if (difference < minDistanceForMarch)
            {
                return;
            }
            float[] biggerBars, smallerBars;
            int maxBiggerBars, maxSmallerBars;
            CreateMarchingBars(rect.Size.Width, difference, out biggerBars, out smallerBars, 
                out maxBiggerBars, out maxSmallerBars);
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

        public void Setup(Preferences preferences)
        {
            LineWidth = preferences.LineWidth;
            UnselectedColor = preferences.CaliperColor;
            SelectedColor = preferences.HighlightColor;
            CaliperColor = UnselectedColor;
            rounding = preferences.RoundingParameter();
            SetInitialPosition();
        }
    }
}
