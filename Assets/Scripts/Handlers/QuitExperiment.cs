using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

namespace VRFlankerTask
{
    public class QuitExperiment : MonoBehaviour
    {        
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

            Session.instance.End();

            #if UNITY_EDITOR_WIN
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}