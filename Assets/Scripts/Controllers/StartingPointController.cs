using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;
using UXFExamples;

namespace VRSingletonSearch
{
    public class StartingPointController : MonoBehaviour
    {
        public GameObject experimentManager;
        public Session session;
        public GameObject cue;
        public GameObject controller;
        public Material responseOrbMat;
        public Material responseOrbMatLight;

        [SerializeField] StartingStateVR state = StartingStateVR.Waiting;
        
        private Coroutine cueCoroutine;

        IEnumerator CueSequence()
        {
            state = StartingStateVR.GetReady;
            yield return new WaitForSeconds(1.0f);
            cue.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            cue.SetActive(false);
            
            state = StartingStateVR.Go;
            session.BeginNextTrial();
        }

        public void HideOrb() {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
    
        public void ResetStartOrb()
        {
            state = StartingStateVR.Waiting;

            gameObject.GetComponent<SphereCollider>().enabled = true;

            gameObject.GetComponent<Renderer>().material = responseOrbMat;
            gameObject.GetComponent<Renderer>().enabled = true;

            cue.SetActive(false);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Controller")) {
                gameObject.GetComponent<Renderer>().material = responseOrbMatLight;
                Debug.Log("Holding in Starting Orb");
                switch (state)
                {
                    case StartingStateVR.Waiting:
                        cueCoroutine = StartCoroutine(CueSequence());
                        break;
                }
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            gameObject.GetComponent<Renderer>().material = responseOrbMat;
            Debug.Log("Exited the Starting Orb");
            
            switch (state)
            {
                case StartingStateVR.GetReady:
                    StopCoroutine(cueCoroutine);
                    cue.SetActive(false);
                    ResetStartOrb();
                    break;

                case StartingStateVR.Go:
                    Vector3 p = controller.transform.position;

                    session.CurrentTrial.result["init_time"] = Time.time;
                    session.CurrentTrial.result["timed_out"] = 0;

                    session.CurrentTrial.result["init_pos_x"] = p.x;
                    session.CurrentTrial.result["init_pos_y"] = p.y;
                    session.CurrentTrial.result["init_pos_z"] = p.z;

                    Debug.Log("init_pos_x =" + p.x);
                    Debug.Log("init_pos_y =" + p.y);
                    Debug.Log("init_pos_z =" + p.z);

                    // Disable collider for rest of trial
                    gameObject.GetComponent<SphereCollider>().enabled = false;

                    break;
            }
        }
    }
    
    public enum StartingStateVR
    {
        Waiting, GetReady, Go
    }
}

