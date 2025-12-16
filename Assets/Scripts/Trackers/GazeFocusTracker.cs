using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

namespace VRSingletonSearch
{
    public class GazeFocusTracker : Tracker
    {

        private GazeFocusDetector detector;
        public override string MeasurementDescriptor => "gazetracker";
        public override IEnumerable<string> CustomHeader => new string[]
        {
           "focus"
        };

        private void Awake()
        {
            detector = GetComponent<GazeFocusDetector>();
        }
        
        protected override UXFDataRow GetCurrentValues()
        {
            int f = detector.focus ? 1 : 0;

            var values = new UXFDataRow()
            {
                ("focus", f.ToString())
            };
                
            return values;
        }
    }
}

