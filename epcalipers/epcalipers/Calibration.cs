using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epcalipers
{
    public class Calibration
    {
        private String rawUnits;

        public CaliperDirection direction { set; get; }
        public String units
        {
            set { rawUnits = value; }
            get {
                if (calibrated) {
                    if (displayRate) {
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
        public String calibrationString { set; get; }
        public Boolean canDisplayRate
        {
            get
            {
                if (direction == CaliperDirection.Vertical  || !calibrated)
                {
                    return false;
                }
                return unitsAreMsecs || unitsAreSeconds;
            }
        }
        // TODO refactor next 2 variables:
        // enum Units { Sec, Msec, None }; 
        public Boolean unitsAreSeconds { get { return _unitsAreSeconds(); } }
        public Boolean unitsAreMsecs { get { return _unitsAreMsecs(); } }
        public Boolean displayRate { get; set; }
        public float multiplier {
            get
            {
                if (calibrated)
                {
                    return currentCalFactor;
                } else
                {
                    return 1.0f;
                }
            }
        }
        public float originalZoom { get; set; }
        public float currentZoom { get; set; }
        public float originalCalFactor { get; set; }
        public float currentCalFactor
        {
            get
            {
                return originalZoom * originalCalFactor / currentZoom;
            }
        }
        public Boolean calibrated { get; set; }

        // constructor
        public Calibration()
        {
            initWithDirection(CaliperDirection.Horizontal);
        }

        public void initWithDirection(CaliperDirection direction)
        {
            reset();
            this.direction = direction;
        }

        private void reset()
        {
            units = "points";
            displayRate = false;
            originalZoom = 1.0f;
            currentZoom = 1.0f;
            calibrated = false;
        }

        private Boolean _unitsAreSeconds()
        {
            if (rawUnits.Length < 1)
                return false;
            String units = rawUnits.ToUpper();
            return units.Equals("S") || units.Equals("SEC") || units.Equals("SECOND")
                || units.Equals("SECS") || units.Equals("SECONDS");
        }

        private Boolean _unitsAreMsecs()
        {
            if (rawUnits.Length < 1)
                return false;
            String units = rawUnits.ToUpper();
            return units.Contains("MSEC") || units.Equals("MS") || units.Contains("MILLIS");
        }
    }
}
