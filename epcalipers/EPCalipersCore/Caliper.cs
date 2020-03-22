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
using TextPosition = EPCalipersCore.Properties.Preferences.TextPosition;
using System.Windows.Forms;

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
        protected double horizontalSizeAdjustment = 0.8;

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
                MakeLine(ref bar1Line, Bar1Position, Bar1Position, 0, canvas.ActualHeight + 30);
                bar1Line.StrokeThickness = LineWidth;
                bar1Line.Stroke = brush;
                canvas.Children.Add(bar1Line);
                // Adding 30 ensures bars reach down to bottom of window, beyond canvas!
                MakeLine(ref bar2Line, Bar2Position, Bar2Position, 0, canvas.ActualHeight + 30);
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
            };
            CaliperText(canvas, brush, CaliperTextPosition, true);
            DrawChosenComponent(canvas);
        }

        protected void MakeLine(ref Line line, double X1, double X2, double Y1, double Y2)
        {
            line.X1 = X1; line.X2 = X2; line.Y1 = Y1; line.Y2 = Y2;
        }

        protected void DrawChosenComponent(Canvas canvas)
        {
            if (ChosenComponent == CaliperComponent.NoComponent) return;
            MBrush brush = new SolidColorBrush(ConvertColor(GetChosenComponentColor()));
            Line chosenComponentLine = new Line();
            switch (ChosenComponent)
            {
                case CaliperComponent.LeftBar:
                    MakeLine(ref chosenComponentLine, Bar1Position, Bar1Position, 0, canvas.ActualHeight + 30);
                    break;
                case CaliperComponent.RightBar:
                    MakeLine(ref chosenComponentLine, Bar2Position, Bar2Position, canvas.ActualHeight + 30, 0);
                    break;
                //case .lowerBar:
                //    +context.move(to: CGPoint(x: 0, y: bar1Position))
                //    + context.addLine(to: CGPoint(x: rect.size.width, y: bar1Position))
                //    +        case .rightBar:
                //    +context.move(to: CGPoint(x: bar2Position, y: rect.size.height))
                //    + context.addLine(to: CGPoint(x: bar2Position, y: 0))
                //    +        case .upperBar:
                //    +context.move(to: CGPoint(x: 0, y: bar2Position))
                //    + context.addLine(to: CGPoint(x: rect.size.width, y: bar2Position))
                //    +        case .crossBar:
                //    +            if (direction == .horizontal)
                //    {
                //        +context.move(to: CGPoint(x: bar2Position, y: crossBarPosition))
                //        + context.addLine(to: CGPoint(x: bar1Position, y: crossBarPosition))
                //        +            }
                //    +            else
                //    {
                //        +context.move(to: CGPoint(x: crossBarPosition, y: bar2Position))
                //        + context.addLine(to: CGPoint(x: crossBarPosition, y: bar1Position))
                //        +            }

                default:
                    break;
            }
            chosenComponentLine.StrokeThickness = LineWidth;
            chosenComponentLine.Stroke = brush;
            canvas.Children.Add(chosenComponentLine);
        }

        private System.Drawing.Color GetChosenComponentColor()
        {
           System.Drawing.Color chosenComponentColor;
           if (IsSelected)
            {
                chosenComponentColor = UnselectedColor;
            }
           else
            {
                chosenComponentColor = SelectedColor;
            }
            return chosenComponentColor;
        }

        protected void CaliperText(Canvas canvas, MBrush brush, TextPosition textPosition,
            bool optimizeTextPosition)
        {
            canvas.Children.Remove(textBlock);
            string text = Measurement();
            textBlock.FontFamily = new System.Windows.Media.FontFamily("Helvetica");
            textBlock.FontSize = defaultCanvasFontSize;
            textBlock.Text = text;
            textBlock.TextAlignment = System.Windows.TextAlignment.Center;
            //textBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            Font font = new Font("Helvetica", defaultCanvasFontSize);
            Size size = TextRenderer.MeasureText(text, font);
            // The textblocks based on TextRenderer.MeasureTest are too wide, so adjust.
            size.Width = (int)Math.Round(horizontalSizeAdjustment * size.Width);
            textBlock.Width = size.Width;
            textBlock.Height = size.Height;
            // Adding padding to a textBlock centers its content.
            textBlock.Padding = new System.Windows.Thickness(3);
            // Shade the textBlock for debugging purposes if necessary.
            //textBlock.Background = new SolidColorBrush(ConvertColor(System.Drawing.Color.Gray));
            textBlock.Foreground = brush;

            RectangleF textRect = GetCaliperTextPosition(textPosition, Math.Min(Bar1Position, Bar2Position),
                            Math.Max(Bar1Position, Bar2Position), CrossbarPosition, size,
                            new RectangleF(0, 0, (float)canvas.ActualWidth, (float)canvas.ActualHeight),
                            optimizeTextPosition);

            Canvas.SetLeft(textBlock, textRect.X);
            Canvas.SetTop(textBlock, textRect.Y);
            canvas.Children.Add(textBlock);
        }

        private void DrawMarchingCalipers(Canvas canvas, MBrush brush)
        {
            float difference = Math.Abs(Bar1Position - Bar2Position);
            if (difference < minDistanceForMarch)
            {
                return;
            }

            CreateMarchingBars(canvas.ActualWidth, difference, out float[] biggerBars, 
                out float[] smallerBars, out int maxBiggerBars, out int maxSmallerBars);
            // draw them
            int i = 0;
            while (i < maxBiggerBars)
            {
                Line line = new Line();
                // Adding 30 makes the bottom of each line extend to bottom of window, beyond canvas
                MakeLine(ref line, biggerBars[i], biggerBars[i], 0, canvas.ActualHeight + 30);
                line.StrokeThickness = LineWidth / 2.0;
                line.Stroke = brush;
                canvas.Children.Add(line);
                i++;
            }
            i = 0;
            while (i < maxSmallerBars)
            {
                Line line = new Line();
                MakeLine(ref line, smallerBars[i], smallerBars[i], 0, canvas.ActualHeight + 30);
                line.StrokeThickness = LineWidth / 2.0;
                line.Stroke = brush;
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
            //CaliperText(g, brush);
            CaliperText(g, brush, rect, CaliperTextPosition, true);
            pen.Dispose();
            brush.Dispose();
        }

        // This is the WinForm version
        protected void CaliperText(Graphics g, DBrush brush)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            string text = Measurement();
            SizeF sizeOfString = g.MeasureString(text, TextFont);
            Console.WriteLine($"size = {sizeOfString.Height} {sizeOfString.Width}");
            float stringWidth = sizeOfString.Width;
            float stringHeight = sizeOfString.Height;
            float firstBarPosition = Bar2Position > Bar1Position ? Bar1Position : Bar2Position;
            float center = firstBarPosition + (Math.Abs(Bar2Position - Bar1Position) / 2);
            if (Direction == CaliperDirection.Horizontal)
            {
                g.DrawString(text, TextFont, brush, center - stringWidth / 2, CrossbarPosition - 20);
            }
            else
            {
                g.DrawString(text, TextFont, brush, CrossbarPosition + 5, center - stringHeight / 2);
            }
        }

        protected void CaliperText(Graphics g, DBrush brush, RectangleF rect, TextPosition textPosition, bool optimizeTextPosition)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            string text = Measurement();
            SizeF size = g.MeasureString(text, TextFont);
            float stringWidth = size.Width;
            float stringHeight = size.Height;
            var textRect = GetCaliperTextPosition(textPosition, Math.Min(Bar1Position, Bar2Position),
                Math.Max(Bar1Position, Bar2Position), CrossbarPosition, size, rect, optimizeTextPosition);
            g.DrawString(text, TextFont, brush, textRect);
        }

        protected RectangleF GetCaliperTextPosition(TextPosition textPosition, float left, float right, float center,
                        SizeF size, RectangleF rect, bool optimizeTextPosition)
        {
            // Assumes X is center of text block and y is text baseline.
            var textOrigin = new PointF();
            var origin = new PointF();
            var textHeight = size.Height;
            var textWidth = size.Width;
            const float xOffset = 5;
            const float yOffset = 5;
            var optimizedPosition = OptimizedTextPosition(left, right, center, rect.Width, rect.Height, textPosition, 
                                                            textWidth, textHeight, optimizeTextPosition);
            if (Direction == CaliperDirection.Horizontal)
            {
                origin.Y = center;
                switch (optimizedPosition)
                {
                    case TextPosition.CenterAbove:
                        origin.X = left + (right - left) / 2;
                        textOrigin.X = origin.X;
                        textOrigin.Y = origin.Y - yOffset;
                        break;
                    case TextPosition.CenterBelow:
                        origin.X = left + (right - left) / 2;
                        textOrigin.X = origin.X;
                        textOrigin.Y = origin.Y + yOffset + textHeight;
                        break;
                    case TextPosition.Left:
                        origin.X = left;
                        textOrigin.X = origin.X - xOffset - textWidth / 2;
                        textOrigin.Y = origin.Y - yOffset;
                        break;
                    case TextPosition.Right:
                        origin.X = right;
                        textOrigin.X = origin.X + xOffset + textWidth / 2;
                        textOrigin.Y = origin.Y - yOffset;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                textOrigin.Y = textHeight / 2 + left + (right - left) / 2;
                switch (optimizedPosition)
                {
                    case TextPosition.Left:
                        textOrigin.X = center - xOffset - textWidth / 2;
                        break;
                    case TextPosition.Right:
                        textOrigin.X = center + xOffset + textWidth / 2;
                        break;
                    case TextPosition.Top:
                        textOrigin.Y = left - yOffset;
                        textOrigin.X = center;
                        break;
                    case TextPosition.Bottom:
                        textOrigin.Y = right + yOffset + textHeight;
                        textOrigin.X = center;
                        break;
                    default:
                        break;
                }
            }
            return new RectangleF(textOrigin.X - textWidth / 2, textOrigin.Y - textHeight, textWidth, textHeight);
        }

        private TextPosition OptimizedTextPosition(float left, double right, double center,
                                                    double windowWidth, double windowHeight,
                                                    TextPosition textPosition,
                                                    double textWidth, double textHeight,
                                                    bool optimizeTextPosition)
        {
            if (!AutoPositionText || !optimizeTextPosition)
            {
                return textPosition;
            }
            const double offset = 4;
            var optimizedPosition = textPosition;
            if (Direction == CaliperDirection.Horizontal)
            {
                switch (optimizedPosition)
                {
                    case TextPosition.CenterAbove:
                    case TextPosition.CenterBelow:
                        // Avoid squeezing label.
                        if (textWidth + offset > right - left)
                        {
                            optimizedPosition = (textWidth + right + offset > windowWidth)
                                ? TextPosition.Left : TextPosition.Right;
                        }
                        break;
                    case TextPosition.Left:
                        if (textWidth + offset > left)
                        {
                            optimizedPosition = (textWidth + right + offset > windowWidth)
                                ? TextPosition.CenterAbove : TextPosition.Right;
                        }
                        break;
                    case TextPosition.Right:
                        if (textWidth + right + offset > windowWidth)
                        {
                            optimizedPosition = (textWidth + offset > left)
                                ? TextPosition.CenterAbove : TextPosition.Left;
                        }
                        break;
                    default:
                        optimizedPosition = textPosition;
                        break;
                }
            }
            else if (Direction == CaliperDirection.Vertical)
            {
                // watch for squeeze
                if ((optimizedPosition == TextPosition.Left || optimizedPosition == TextPosition.Right)
                    && textHeight + offset > right - left)
                {
                    optimizedPosition = (left - textHeight - offset < 0)
                        ? TextPosition.Bottom : TextPosition.Top;
                }
                else
                {
                    switch (optimizedPosition)
                    {
                        case TextPosition.Left:
                            if (textWidth + offset > center)
                            {
                                optimizedPosition = TextPosition.Right;
                            }
                            break;
                        case TextPosition.Right:
                            if (textWidth + center + offset > windowWidth)
                            {
                                optimizedPosition = TextPosition.Left;
                            }
                            break;
                        case TextPosition.Top:
                            if (left - textHeight - offset < 0)
                            {
                                optimizedPosition = (right + textHeight - offset < 0)
                                    ? TextPosition.Right : TextPosition.Bottom;
                            }
                            break;
                        case TextPosition.Bottom:
                            if (right + textHeight + offset > windowHeight)
                            {
                                optimizedPosition = (left - textHeight - offset < 0)
                                    ? TextPosition.Right : TextPosition.Top;
                            }
                            break;
                        default:
                            optimizedPosition = textPosition;
                            break;
                    }
                }
            }
            return optimizedPosition;
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

            CreateMarchingBars(rect.Size.Width, difference, out float[] biggerBars, out float[] smallerBars, 
                out int maxBiggerBars, out int maxSmallerBars);
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
            Rounding = preferences.RoundingParameter();
            AutoPositionText = preferences.AutoPositionText;
            if (Direction == CaliperDirection.Horizontal)
            {
                CaliperTextPosition = preferences.TimeCaliperTextPositionParameter();
            }
            else
            {
                CaliperTextPosition = preferences.AmplitudeCaliperTextPositionParameter();
            }
            SetInitialPosition();
        }
    }
}
