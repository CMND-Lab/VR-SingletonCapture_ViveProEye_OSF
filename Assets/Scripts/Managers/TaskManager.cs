using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.Extras;
using UXF;
using ViveSR.anipal.Eye;
using TMPro;

namespace VRSingletonSearch
{
    public class TaskManager : MonoBehaviour
    {
        
        public Session session;

        public ResearcherCanvasController researcherCanvasController;

        public EnvironmentManager environmentManager;
        
        [SerializeField] TaskController taskController;
        [SerializeField] CanvasController canvasController;
        
        public GameObject experiment;
        public GameObject canvas;
        
        private void Awake()
        {
            //instructionsText = instructions.GetComponent<TextMeshProUGUI>();

            //controllerCollider = controller.GetComponent<Collider>();
            
            canvasController = canvas.GetComponent<CanvasController>();
        }

        IEnumerator EndOfExperiment()
        {
            Debug.Log("Finalizing Session");
            yield return new WaitForSeconds(10.0f);
            session.End();
        }

        public void EndOfTrial(Trial trial) {
            taskController.ResetTask();

            string trialType = session.CurrentTrial.settings.GetString("type");

            researcherCanvasController.setAbsoluteTrial(session.currentTrialNum + 1);

            switch (trialType)
            {
                case("baseline"): // End of Baseline trial
                    researcherCanvasController.setTrialText(trial.numberInBlock + 1, session.settings.GetInt("n_baseline_trials"));
                    break;
                case("practice"): // End of Practice trial
                    researcherCanvasController.setTrialText(trial.numberInBlock + 1, session.settings.GetInt("n_practice_trials"));
                    break;
                case("experiment"): // End of Experiment trial
                    researcherCanvasController.setTrialText(trial.numberInBlock + 1, session.settings.GetInt("n_experimental_trials"));
                    break;
            }
        }

        public void EndOfBlock() // Is called at the end of each block of trials via the UXF Event system
        {
            Debug.Log("End of Block");
            
            // Get block type
            string trialType = session.CurrentTrial.settings.GetString("type");
            
            //Set EndTrial flag to false (this sometimes doesn't happen on the last trial via TaskController, not sure why)
            taskController.endTrial = false;
            Debug.Log(taskController.endTrial);

            switch (trialType)
            {
                
                case("baseline"): // End of Baseline Block
                    
                    Debug.Log("End of Baseline Block");

                    researcherCanvasController.setExperimentText("Practice");
                    researcherCanvasController.setTrialText(1, session.settings.GetInt("n_practice_trials"));

                    canvasController.EndOfBaselineBlock();
                    break;
                case("practice"): // End of Practice Block
                    
                    Debug.Log("End of Practice Block");

                    researcherCanvasController.setExperimentText("Experiment");
                    researcherCanvasController.setBlockText(1, session.settings.GetInt("n_experimental_blocks"));
                    researcherCanvasController.setTrialText(1, session.settings.GetInt("n_experimental_trials"));

                    canvasController.EndOfPracticeBlock();
                    break;
                case("experiment"): // End of Experiment Block
                    Debug.Log("End of Experimental Block");

                    // Block 1 = baseline
                    // Block 2 = practice
                    // Block 3+ = experiment
                    // Next experimental block num = (current block num - 2) + 1
                    int experimentBlock = session.currentBlockNum - 1;
                    researcherCanvasController.setBlockText(experimentBlock, session.settings.GetInt("n_experimental_blocks"));
                    researcherCanvasController.setTrialText(1, session.settings.GetInt("n_experimental_trials"));

                    canvasController.EndOfExperimentBlock();
                    break;
            }
            if(session.CurrentTrial == session.LastTrial && session.GetBlock(session.blocks.Count).lastTrial == session.CurrentTrial) // test whether some of this can be removed
            {
                StartCoroutine(EndOfExperiment());          
            }
            
        }
    }
}


