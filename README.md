# VR Singleton Detection Task

This project is for a singleton detection task, implemented in Unity, using UXF for data collection: https://github.com/immersivecognition/unity-experiment-framework.
On each trial, an array of 4 fruits will be presented, with the goal of touching the uniquely shaped fruit.

## Requirements
- ViveEyePro VR headset
- Unity Hub & Unity v2021.3.45f
- SteamVR
- Tobii eye tracking: https://developer.tobiipro.com/index.html
  - NOTE: The experiment will work without eye tracking

## Getting Started

1. Using Git bash or a Command Prompt window, clone the repo:
```
git clone https://github.com/CMND-Lab/VR-SingletonCapture_ViveProEye_OSF.git
```

2. Import the project into Unity Hub & open it

3. If SteamVR doesn't open automatically, open it

4. Start the tobii eye tracking runtime

5. Run the project using the Play button at the top of the Unity window, then fill in the information in the UXF startup window
   - The settings file should be loaded automatically
   - After the data directory has been selected once, it will be automatically loaded in future sessions
   - The dice button can be used to generate a random participant name

