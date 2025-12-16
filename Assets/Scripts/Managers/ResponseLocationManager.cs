using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

namespace VRSingletonSearch
{
    public class ResponseLocationManager : MonoBehaviour
    {
        public Session session;
        
        [SerializeField] TargetController topResponseTarget;
        [SerializeField] TargetController bottomResponseTarget;
        [SerializeField] TargetController leftResponseTarget;
        [SerializeField] TargetController rightResponseTarget;

        private Dictionary<ResponseLocations, TargetController> controllerDict;
        
        public void Awake() {
            if (controllerDict == null) {
                controllerDict = new Dictionary<ResponseLocations, TargetController>();

                controllerDict[ResponseLocations.Top] = topResponseTarget;
                controllerDict[ResponseLocations.Left] = leftResponseTarget;
                controllerDict[ResponseLocations.Right] = rightResponseTarget;
                controllerDict[ResponseLocations.Bottom] = bottomResponseTarget;
            }
        }

        public void ActiveTargets(bool active) {
            leftResponseTarget.ShowTarget(active);
            bottomResponseTarget.ShowTarget(active);
            rightResponseTarget.ShowTarget(active);
            topResponseTarget.ShowTarget(active);
        }

        internal void ShowTarget(ResponseLocations target)
        {
            controllerDict[target].ShowTarget(true);
        }

        internal void SetTarget(ResponseLocations target_location, TargetShape targetShape)
        {
            TargetShape otherShape;

            if (targetShape == TargetShape.Apple) {
                otherShape = TargetShape.Pear;
            } else {
                otherShape = TargetShape.Apple;
            }

            topResponseTarget.SetMesh(otherShape);
            leftResponseTarget.SetMesh(otherShape);
            rightResponseTarget.SetMesh(otherShape);
            bottomResponseTarget.SetMesh(otherShape);

            controllerDict[target_location].SetMesh(targetShape);
            SetCorrectResponseFlag(target_location);
        }

        private void SetCorrectResponseFlag(ResponseLocations correctResponseLocation)
        {
            topResponseTarget.GetComponent<TrialTargetController>().SetCorrectResponseLocationFlag(correctResponseLocation == ResponseLocations.Top);
            bottomResponseTarget.GetComponent<TrialTargetController>().SetCorrectResponseLocationFlag(correctResponseLocation == ResponseLocations.Bottom);
            leftResponseTarget.GetComponent<TrialTargetController>().SetCorrectResponseLocationFlag(correctResponseLocation == ResponseLocations.Left);
            rightResponseTarget.GetComponent<TrialTargetController>().SetCorrectResponseLocationFlag(correctResponseLocation == ResponseLocations.Right);
        }

        internal void SetDistractor(ResponseLocations distractor, Material material)
        {
            controllerDict[distractor].SetMaterial(material);
        }

        internal void SetMainMaterial(Material material)
        {
            leftResponseTarget.SetMaterial(material);
            bottomResponseTarget.SetMaterial(material);
            rightResponseTarget.SetMaterial(material);
            topResponseTarget.SetMaterial(material);
        }
    }
}