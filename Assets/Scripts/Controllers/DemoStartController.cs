using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

namespace VRSingletonSearch
{
    public class DemoStartController : MonoBehaviour
    {
        public Session session;
        public GameObject canvas;
        private CanvasController canvasController;

        public TargetController leftOrb;
        public TargetController rightOrb;
        public TargetController topOrb;
        public TargetController bottomOrb;

        [SerializeField]
        private int KillDemoCount = 0;
        [SerializeField]
        private int numDemoTrials = 4;

        [Header("Display")]
        public Material startOrbMat;
        public Material startOrbMatLight;
        public Material responseOrbMat;
        public Material responseOrbMatLight;
        
        private DemoStartingState state;
        private TargetController glowingOrb;

        void Awake()
        {
            glowingOrb = null;
            canvasController = canvas.GetComponent<CanvasController>();
        }

        void Start() 
        {
            state = DemoStartingState.Waiting;
        }

        void Update()
        {               
            if (KillDemoCount == numDemoTrials)
            {
                canvasController.DemoEnd();
            }
        }

        public void LightRandomOrb()
        {
            ResponseLocations[] locations = {
                ResponseLocations.Left,
                ResponseLocations.Right,
                ResponseLocations.Top
            };

            ResponseLocations rand = locations[UnityEngine.Random.Range(0, locations.Length)];

            switch (rand)
            {
                case ResponseLocations.Left:
                    Debug.Log("Left Orb Light = On");
                    glowingOrb = leftOrb;
                    break;
                case ResponseLocations.Right:
                    Debug.Log("Right Orb Light = On");
                    glowingOrb = rightOrb;
                    break;
                case ResponseLocations.Top:
                    Debug.Log("Top Orb Light = On");
                    glowingOrb = topOrb;
                    break;
                case ResponseLocations.Bottom:
                    Debug.Log("Bottom Orb Light = On");
                    glowingOrb = bottomOrb;
                    break;
                default:
                    break; 
            }
            
            glowingOrb.GetComponent<DemoTargetController>().selected = false;
            SetOrbs();
        }

        public void SetOrbs() {
            TargetShape targetShape = TargetShape.Pear;
            TargetShape otherShape = TargetShape.Apple;

            topOrb.SetMesh(otherShape);
            topOrb.SetMaterial(responseOrbMat);
            leftOrb.SetMesh(otherShape);
            leftOrb.SetMaterial(responseOrbMat);
            rightOrb.SetMesh(otherShape);
            rightOrb.SetMaterial(responseOrbMat);
            bottomOrb.SetMesh(otherShape);
            bottomOrb.SetMaterial(responseOrbMat);

            glowingOrb?.SetMesh(targetShape);
            glowingOrb?.SetMaterial(responseOrbMatLight);
        }

        IEnumerator DemoSequence()
        {
            state = DemoStartingState.GetReady;
            yield return new WaitForSeconds(1.0f);
            state = DemoStartingState.Go;

            EnableStartOrb(false);
            LightRandomOrb();
            
            yield return new WaitUntil(() => glowingOrb.GetComponent<DemoTargetController>().selected);

            glowingOrb = null;
            KillDemoCount += 1;
            state = DemoStartingState.Waiting;
            
            SetOrbs();
            EnableStartOrb(true);
        }

        void EnableStartOrb(bool enable) {
            gameObject.GetComponent<SphereCollider>().enabled = true;
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        
        void OnTriggerEnter(Collider other)
        {
            gameObject.GetComponent<Renderer>().material = startOrbMatLight;

            if (other.CompareTag("Controller") && state == DemoStartingState.Waiting)
            {
                StartCoroutine(DemoSequence());
            }
        }
        private void OnTriggerExit(Collider other)
        {
            gameObject.GetComponent<Renderer>().material = startOrbMat;
            
            if (state == DemoStartingState.GetReady) 
            {
              StopAllCoroutines();
              state = DemoStartingState.Waiting;
            }
        }
    }
    
    public enum DemoStartingState
    {
        Waiting, GetReady, Go
    }
}