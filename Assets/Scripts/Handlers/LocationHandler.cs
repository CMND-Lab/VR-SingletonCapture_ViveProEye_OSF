using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using System.Linq;
using UXF;

namespace VRSingletonSearch
{
    public class LocationHandler : MonoBehaviour
    {
        public Session session;
        public bool trackHead = false;
        public GameObject display;
        private bool adjusted = false;
        [SerializeField] Vector3 headPosition;
        private List<XRNodeState> nodeStates = new List<XRNodeState>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) && !trackHead)
            {
                AdjustExperimentHeight();
            }

            if (trackHead) {
                InputTracking.GetNodeStates(nodeStates);
                var headState = nodeStates.FirstOrDefault(node => node.nodeType == XRNode.Head);
                headState.TryGetPosition(out headPosition);
                
                Vector3 pos = display.transform.position;
                pos.y = headPosition.y;
                display.transform.position = pos;
            }
        }

        public void AdjustExperimentHeight()
        {
            Transform display_transform = display.transform;
            
            Debug.Log("THE LOCATION OF THE EXPERIMENT WAS ADJUSTED");
            Debug.Log("Current Experiment Position: " + display_transform.localPosition.ToString("F4"));

            InputTracking.GetNodeStates(nodeStates);
            var headState = nodeStates.FirstOrDefault(node => node.nodeType == XRNode.Head);
            headState.TryGetPosition(out headPosition);

            display_transform.localPosition = new Vector3(display_transform.localPosition.x, headPosition.y, display_transform.localPosition.z);

            Debug.Log("New Experiment Position: " + display_transform.position.ToString("F4"));

            if (session.hasInitialised) {
                session.participantDetails["height"] = headPosition.y;
            }
        }
    }
}