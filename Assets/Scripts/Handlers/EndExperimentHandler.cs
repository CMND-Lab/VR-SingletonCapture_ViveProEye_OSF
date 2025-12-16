using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

namespace VRSingletonSearch
{
    public class EndExperimentHandler : MonoBehaviour
    {
        public Session session;
        public TaskController taskController;
        
        [SerializeField]
        float secsToEnd = 5;

        private float endCounter;

        private void Awake() {
            endCounter = 0;
        }

        private void Update()
        {
            /*
             * Press ESCAPE if the experiment needs to be stopped 
             */
            float increment = 1 / secsToEnd * Time.deltaTime;

            if (Input.GetKey(KeyCode.Escape))
            {
                Debug.Log("Keep holding to end the experiment.");
                endCounter += increment;
            }
            else {
                endCounter = Mathf.Max(0, endCounter - increment);
            }

            if (endCounter >= 1) {
                EndExperiment();
            }
        }

        private void EndExperiment() {
            Debug.Log("Ending experiment...");

            // End current trial if one is running
            if (Session.instance.InTrial) {                
                session.CurrentTrial.result["accuracy"] = 0;
                session.CurrentTrial.result["experimenter_ended"] = 1;

                session.CurrentTrial.End();
            }

            session.End();
        }
    }
}

