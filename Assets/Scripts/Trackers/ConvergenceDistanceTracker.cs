using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

namespace VRSingletonSearch
{
    public class ConvergenceDistanceTracker : Tracker
    {
        private EyeTracking eyeTracking;
        public override string MeasurementDescriptor => "convergence";
        public override IEnumerable<string> CustomHeader => new string[]
            {
                "distance",
                "validity"
            };
        private void Awake()
        {
            eyeTracking = GetComponentInParent<EyeTracking>();
        }

        protected override UXFDataRow GetCurrentValues()
         {
             float convergenceDistance = eyeTracking.data_world.ConvergenceDistance;
             int isValid = eyeTracking.data_world.ConvergenceDistanceIsValid ? 1 : 0;
             
             var values = new UXFDataRow()
             {
                 ("distance", convergenceDistance),
                 ("validity", isValid.ToString())
             };
        
             return values;
        }
    }
}


