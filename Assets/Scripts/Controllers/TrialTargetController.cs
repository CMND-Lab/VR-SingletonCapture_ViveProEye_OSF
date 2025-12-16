using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

namespace VRSingletonSearch
{
    public class TrialTargetController : MonoBehaviour
    {
        public Session session;
        public GameObject controller;
        public TaskController taskcontroller;
       
        public bool isCorrectResponseLocation;

       
        public void SetCorrectResponseLocationFlag(bool thisIsCorrect)
        {
            isCorrectResponseLocation = thisIsCorrect;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Controller"))
            {
                Debug.Log("Selected the " + gameObject.tag + "response location.");
                
                Vector3 p = controller.transform.position;

                session.CurrentTrial.result["response_time"] = Time.time;
                session.CurrentTrial.result["fin_pos_x"] = p.x;
                session.CurrentTrial.result["fin_pos_y"] = p.y;
                session.CurrentTrial.result["fin_pos_z"] = p.z;
                
                // assign response location tag to the Task Manager
                taskcontroller.response = gameObject.tag;
                
                // signal end trial to the Task Manager
                taskcontroller.correctResponse = isCorrectResponseLocation;
                taskcontroller.endTrial = true;
            }            
        }
    }
}