using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRSingletonSearch
{
    public class DemoManager : MonoBehaviour
    {

        public GameObject demoTargets;
        public DemoStartController demoBehaviour;
        
        // Start is called before the first frame update
        void Awake()
        { 
            HideAll();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void startResponseDemo(string thing)
        {
            if (thing == "startOrb")
            {
                demoBehaviour.gameObject.SetActive(true);
            }
            if (thing == "responseTargets")
            {
                demoBehaviour.SetOrbs();
                demoTargets.SetActive(true);
            }
        }

        internal void HideAll()
        {
            demoBehaviour.gameObject.SetActive(false);
            demoTargets.SetActive(false);
        }
    }
    
}