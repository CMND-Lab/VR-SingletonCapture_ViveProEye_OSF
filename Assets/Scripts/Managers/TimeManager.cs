using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

namespace VRSingletonSearch
{
    public class TimeManager : MonoBehaviour
    {
        public Session session;

        private TaskController taskController;

        public GameObject controller;

        [SerializeField] float waitingTime = 3;

        private Coroutine countdown;

        private void Awake() {
            taskController = gameObject.GetComponent<TaskController>();
        }

        public void BeginCountdown()
        {
            if (session.CurrentTrial.settings.GetString("type") == "experiment" && waitingTime > 0)
            {
                countdown = StartCoroutine(Countdown());
            }
        }

        public void StopCountdown()
        {
            if (countdown != null)
            {
                StopCoroutine(countdown);
            }
        }

        IEnumerator Countdown()
        {
            yield return new WaitForSeconds(waitingTime);
            Vector3 p = controller.transform.position;

            session.CurrentTrial.result["accuracy"] = 0;
            session.CurrentTrial.result["timed_out"] = 1;
            session.CurrentTrial.result["final_time"] = Time.time;
            session.CurrentTrial.result["fin_pos_x"] = p.x;
            session.CurrentTrial.result["fin_pos_y"] = p.y;
            session.CurrentTrial.result["fin_pos_z"] = p.z;

            taskController.endTrial = true;
            taskController.forceStop = true;
        }
    }
}


