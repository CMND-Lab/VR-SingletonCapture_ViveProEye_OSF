using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
using UXF;

public class CustomGazeTrack : MonoBehaviour
{
    // Start is called before the first frame update
    private EyeTracking eyeTracking;
    private Collider gazeCollider;
    public GameObject gazePoint;

    [SerializeField] float lerpRate = 0.15f;

    void Awake()
    {
        eyeTracking = GetComponent<EyeTracking>();
        gazeCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (eyeTracking != null && eyeTracking.data_world != null) {
            Vector3 rayOrigin = eyeTracking.data_world.GazeRay.Origin;
            Vector3 rayDirection = eyeTracking.data_world.GazeRay.Direction;

            Ray ray = new Ray(rayOrigin, rayDirection);

            if (gazeCollider.Raycast(ray, out RaycastHit hitData, 1f)) {
                Vector3 gazePosition = hitData.point;
                Vector3 pointPosition = gazePoint.transform.position;

                Vector3 newPosition = Vector3.Lerp(pointPosition, gazePosition, lerpRate);

                gazePoint.transform.position = newPosition;
            }
        }
    }
}
