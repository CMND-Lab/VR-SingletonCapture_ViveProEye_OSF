using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

namespace VRSingletonSearch
{
    public class ValidConvergenceDistanceTracker : Tracker
    {
        private EyeTracking eyeTracking;
        
        public override string MeasurementDescriptor => "convergence";
            
        public override IEnumerable<string> CustomHeader => new string[]
        {
            "validity"
        };
        private void Awake()
        {
            eyeTracking = GetComponentInParent<EyeTracking>();
        }

        
        
        

        protected override UXFDataRow GetCurrentValues()
        {
            int isValid = eyeTracking.data_world.GazeRay.IsValid ?  1 : 0;
            
            var values = new UXFDataRow()
            {
                ("validity", isValid.ToString())
            };

            return values;
        }
    }
}

