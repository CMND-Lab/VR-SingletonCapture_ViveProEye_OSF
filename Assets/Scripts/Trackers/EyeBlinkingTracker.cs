using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

namespace VRSingletonSearch
{
    public class EyeBlinkingTracker : Tracker
    {
        private EyeTracking eyeTracking;
        public override string MeasurementDescriptor => "blinking";
        public override IEnumerable<string> CustomHeader => new string[]
                    {
                        "left_eye",
                        "right_eye"
                    };
        
        private void Awake()
        {
            eyeTracking = GetComponentInParent<EyeTracking>();
        }
        
        protected override UXFDataRow GetCurrentValues()
        {
            int isLeftEyeBlinking = eyeTracking.data_local.IsLeftEyeBlinking ?  1: 0;
            int isRightEyeBlinking = eyeTracking.data_local.IsRightEyeBlinking ? 1 : 0;
            
            var values = new UXFDataRow()
            {
                ("left_eye", isLeftEyeBlinking.ToString()),
                ("right_eye", isRightEyeBlinking.ToString())
            };

            return values;
        }
    }
}

