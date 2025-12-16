using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.XR;

public class EyeTracking : MonoBehaviour
{
    public TobiiXR_EyeTrackingData data_world;

    public TobiiXR_EyeTrackingData data_local;
    
    private void Update()
    {
        data_world = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.World);

        data_local = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.Local);
    }
}
