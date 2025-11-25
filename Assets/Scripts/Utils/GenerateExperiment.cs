using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;
namespace VRSingletonSearch
{
    public class GenerateExperiment : MonoBehaviour
    {
        
        public ResearcherCanvasController researcherCanvasController;

        private ResponseLocations[] targetLocations = {
            ResponseLocations.Left,
            ResponseLocations.Top,
            ResponseLocations.Right
        };
        private ResponseLocations[] distractorLocations = {
            ResponseLocations.Left,
            ResponseLocations.Top,
            ResponseLocations.Right
        };
        private string[] colours = {
            "green",
            "red"
        };

        private string GetRandomColour() {
            if (Random.value < 0.5f) {
                return colours[0];
            } else {
                return colours[1];
            }
        }

        private List<ResponseLocations[]> GenerateParameterCombinations() {
            List<ResponseLocations[]> combs = new List<ResponseLocations[]>();

            foreach (ResponseLocations target in targetLocations) {
                foreach(ResponseLocations distractor in distractorLocations) {
                    if (target != distractor) {
                        // For each trial with a distractor, there should be a trial without a distractor
                        combs.Add(new ResponseLocations[]{target, ResponseLocations.None});
                        combs.Add(new ResponseLocations[]{target, distractor});
                    }
                }
            }

            int count = combs.Count;
            int last = count - 1;
            for (int i = 0; i < last; i++) {
                int r = Random.Range(i, count);
                ResponseLocations[] tmp = combs[i];
                combs[i] = combs[r];
                combs[r] = tmp;
            }

            return combs;
        }

        private ResponseLocations GetRandomTargetLocation() {
            return targetLocations[(int)Random.Range(0, targetLocations.Length)];
        }

        private ResponseLocations GetRandomDistractorLocation(ResponseLocations target) {
            ResponseLocations distractor = distractorLocations[(int)Random.Range(0, distractorLocations.Length)];

            while (distractor == target) {
                distractor = distractorLocations[(int)Random.Range(0, distractorLocations.Length)];
            }

            return distractor;
        }
        
        private TargetShape GetRandomShape() {
            return (TargetShape) Random.Range(0, System.Enum.GetNames(typeof(TargetShape)).Length);
        }

        public void Generate(Session session)
        {
            // Get session settings from json
            int numberOfBaselineTrials = session.settings.GetInt("n_baseline_trials");
            int numberOfPracticeTrials = session.settings.GetInt("n_practice_trials");
            int numberOfBlocks = session.settings.GetInt("n_experimental_blocks");
            int maxTrials = session.settings.GetInt("max_experimental_trials");

            List<ResponseLocations[]> paramCombinations = GenerateParameterCombinations();

            int experimentalTrialMultiplier = 2;
            int numberOfLocations = paramCombinations.Count;
            int numberOfColours = colours.Length;
            int numberOfExperimentalTrials = numberOfLocations * numberOfColours * experimentalTrialMultiplier;

            numberOfExperimentalTrials = Mathf.Min(numberOfExperimentalTrials, maxTrials);

            Debug.Log("Creating " + numberOfExperimentalTrials + " trials per block...");

            session.settings.SetValue("n_experimental_trials", numberOfExperimentalTrials);
            

            //*** BASELINE BLOCK ***//

            Block baselineBlock = session.CreateBlock(numberOfBaselineTrials);
            baselineBlock.settings.SetValue("type", "baseline");
            
            for (int i = 0; i < numberOfBaselineTrials; i++)
            {
                ResponseLocations target = targetLocations[i % targetLocations.Length];
                string colour = GetRandomColour();
                TargetShape shape = GetRandomShape();
                
                baselineBlock.GetRelativeTrial(i + 1).settings.SetValue("target_shape", shape);
                baselineBlock.GetRelativeTrial(i + 1).settings.SetValue("target_location", target);
                baselineBlock.GetRelativeTrial(i + 1).settings.SetValue("colour_distractor_location", ResponseLocations.None);
                baselineBlock.GetRelativeTrial(i + 1).settings.SetValue("target_colour", colour);
            }
            
            baselineBlock.trials.Shuffle();
            

            //*** PRACTICE BLOCK ***//
            
            Block practiceBlock = session.CreateBlock(numberOfPracticeTrials);
            practiceBlock.settings.SetValue("type", "practice");
            
            for (int i = 0; i < numberOfPracticeTrials; i++)
            {
                // Pick random trial parameters
                // int combIndex = (int)Random.Range(0, locationCombinations.Count);
                // ResponseLocations[] locations = locationCombinations[combIndex];
                string colour = GetRandomColour();

                TargetShape shape = GetRandomShape();
                ResponseLocations target = GetRandomTargetLocation();
                ResponseLocations distractor = GetRandomDistractorLocation(target);

                if (i < numberOfPracticeTrials/2) {
                    distractor = ResponseLocations.None;
                }
                
                practiceBlock.GetRelativeTrial(i + 1).settings.SetValue("target_shape", shape);
                practiceBlock.GetRelativeTrial(i + 1).settings.SetValue("target_location", target);
                practiceBlock.GetRelativeTrial(i + 1).settings.SetValue("colour_distractor_location", distractor);
                practiceBlock.GetRelativeTrial(i + 1).settings.SetValue("target_colour", colour);
            }

            practiceBlock.trials.Shuffle();
            
            
            //*** EXPERIMENTAL BLOCKS ***//

            for (int blockIndex = 0; blockIndex < numberOfBlocks; blockIndex++)
            {
                Block experimentalBlock = new Block((uint) numberOfExperimentalTrials, session);
                experimentalBlock.settings.SetValue("type", "experiment");
                experimentalBlock.settings.SetValue("block", blockIndex);

                int trialIndex = 1;
                for (int repeat = 0; repeat < experimentalTrialMultiplier; repeat++) {
                    foreach (string c in colours) {
                        foreach (ResponseLocations[] locations in paramCombinations) {
                            ResponseLocations target = locations[0];
                            ResponseLocations distractor = locations[1];
                            TargetShape shape = (TargetShape) ((blockIndex + trialIndex) % System.Enum.GetNames(typeof(TargetShape)).Length);

                            experimentalBlock.GetRelativeTrial(trialIndex).settings.SetValue("target_shape", shape);
                            experimentalBlock.GetRelativeTrial(trialIndex).settings.SetValue("target_location", target);
                            experimentalBlock.GetRelativeTrial(trialIndex).settings.SetValue("colour_distractor_location", distractor);
                            experimentalBlock.GetRelativeTrial(trialIndex).settings.SetValue("target_colour", c);

                            trialIndex += 1;

                            if (trialIndex > maxTrials) {
                                break;
                            }
                        }
                        if (trialIndex > maxTrials) {
                            break;
                        }
                    }
                    if (trialIndex > maxTrials) {
                        break;
                    }
                }
                
                experimentalBlock.trials.Shuffle();
            }


            //*** RESEARCHER DISPLAY ***//
            researcherCanvasController.setExperimentText("Baseline");
            researcherCanvasController.setBlockText("");
            researcherCanvasController.setTrialText(1, Session.instance.settings.GetInt("n_baseline_trials"));
        }
        
    }
    public enum ResponseLocations
    {
        None,
        Left,
        Right,
        Top,
        Bottom
    }
}

