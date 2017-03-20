using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epcalipers
{
    public class Calibration
    {
        private string rawUnits;

        public CaliperDirection Direction { set; get; }
        public string Units
        {
            set { rawUnits = value; }
            get {
                if (Calibrated) {
                    if (DisplayRate) {
                        return "bpm";
                    }
                    else {
                        return rawUnits;
                    }
                }
                else {
                    return "points";
                }
            }
        }
        public string CalibrationString { set; get; }
        public bool CanDisplayRate
        {
            get
            {
                if (Direction == CaliperDirection.Vertical  || !Calibrated)
                {
                    return false;
                }
                return UnitsAreMsecs || UnitsAreSeconds;
            }
        }
        // TODO possibly refactor next 2 variables:
        //public enum UnitType { Sec, Msec, Bpm, None };
        public bool UnitsAreSeconds { get { return _unitsAreSeconds(); } }
        public bool UnitsAreMsecs { get { return _unitsAreMsecs(); } }
        public bool UnitsAreMM { get { return _unitsAreMM(); } }
        public bool DisplayRate { get; set; }
        public double Multiplier {
            get
            {
                if (Calibrated)
                {
                    return CurrentCalFactor;
                } else
                {
                    return 1.0f;
                }
            }
        }
        public double OriginalZoom { get; set; }
        public double CurrentZoom { get; set; }
        public double OriginalCalFactor { get; set; }
        public double CurrentCalFactor
        {
            get
            {
                return OriginalZoom * OriginalCalFactor / CurrentZoom;
            }
        }
        public bool Calibrated { get; set; }

        // constructor
        public Calibration()
        {
            InitWithDirection(CaliperDirection.Horizontal);
        }

        public Calibration(CaliperDirection direction)
        {
            InitWithDirection(direction);
        }

        public void InitWithDirection(CaliperDirection direction)
        {
            Reset();
            this.Direction = direction;
        }

        public void Reset()
        {
            Units = "points";
            DisplayRate = false;
            OriginalZoom = 1.0f;
            CurrentZoom = 1.0f;
            Calibrated = false;
        }

        private bool _unitsAreSeconds()
        {
            if (rawUnits.Length < 1)
                return false;
            string units = rawUnits.ToUpper();
            return units.Equals("S") || units.Equals("SEC") || units.Equals("SECOND")
                || units.Equals("SECS") || units.Equals("SECONDS");
        }

        private bool _unitsAreMsecs()
        {
            if (rawUnits.Length < 1)
                return false;
            string units = rawUnits.ToUpper();
            return units.Contains("MSEC") || units.Equals("MS") || units.Contains("MILLIS");
        }

        private bool _unitsAreMM()
        {
            if (rawUnits.Length < 1 || Direction != CaliperDirection.Vertical)
            {
                return false;
            }
            string upcasedUnits = rawUnits.ToUpper();
            return upcasedUnits.Equals("MM") || upcasedUnits.Contains("MILLIM");
        }
    }
}
