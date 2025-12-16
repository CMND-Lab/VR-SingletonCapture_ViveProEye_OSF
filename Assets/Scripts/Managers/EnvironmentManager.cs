using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

namespace VRSingletonSearch
{
    public class EnvironmentManager : MonoBehaviour
    {   
        public Session session;
        [SerializeField] List<GameObject> controlEnvironmentObjects;
        [SerializeField] List<GameObject> realisticEnvironmentObjects;

        [SerializeField] Material controlSkybox;
        [SerializeField] Material realisticSkybox;

        [SerializeField] GameObject cue;

        private void Awake() {
            SetRealisticEnvironment();
        }

        public void SetStartingEnvironment() {
            if (Convert.ToInt32(session.participantDetails["counterbalance"]) == 1) {
                SetRealisticEnvironment();
            } else {
                SetControlEnvironment();
            }
        }

        public void SwitchEnvironment() {
            if (Convert.ToInt32(session.participantDetails["counterbalance"]) == 1) {
                SetControlEnvironment();
            } else {
                SetRealisticEnvironment();
            }
        }

        public void SetRealisticEnvironment() {
            Debug.Log("Enabling realistic environment...");

            ShowObjects(realisticEnvironmentObjects, true);
            ShowObjects(controlEnvironmentObjects, false);

            cue.GetComponent<TextMesh>().color = Color.white;

            RenderSettings.skybox = realisticSkybox;
        }

        public void SetControlEnvironment() {
            Debug.Log("Enabling control environment...");

            ShowObjects(realisticEnvironmentObjects, false);
            ShowObjects(controlEnvironmentObjects, true);

            cue.GetComponent<TextMesh>().color = Color.white;

            RenderSettings.skybox = controlSkybox;
        }

        private void ShowObjects(List<GameObject> objects, bool show) {
            foreach (GameObject o in objects) {
                o.SetActive(show);
            }
        }
    }
}

