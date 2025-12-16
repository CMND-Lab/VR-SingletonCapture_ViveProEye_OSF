using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRSingletonSearch
{
    public class DemoTargetController : MonoBehaviour
    {
        public bool selected = false;
       
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Controller"))
            {                
                selected = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Controller"))
            {
                selected = false;
            }
        }
    }
}