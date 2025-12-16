using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;
using TMPro;


namespace VRSingletonSearch
{
    public class TaskController : MonoBehaviour
    {
        public Session session;
        public StartingPointController startingPoint;
        public ResponseLocationManager responseLocationManager;

        public Material greenMat;
        public Material redMat;
        
        public AudioClip audioCorrect;
        public AudioClip audioIncorrect;

        public string response;    
        public bool correctResponse;
        public bool endTrial;
        public bool forceStop;

        public void Awake()
        {
            responseLocationManager.ActiveTargets(false);
            startingPoint.ResetStartOrb();
        }

        // Called at the start of each trial via UFX
        public void RunTrial(Trial trial)
        {
            response = "";
            correctResponse = false;
            endTrial = false;
            forceStop = false;

            StartCoroutine(TaskTrialSequence(trial));
        }

        void SetupTrialDisplay(Trial trial)
        {
            ResponseLocations target_location = (ResponseLocations)trial.settings.GetObject("target_location");
            ResponseLocations distractor_location = (ResponseLocations)trial.settings.GetObject("colour_distractor_location");
            string colour = trial.settings.GetString("target_colour");
            string trialType = trial.settings.GetString("type");
            TargetShape targetShape = (TargetShape) trial.settings.GetObject("target_shape");

            responseLocationManager.SetTarget(target_location, targetShape);

            Material mainMat;
            Material secondaryMat;
            if (colour == "green") {
                mainMat = greenMat;
                secondaryMat = redMat;
            } else {
                mainMat = redMat;
                secondaryMat = greenMat;
            }

            responseLocationManager.SetMainMaterial(mainMat);

            if (distractor_location != ResponseLocations.None) {
                responseLocationManager.SetDistractor(distractor_location, secondaryMat);
            }

            trial.result["target_location"] = target_location;
            trial.result["distractor_location"] = distractor_location;
            trial.result["target_colour"] = colour;
            trial.result["target_shape"] = targetShape;
            
            // Only show target object in baseline trials
            if (trialType == "baseline") {
                responseLocationManager.ShowTarget(target_location);
            } else {
                responseLocationManager.ActiveTargets(true);
            }
        }

        public void ResetTask() {
            DisableTargets();
            startingPoint.ResetStartOrb();
        }

        private void DisableTargets() {
            responseLocationManager.ActiveTargets(false);
        }
        
        // Coroutine for the trial behaviour
        IEnumerator TaskTrialSequence(Trial trial)
        {
            string trialType = trial.settings.GetString("type");
            session.CurrentTrial.result["type"] = trialType;
            
            // save start time for timeout timer
            float startTime = Time.time;
            
            // Turn off the starting point mesh renderer when the trial starts so that its not in the way
            startingPoint.HideOrb();

            Debug.Log("Running trial " + trial.number);
            SetupTrialDisplay(trial);

            Debug.Log("Waiting for trial response");
            yield return new WaitUntil(() => endTrial == true);

            if (correctResponse) {
                AudioSource.PlayClipAtPoint(audioCorrect, new Vector3(0, 0, 0), 0.1f);
            } else {
                AudioSource.PlayClipAtPoint(audioIncorrect, new Vector3(0, 0, 0), 0.1f);
            }

            // small delay to allow recording on a few extra frames into the response location
            yield return new WaitForSeconds(0.05f); 
            
            switch (trialType)
            {
                case ("baseline"):
                    trial.result["accuracy"] = 1;
                    trial.result["response"] = response;
                    break;

                default:
                    Debug.Log("Resetting non-baseline trial");
                    
                    // log the response the participant made
                    if (!forceStop) // if the trial did not time out
                    {
                        trial.result["response"] = response;
                        trial.result["correct"] = correctResponse;
                        
                        if (correctResponse)
                            trial.result["accuracy"] = 1;
                        else
                            trial.result["accuracy"] = 0;
                    }
                    else if (forceStop) {
                        session.CurrentTrial.result["accuracy"] = 0;
                        session.CurrentTrial.result["experimenter_ended"] = 1;
                    }
                    break;
            }


            // reset for next trial
            endTrial = false;
            forceStop = false;
            startingPoint.ResetStartOrb();

            // end current trial
            trial.End();   
        }

        public void EndIfLastTrial(Trial trial)
        {
            if (trial == Session.instance.LastTrial)
            {
                Session.instance.End();
            }
        }
    }
}
