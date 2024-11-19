using MelonLoader;
using UnityEngine;
using ECM.Components;
using SpeedMod.Checkpoints;
using SpeedMod.Speed;


[assembly: MelonInfo(typeof(SpeedMod.Core), "SpeedMod", "1.0.0", "Boo!", null)]
[assembly: MelonGame("Rubeki Games", "LornsLure")]

namespace SpeedMod
{
    public class Core : MelonMod
    {
        List<string> excludedScenes = new List<string> {
            "RubekiStudio", "LanguageSelectScreen", "Menu"
        };

        // For Checkpoint handling
       // private CheckpointDisplay checkpointDisplay;
        private CheckpointManager checkpointManager;
       // private GameObject displayObject;
        private GameObject speedObject;
        private SpeedDisplay speedDisplay;

        // Player and Character movement
        private CharacterMovement characterMovement;
        private GameObject climber;
        private bool modEnabled = false;

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (!excludedScenes.Contains(sceneName))
            {
                modEnabled = true;
                // Initializing a CheckpointManager
                checkpointManager = new CheckpointManager();
              /*  
                // Initializing CheckpointDisplay
                displayObject = new GameObject("CheckpointDisplay");
                checkpointDisplay = displayObject.AddComponent<CheckpointDisplay>();
                checkpointDisplay.Initialize(checkpointManager);
                */
                // Initializing SpeedDisplay
                speedObject = new GameObject("SpeedDisplay");
                speedDisplay = speedObject.AddComponent<SpeedDisplay>();
                speedDisplay.Initialize();
            }
        }

        /// <summary>
        /// Handles checkpoints when related keys are pressed.
        /// </summary>
        void HandleInput(Transform climberTransform, CharacterMovement characterMovement)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                checkpointManager.SetCheckpoint(climberTransform);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
             {
                 /*
                Component[] components = climber.GetComponents<Component>();

            foreach (Component component in components)
            {
                MelonLogger.Msg($"Component: {component.GetType().Name}");
            }
            */
                checkpointManager.LoadCheckpoint(climberTransform, characterMovement);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                checkpointManager.DeleteCheckpoint();
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                checkpointManager.ResetCheckpoints();
            }
        }


        public override void OnUpdate()
        {
            if (!modEnabled)
            {
                return;
            }
            // Loading character movement and climber to get velocity
            if (characterMovement == null)
            {
                characterMovement = UnityEngine.Object.FindObjectOfType<CharacterMovement>();
            }
            if (climber == null)
            {
                climber = GameObject.Find("Climber");
            }

            // Only create UI elements if both references are not null
            if (characterMovement != null && climber != null)
            {
                HandleInput(climber.transform, characterMovement);
                speedDisplay.DisplayBarOnGUI(characterMovement.velocity.magnitude);
                speedDisplay.DisplayTextOnGUI(characterMovement.velocity.magnitude);
            }
        }
    }
}