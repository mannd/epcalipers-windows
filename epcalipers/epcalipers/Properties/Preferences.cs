﻿using System;
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
        private bool roundMsecRate;
        private int numberOfIntervalsMeanRR;
        private int numberOfIntervalsQtc;

        private const int MAX_LINEWIDTH = 3;
        private const int MAX_NUMBER_OF_INTERVALS = 10;

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
            roundMsecRate = (bool)Settings.Default["RoundMsecRate"];
            numberOfIntervalsMeanRR = (int)Settings.Default["NumberOfIntervalsMeanRR"];
            numberOfIntervalsQtc = (int)Settings.Default["NumberOfIntervalsQtc"];
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
            DisplayName("Selected caliper color"),
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
            Description("Round msec and rates to nearest integer"),
            DisplayName("Round msec and rates"),
            Category("Measurements")]
        public bool RoundMsecRate
        {
            get { return roundMsecRate; }
            set { roundMsecRate = value; }
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



        public void Save()
        {
            Settings.Default["CaliperColor"] = caliperColor;
            Settings.Default["HighlightColor"] = highlightColor;
            Settings.Default["LineWidth"] = lineWidth;
            Settings.Default["HorizontalCalibration"] = horizontalCalibration;
            Settings.Default["VerticalCalibration"] = verticalCalibration;
            Settings.Default["RoundMsecRate"] = roundMsecRate;
            Settings.Default["NumberOfIntervalsMeanRR"] = numberOfIntervalsMeanRR;
            Settings.Default["NumberOfIntervalsQtc"] = numberOfIntervalsQtc;
            Settings.Default.Save();
        }

    }
}
