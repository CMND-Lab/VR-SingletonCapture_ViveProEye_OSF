using UnityEngine;
using System;
using UXF;


namespace VRSingletonSearch
{
    public class CanvasInstructions : MonoBehaviour
    {
        public Session session;

        private void Awake() {
        }

        public string[] getInitInstructions() {
            string[] initInstructions =
            {
                "Welcome to the experiment! Please take a moment to familiarise yourself with this virtual environment. <br><br> " +
                "If you experience any symptoms of motion sickness, please let your experimenter know right away.", // 0

                "First, we will check you are positioned correctly. \n\n " + 
                "Notice the <i>white orb</i> on the top of the controller. \n\n " + 
                "Hold this inside the <i>central orb</i> in front of you - this is the starting position you will take to begin each trial. \n\n " + 
                "Once the experimenter has checked your position, click <i>Next</i> to continue.", // 1

                "Now, notice the <i>four response targets</i> on the left, right, top, and bottom. \n\n " +
                "During this experiment, you will reach to touch these locations to make your responses. \n\n " +
                "To practice, please hold the controller in the central orb until one of the targets changes shape and lights up. \n\n" +
                "Reach to touch it with the white orb on the controller, then return to the central orb until another target lights up.", //2

                "Great job.\n\n " + 
                "Next, we are going to calibrate your eye movements. \n\n " +
                "To calibrate, select <i>Calibrate</i> below and then follow the instructions presented on the screen." // 3
            };

            return initInstructions;
        }

        public string[] getBaselineInstructions() {
            string[] baselineInstructions =
            {
                "Now, we are going to take some <i>baseline measurements</i> of your reaching movements.", // 0

                "Shortly, you will see the central orb in front of you. Place the controller inside the central orb. \n\n " + 
                "A fixation cross will appear - this should be positioned directly in the centre of your visual field. \n\n" +
                "After the fixation cross, you will see a response target appear to the left, right, top, or bottom. \n\n " +
                "Touch the target, then return back to the starting orb.", //1

                "Continue these steps until the " + session.settings.GetInt("n_baseline_trials") + " baseline trials are complete.", //2

                "Note - please keep the controller in the central orb until the response target appears. \n<i>Otherwise, the trial will restart.</i> \n\n" +
                "Press 'Start' to begin." //3
            };

            return baselineInstructions;
        }

        public string[] getPracticeInstructions() {
            string[] practiceInstructions =
            {
                "Baseline measurements complete. \n\n " +
                "Press <i>Next</i> to continue on to the experiment task.", // 0
                
                "TASK INSTRUCTIONS: <br><br>" + 
                "In this task, you will see a set of fruits in front of you, and your goal is to identify the odd shaped fruit.", // 1
                
                "There will be 4 fruits, and 1 will be a different shape to the other 3. \n" +
                "For example, there might be 3 apples and 1 pear. \n\n" +
                "When you have identified the unique fruit, touch it with the orb on your controller.", //2
                
                "The fruits may all be green or red. Sometimes, one of the fruits will be a different colour. \n\n" +
                "Ignore the colours and respond only to the fruit with a different shape.", // 3
                
                "To begin each trial, you must hold the controller inside the central orb. \n\n " +
                "A fixation cross will appear, followed by the fruits. \n\n " +
                "Please keep your controller inside the central orb until the fruits appear. \n<i>Otherwise, the trial will restart.</i>", // 4
                
                "REMEMBER: \n\n" +
                "Use your controller orb to touch the <i>unique shape</i>. \n\n " +
                "The colour of the fruits <i>doesn't matter</i>, you should only pay attention to the shape.", // 5
                
                "Please respond as <i>quickly as you can</i> while avoiding making errors.", // 6

                "We will begin with a practice round of " + session.settings.GetInt("n_practice_trials") + " trials. \n\n" + // 7
                "Press 'Start' to begin."
            };

            return practiceInstructions;
        }

        public string[] getExperimentInstructions() {
            string[] expInstructions =
            {
                "The practice round has finished. Great job!", // 0
                
                "We will now begin the experiment. \n\n" +
                "You will complete " + session.settings.GetInt("n_experimental_blocks") +
                " blocks of " + session.settings.GetInt("n_experimental_trials") + " trials.", // 1
                
                "You will be given an opportunity to have a short rest between each block. \n\n " +
                "At the halfway point, you will remove the headset and take a longer break. \n\n " +
                "If at any point during the experiment you would like to stop, or if you begin to experience any symptoms of cyber-sickness " + 
                " (e.g., nausea, sweating, dizziness, headache, eyestrain), please let the experimenter know.", // 2
                
                "REMEMBER: \n\n" +
                "Use your controller orb to touch the <i>unique shape</i>. \n\n " +
                "The colour of the fruits <i>doesn't matter</i>, you should only pay attention to the shape.", // 4
                
                "Please respond as quickly as you can while avoiding making errors. \n\n" +
                "Press <i>Start</i> when you are ready to begin the experiment." //5
            };

            return expInstructions;
        }
    }
}    
    
