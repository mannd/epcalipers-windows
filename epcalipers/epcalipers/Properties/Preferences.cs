using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epcalipers.Properties
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
        private bool showTransparentWindowAtStart;
        private bool useAlternativeTransparency;
        private bool windowOnTopWhenTransparent;
        private float alternativeTransparencyAlpha;
        private string rounding;

        private const int MAX_LINEWIDTH = 3;
        private const int MAX_NUMBER_OF_INTERVALS = 10;
        private const float MIN_ALPHA = 0.2F;
        private const float MAX_ALPHA = 0.8F;

        public enum Rounding
        {
            ToInt,
            ToFourPlaces,
            ToTenths,
            ToHundredths,
            None
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
            showTransparentWindowAtStart = (bool)Settings.Default["ShowTransparentWindowAtStart"];
            useAlternativeTransparency = (bool)Settings.Default["UseAlternativeTransparency"];
            windowOnTopWhenTransparent = (bool)Settings.Default["WindowOnTopWhenTransparent"];
            alternativeTransparencyAlpha = (float)Settings.Default["AlternativeTransparencyAlpha"];
            rounding = (string)Settings.Default["RoundTo"];
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
            DisplayName("Transparent window at start"),
            Description("Show transparent window at startup"),
            Category("Transparency")]
        public bool ShowTransparentWindowAtStart
        {
            get { return showTransparentWindowAtStart; }
            set { showTransparentWindowAtStart = value; }
        }

        [Browsable(true),
            ReadOnly(false),
            DisplayName("Use altenative transparency"),
            Description("Full transparency doesn't work for some versions of Windows.  If default transparency doesn't work" +
            ", select this option."),
            Category("Transparency")]
        public bool UseAlternativeTransparency
        {
            get { return useAlternativeTransparency; }
            set { useAlternativeTransparency = value; }
        }

        [Browsable(true),
            ReadOnly(false),
            DisplayName("Transparent window on top"),
            Description("Transparent window always floats above other windows"),
            Category("Transparency")]
        public bool WindowOnTopWhenTransparent
        {
            get { return windowOnTopWhenTransparent; }
            set { windowOnTopWhenTransparent = value; }
        }
        
        [Browsable(true),
            ReadOnly(false),
            DisplayName("Transparency alpha value"),
            Description("Alternative transparency method alpha value (legal values 0.2-0.8)"),
            Category("Transparency")]
        public float AlternativeTransparencyAlpha
        {
            get { return alternativeTransparencyAlpha; }
            set {
                if (value < MIN_ALPHA)
                {
                    value = MIN_ALPHA;
                }
                if (value > MAX_ALPHA)
                {
                    value = MAX_ALPHA;
                }
                alternativeTransparencyAlpha = value; }
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
            Settings.Default["ShowTransparentWindowAtStart"] = showTransparentWindowAtStart;
            Settings.Default["UseAlternativeTransparency"] = useAlternativeTransparency;
            Settings.Default["WindowOnTopWhenTransparent"] = windowOnTopWhenTransparent;
            Settings.Default["AlternativeTransparencyAlpha"] = alternativeTransparencyAlpha;
            Settings.Default["RoundTo"] = rounding;
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
}
