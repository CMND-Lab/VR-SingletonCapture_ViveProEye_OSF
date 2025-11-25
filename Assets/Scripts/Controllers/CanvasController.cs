using System.Collections;
using UnityEngine;
using System;
using Valve.VR.Extras;
using UXF;
using ViveSR.anipal.Eye;
using TMPro;


namespace VRSingletonSearch
{
    public class CanvasController : MonoBehaviour
    {
        public CalibrationResult calibrationResult;
        public SteamVR_LaserPointer laserPointer;
        public GameObject controller;
        private Collider controllerCollider;
        public GameObject experiment;
        public GameObject researcherDisplay;
        public GameObject calibrateButton;
        public GameObject backButton;
        public GameObject nextButton;
        public GameObject continueButton;
        public GameObject instructions;

        private TextMeshProUGUI instructionsText;
        private TextMeshProUGUI nextButtonText;

        public int numInstruction = 0;
        public int totInstructions;
        private bool startExperiment = false;
        private int displayStartOrb = 1;
        private int displayResponseTargets = 2;

        public bool calibrationCompleted = false;
        public Session session;

        public CanvasState canvasState;

        public DemoManager responseDemo;
        public LocationHandler experimentLocation;
        
        public CanvasInstructions canvasInstructions;
        public GhostController ghostController;

        public EnvironmentManager environmentManager;

        private string[] initInstructions;
        private string[] baselineInstructions;
        private string[] practiceInstructions;
        private string[] expInstructions;

        internal void Initialise() {
            GetInstructions();
            SetCanvasState(CanvasState.Init);
        }
        
        private void GetInstructions()
        {
            initInstructions = canvasInstructions.getInitInstructions();
            baselineInstructions = canvasInstructions.getBaselineInstructions();
            practiceInstructions = canvasInstructions.getPracticeInstructions();
            expInstructions = canvasInstructions.getExperimentInstructions();
        }

        private void Awake()
        {
            laserPointer.PointerClick += PointerClick;
            controllerCollider = controller.GetComponent<Collider>();
            instructionsText = instructions.GetComponent<TextMeshProUGUI>();
            nextButtonText = nextButton.GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            if (session.hasInitialised) {
                canvasState = CanvasState.Init;
                SetCanvasState();
            }
            else {
                nextButton.SetActive(false);
                backButton.SetActive(false);
                continueButton.SetActive(false);
                calibrateButton.SetActive(false);

                SetInstructions("Initialising experiment... feel free to look around!");
            }
            controllerCollider.enabled = false;
        }

        public void SetCanvasState() {
          SetCanvasState(this.canvasState);
        }
        
        public void SetCanvasState(CanvasState state)
        { 
            switch (state)
            {
                case CanvasState.Init:
                    continueButton.SetActive(false);
                    nextButton.SetActive(true);
                    calibrateButton.SetActive(false);
                    backButton.SetActive(false);
                    numInstruction = 0;
                    instructionsText.text = initInstructions[0];
                    totInstructions = initInstructions.Length - 1;
                    break;
                case CanvasState.Demo:
                    laserPointer.holder.SetActive(false);
                    laserPointer.pointer.SetActive(false);
                    controllerCollider.enabled = true;
                    continueButton.SetActive(false);
                    nextButton.SetActive(false);
                    calibrateButton.SetActive(false);
                    backButton.SetActive(false);
                    ghostController.MakeTransparent();
                    break;
                case CanvasState.Baseline:
                    continueButton.SetActive(false);
                    nextButton.SetActive(true);
                    calibrateButton.SetActive(false);
                    backButton.SetActive(false);
                    numInstruction = 0;
                    instructionsText.text = baselineInstructions[0];
                    totInstructions = baselineInstructions.Length - 1;
                    break;
                case CanvasState.Practice:
                    nextButtonText.text = "Next";
                    startExperiment = false;
                    continueButton.SetActive(false);
                    nextButton.SetActive(true);
                    calibrateButton.SetActive(false);
                    backButton.SetActive(false);
                    numInstruction = 0;
                    instructionsText.text = practiceInstructions[0];
                    totInstructions = practiceInstructions.Length - 1;
                    instructions.SetActive(true);
                    break;
                case CanvasState.Experiment:
                    nextButtonText.text = "Next";
                    startExperiment = false;
                    continueButton.SetActive(false);
                    nextButton.SetActive(true);
                    calibrateButton.SetActive(false);
                    backButton.SetActive(false);
                    numInstruction = 0;
                    instructionsText.text = expInstructions[0];
                    totInstructions = expInstructions.Length - 1;
                    instructions.SetActive(true);
                    break;
                case CanvasState.Break:
                    gameObject.SetActive(true);
                    experiment.SetActive(false);
                    continueButton.SetActive(true);
                    nextButton.SetActive(false);
                    calibrateButton.SetActive(true);
                    backButton.SetActive(false);
                    instructions.SetActive(true);
                    laserPointer.holder.SetActive(true);
                    laserPointer.pointer.SetActive(true);
                    controllerCollider.enabled = true;
                    break;
            }

            canvasState = state;
        }

        public void SetInstructions(string text) {
          instructionsText.text = text;
        }

        private void Next()
        {
            if (numInstruction < totInstructions)
            {
                numInstruction += 1;

                if (canvasState == CanvasState.Practice)
                {
                    instructionsText.text = practiceInstructions[numInstruction];
                }
                
                else if (canvasState == CanvasState.Experiment)
                {
                    instructionsText.text = expInstructions[numInstruction];
                }
                else
                {
                    instructionsText.text = baselineInstructions[numInstruction];
                }
            }
        }

        private void Back()
        {
            nextButton.SetActive(true);
            if (numInstruction > 0)
            {
                numInstruction -= 1;
                if (numInstruction == 0)
                {
                    backButton.SetActive(false);
                }

                switch (canvasState)
                {
                    case CanvasState.Init:
                        calibrateButton.SetActive(false);
                        instructionsText.text = initInstructions[numInstruction];
                        break;
                    case CanvasState.Baseline:
                        instructionsText.text = baselineInstructions[numInstruction];
                        break;
                    case CanvasState.Practice:
                        instructionsText.text = practiceInstructions[numInstruction];
                        break;
                    case CanvasState.Experiment:
                        instructionsText.text = expInstructions[numInstruction];
                        break;
                }
            }
        }

        public void PointerClick(object sender, PointerEventArgs e)
        {
            // ******************CALIBRATION****************** //
            if (e.target.name == "Calibrate")
            {
                // TODO: Verify this is correct
                int successfulcalibration = SRanipal_Eye_API.LaunchEyeCalibration(IntPtr.Zero);
                //if (successfulcalibration == (int) Error.WORK)
                //if(calibrationResult == CalibrationResult.SUCCESS) 
                if(successfulcalibration == 0)
                {
                    //SRanipal_Eye_API.IsUserNeedCalibration(ref cal_need);
                    //if (cal_need)
                    // {
                    //     calibrateButton.SetActive(true);
                    //     nextButton.SetActive(true);
                    //     backButton.SetActive(false);
                    //     nextButtonText.text = "Skip";
                    //     instructionsText.text = "Calibration was unsuccessful. Please try again or press 'skip'.";
                    // }
                    // if (!cal_need)
                    //{
                        calibrateButton.SetActive(false);
                        Debug.Log("Successful Calibration");
                        calibrationCompleted = true;
                        nextButtonText.text = "Next";
                        
                        if (canvasState == CanvasState.Init)
                        {
                            nextButton.SetActive(true);
                            backButton.SetActive(false);
                            instructionsText.text = "Calibration was successful. Please press “Next”.";
                        }
                        else
                        {
                            continueButton.SetActive(true);
                            instructionsText.text = "Press “Continue” when you are ready.";
                        }
                }

                //if (successfulcalibration != (int) Error.WORK)
                //if(calibrationResult == CalibrationResult.FAIL)
                if(successfulcalibration != 0)
                {
                    Debug.Log(calibrationResult);
                    Debug.Log("Calibration Failed");
                    calibrateButton.SetActive(true);
                    nextButton.SetActive(true);
                    backButton.SetActive(false);
                    nextButtonText.text = "Skip";
                    instructionsText.text = "Calibration was unsuccessful. Please try again or press 'skip'.";
                }
                // ********************************************** //
            }

            if (e.target.name == "Skip")
            {
                nextButtonText.text = "Next";
                switch (canvasState)
                {
                    case CanvasState.Init:
                        canvasState = CanvasState.Baseline;
                        SetCanvasState();
                        break;
                    case CanvasState.Experiment:
                        break;
                }
            }
            if (e.target.name == "Next")
            {
                switch (canvasState)
                {
                    case CanvasState.Init:
                        backButton.SetActive(true);
                        if (numInstruction < totInstructions)
                        {
                            calibrateButton.SetActive(false);
                            numInstruction += 1;
                            instructionsText.text = initInstructions[numInstruction];
                            if (numInstruction == displayStartOrb)
                            {
                                experimentLocation.AdjustExperimentHeight();
                                responseDemo.startResponseDemo("startOrb");
                            }
                            if (numInstruction == displayResponseTargets)
                            {
                                canvasState = CanvasState.Demo;
                                SetCanvasState();
                                responseDemo.startResponseDemo("responseTargets");
                            }
                        }

                        if (numInstruction == totInstructions)
                        {
                            canvasState = CanvasState.Baseline;
                            SetCanvasState();
                        }
                        break;
                    
                    default:
                        backButton.SetActive(true);
                        if (!startExperiment)
                        {
                            Next();
                            if (numInstruction == totInstructions)
                            {
                                nextButtonText.text = "Start";
                                startExperiment = true;
                            }
                        }
                        else // This is where experiment is shown, so begins each block
                        {
                            experiment.SetActive(true);
                            ghostController.MakeTransparent();
                            laserPointer.holder.SetActive(false);
                            laserPointer.pointer.SetActive(false);
                            controllerCollider.enabled = true;
                            researcherDisplay.SetActive(true);
                            gameObject.SetActive(false);
                        }
                        break;
                    
                }
            }

            if (e.target.name == "Back")
            {
                Back();

                if (nextButtonText.text == "Start")
                {
                    startExperiment = false;
                    nextButtonText.text = "Next";
                }
            }

            if (e.target.name == "Continue")
            {
                if (canvasState == CanvasState.Init) {
                    calibrationCompleted = true;
                    SetCanvasState(CanvasState.Baseline);
                } else {
                    experiment.SetActive(true);
                    ghostController.MakeTransparent();
                    laserPointer.holder.SetActive(false);
                    laserPointer.pointer.SetActive(false);
                    controllerCollider.enabled = true;
                    researcherDisplay.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
        }

        public void DemoEnd()
        {
            canvasState = CanvasState.Init;
            laserPointer.holder.SetActive(true);
            laserPointer.pointer.SetActive(true);
            controllerCollider.enabled = false;
            nextButton.SetActive(false);
            calibrateButton.SetActive(true);
            backButton.SetActive(false);
            ghostController.MakeOpaque();
            instructionsText.text = initInstructions[3];
            
            continueButton.SetActive(false);

            responseDemo.HideAll();
        }
        
        public void EndOfBaselineBlock()
        {
            // Set Controller to UI mode
            laserPointer.holder.SetActive(true);
            laserPointer.pointer.SetActive(true);
            controllerCollider.enabled = false;
            ghostController.MakeOpaque();
            
            // Turn off Experiment, Turn on Canvas
            experiment.SetActive(false);
            gameObject.SetActive(true);
            
            // Set Canvas State to Practice
            canvasState = CanvasState.Practice;
            SetCanvasState();

            environmentManager.SetStartingEnvironment();
        }

        public void EndOfPracticeBlock()
        {
            // Set Controller to UI mode
            laserPointer.holder.SetActive(true);
            laserPointer.pointer.SetActive(true);
            controllerCollider.enabled = false;
            ghostController.MakeOpaque();
            
            // Turn off Experiment, Turn on Canvas
            experiment.SetActive(false);
            gameObject.SetActive(true);
            
            // Set Canvas State to Experiment
            canvasState = CanvasState.Experiment;
            SetCanvasState();
        }

        IEnumerator BreakDelaySequence()
        {
            yield return new WaitForSeconds(60);
            calibrateButton.SetActive(true);
            environmentManager.SwitchEnvironment();
        }

        public void EndOfExperimentBlock()
        {
            // Set Controller to UI mode
            laserPointer.holder.SetActive(true);
            laserPointer.pointer.SetActive(true);
            controllerCollider.enabled = false;
            ghostController.MakeOpaque();
            
            // Turn off Experiment, Turn on Canvas
            experiment.SetActive(false);
            gameObject.SetActive(true);
            
            backButton.SetActive(false);
            nextButton.SetActive(false);
            
            
            if ((session.currentBlockNum - 2) * 2 == (session.blocks.Count - 2))
            {
                instructionsText.text =
                      "You are halfway! <br> Please remove the headset and take a break. <br><br> Recalibrate before you continue the experiment."; // Mandatory Break
                continueButton.SetActive(false);
                calibrateButton.SetActive(false);

                StartCoroutine(BreakDelaySequence());
            }
            else if(session.CurrentTrial == session.LastTrial && session.GetBlock(session.blocks.Count).lastTrial == session.CurrentTrial)
            {
                  instructionsText.text = 
                      "This is the end of the experiment. Thank you so much for participating!";
                  calibrateButton.SetActive(false);
                  continueButton.SetActive(false);
            }
            else
            {
                instructionsText.text = 
                    "Please take a short 10-15 second break and then press continue to begin the next block when you are ready.<br><br>" +
                    "Please remember to go as quickly as you can while avoiding making errors.<br><br><br><br>" +
                    "Note: If you removed the headset during this break, please recalibrate before starting the next block.";
                calibrateButton.SetActive(true);
                continueButton.SetActive(true);
            }
            instructions.SetActive(true);
        }

    }
    public enum CanvasState
    {
        Init,
        Demo,
        Baseline,
        Practice,
        Experiment,
        Break
    }
    
    
}
  
