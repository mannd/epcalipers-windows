using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersCore.Properties
{
    public class Preferences
    {
        private Color caliperColor;
        private Color highlightColor;
        private int lineWidth;
        private string horizontalCalibration;
        private string verticalCalibration;
        private int numberOfIntervalsMeanRR;
        private int numberOfIntervalsQtc;
        private string defaultQtcFormula;
        private string rounding;
        private bool autoPositionText;
        private string timeCaliperTextPosition;
        private string amplitudeCaliperTextPosition;

        private const int MAX_LINEWIDTH = 3;
        private const int MAX_NUMBER_OF_INTERVALS = 10;

        public enum Rounding
        {
            ToInt,
            ToFourPlaces,
            ToTenths,
            ToHundredths,
            None
        }

        public enum TextPosition
        {
            CenterAbove,
            CenterBelow,
            Left,
            Right,
            Top,
            Bottom
        }

        public Preferences()
        {
            Load();
        }

        public void Load()
        {
            caliperColor = (Color)Settings.Default["CaliperColor"];
            highlightColor = (Color)Settings.Default["HighlightColor"];
            lineWidth = (int)Settings.Default["LineWidth"];
            horizontalCalibration = (string)Settings.Default["HorizontalCalibration"];
            verticalCalibration = (string)Settings.Default["VerticalCalibration"];
            numberOfIntervalsMeanRR = (int)Settings.Default["NumberOfIntervalsMeanRR"];
            numberOfIntervalsQtc = (int)Settings.Default["NumberOfIntervalsQtc"];
            defaultQtcFormula = (string)Settings.Default["DefaultQtcFormula"];
            rounding = (string)Settings.Default["RoundTo"];
            autoPositionText = (bool)Settings.Default["AutoPositionText"];
            timeCaliperTextPosition = (string)Settings.Default["TimeCaliperTextPosition"];
            amplitudeCaliperTextPosition = (string)Settings.Default["AmplitudeCaliperTextPosition"];
        }

        public QtcFormula ActiveQtcFormula()
        {
            switch (defaultQtcFormula)
            {
                case "Bazett":
                    return QtcFormula.qtcBzt;
                case "Hodges":
                    return QtcFormula.qtcHdg;
                case "Framingham":
                    return QtcFormula.qtcFrm;
                case "Fridericia":
                    return QtcFormula.qtcFrd;
                case "All":
                    return QtcFormula.qtcAll;
                default:
                    return QtcFormula.qtcBzt;
            }
        }

        public Rounding RoundingParameter()
        {
            switch (rounding)
            {
                case "To Integer":
                    return Rounding.ToInt;
                case "To Four Places":
                    return Rounding.ToFourPlaces;
                case "To Tenths":
                    return Rounding.ToTenths;
                case "To Hundredths":
                    return Rounding.ToHundredths;
                case "No Rounding":
                    return Rounding.None;
                default:
                    return Rounding.ToInt;
            }
        }

        public TextPosition TimeCaliperTextPositionParameter()
        {
            return GetTextPosition(timeCaliperTextPosition);
        }

        public TextPosition AmplitudeCaliperTextPositionParameter()
        {
            return GetTextPosition(amplitudeCaliperTextPosition);
        }

        private TextPosition GetTextPosition(string position)
        {
            switch (position)
            {
                case "Center Above":
                    return TextPosition.CenterAbove;
                case "Center Below":
                    return TextPosition.CenterBelow;
                case "Right":
                    return TextPosition.Right;
                case "Left":
                    return TextPosition.Left;
                case "Top":
                    return TextPosition.Top;
                case "Bottom":
                    return TextPosition.Bottom;
                default:
                    return TextPosition.CenterAbove;
            }

        }

        [Browsable(true),
            ReadOnly(false),
            Description("Position caliper text automatically"),
            DisplayName("Auto-position text"),
            Category("Calipers")]
        public bool AutoPositionText
        {
            get { return autoPositionText; }
            set { autoPositionText = value; }
        }

        [Browsable(true),
            ReadOnly(false),
            Description("Unselected caliper color"),
            DisplayName("Caliper color"),
            Category("Calipers")]
        public Color CaliperColor
        {
            get { return caliperColor; }
            set { caliperColor = value; }
        }

        [Browsable(true),
            ReadOnly(false),
            Description("Selected caliper color"),
            DisplayName("Caliper selected color"),
            Category("Calipers")]
        public Color HighlightColor
        {
            get { return highlightColor; }
            set { highlightColor = value; }
        }

        [Browsable(true),
            ReadOnly(false),
            Description("Caliper line width (between 1 and 3)"),
            DisplayName("Line width"),
            Category("Calipers")]
        public int LineWidth
        {
            get { return lineWidth; }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                if (value > MAX_LINEWIDTH)
                {
                    value = MAX_LINEWIDTH;
                }
                lineWidth = value;
            }
        }

        [Browsable(true),
            ReadOnly(false),
            Description("Default calibration interval for time calipers"),
            DisplayName("Default time calibration"),
            Category("Calibration")]
        public string HorizontalCalibration
        {
            get { return horizontalCalibration; }
            set { horizontalCalibration = value; }
        }

        [Browsable(true),
            ReadOnly(false),
            Description("Default calibration interval for amplitude calipers"),
            DisplayName("Default amplitude calibration"),
            Category("Calibration")]
        public string VerticalCalibration
        {
            get { return verticalCalibration; }
            set { verticalCalibration = value; }
        }

         [Browsable(true),
            ReadOnly(false),
            TypeConverter(typeof(RoundingConverter)),
            Description("Rounding to integer or number of places"),
            DisplayName("Round msec and rates"),
            Category("Measurements")]
        public string RoundTo
        {
            get { return rounding; }
            set { rounding = value; }
        }

         [Browsable(true),
            ReadOnly(false),
            TypeConverter(typeof(TimeCaliperTextPositionConverter)),
            Description("Time caliper text position"),
            DisplayName("Time text position"),
            Category("Calipers")]
        public string TimeCaliperTextPosition
        {
            get { return timeCaliperTextPosition; }
            set { timeCaliperTextPosition = value; }
        }

         [Browsable(true),
            ReadOnly(false),
            TypeConverter(typeof(AmplitudeCaliperTextPositionConverter)),
            Description("Amplitude caliper text position"),
            DisplayName("Amplitude text position"),
            Category("Calipers")]
        public string AmplitudeCaliperTextPosition
        {
            get { return amplitudeCaliperTextPosition; }
            set { amplitudeCaliperTextPosition = value; }
        }

        [Browsable(true),
            ReadOnly(false),
            Description("Number of intervals for mean RR measurement (between 2 and 10)"),
            DisplayName("Number of intervals (mean RR)"),
            Category("Measurements")]
        public int NumberOfIntervalsMeanRR
        {
            get { return numberOfIntervalsMeanRR; }
            set
            {
                if (value < 2)
                {
                    value = 2;
                }
                if (value > MAX_NUMBER_OF_INTERVALS)
                {
                    value = MAX_NUMBER_OF_INTERVALS;
                }
                numberOfIntervalsMeanRR = value;
            }
        }

        [Browsable(true),
            ReadOnly(false),
            Description("Number of intervals for RR measurement for QTc calculation (between 1 and 10)"),
            DisplayName("Number of intervals (QTc)"),
            Category("Measurements")]
        public int NumberOfIntervalsQtc
        {
            get { return numberOfIntervalsQtc; }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                if (value > MAX_NUMBER_OF_INTERVALS)
                {
                    value = MAX_NUMBER_OF_INTERVALS;
                }
                numberOfIntervalsQtc = value;
            }
        }

        [Browsable(true),
            ReadOnly(false),
            TypeConverter(typeof(QtcFormulaConverter)),
            DisplayName("QTc formula"),
            Description("QTc formula used for calculating QTc"),
            Category("Measurements")]
        public string DefaultQtcFormula
        {
            get { return defaultQtcFormula; }
            set { defaultQtcFormula = value; }
        }

         public void Save()
        {
            Settings.Default["CaliperColor"] = caliperColor;
            Settings.Default["HighlightColor"] = highlightColor;
            Settings.Default["LineWidth"] = lineWidth;
            Settings.Default["HorizontalCalibration"] = horizontalCalibration;
            Settings.Default["VerticalCalibration"] = verticalCalibration;
            Settings.Default["NumberOfIntervalsMeanRR"] = numberOfIntervalsMeanRR;
            Settings.Default["NumberOfIntervalsQtc"] = numberOfIntervalsQtc;
            Settings.Default["DefaultQtcFormula"] = defaultQtcFormula;
            Settings.Default["RoundTo"] = rounding;
            Settings.Default["AutoPositionText"] = autoPositionText;
            Settings.Default["TimeCaliperTextPosition"] = timeCaliperTextPosition;
            Settings.Default["AmplitudeCaliperTextPosition"] = amplitudeCaliperTextPosition;
            Settings.Default.Save();
        }

    }

    public class QtcFormulaConverter: StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "Bazett",
                                                                "Hodges",
                                                                "Framingham",
                                                                "Fridericia",
                                                                "All"});

        }
    }

    public class RoundingConverter: StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "To Integer",
                                                                "To Four Places",
                                                                "To Tenths",
                                                                "To Hundredths",
                                                                "No Rounding"});

        }
    }

    public class TimeCaliperTextPositionConverter: StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "Center Above",
                                                                "Center Below",
                                                                "Right",
                                                                "Left"});

        }
    }

    public class AmplitudeCaliperTextPositionConverter: StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "Top",
                                                                "Bottom",
                                                                "Right",
                                                                "Left"});

        }
    }
}
