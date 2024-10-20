using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BepInEx.Logging;
using ECM.Components;
using BepInEx.Configuration;

namespace SpeedMod.Checkpoints
{
    public class CheckpointDisplay : MonoBehaviour
    {
        // Logging
        internal static ManualLogSource Logger;
        private CheckpointManager checkpointManager;
        private CharacterMovement characterMovement;
        private GameObject climber;

    
        private Image[] checkpointImages;
        public Sprite checkpointSprite;
        private Canvas checkpointCanvas;

        private Color setColor = Color.green;  // Green for set checkpoints
        private Color unsetColor = new Color(1f, 0.53f, 0); // Orange for unset checkpoints
        private ConfigEntry<bool> displayCheckpoints;
        
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        public void Initialize(ManualLogSource logger, CheckpointManager cm, ConfigEntry<bool> dCheckpoints)
        {
            Logger = logger;
            this.checkpointManager = cm;
            checkpointManager.OnCheckpointUpdated += UpdateCheckpoints;
            displayCheckpoints = dCheckpoints;
            InitializeCanvas();
            Logger.LogInfo("CheckpointDisplay initialized.");
        }

        private void InitializeCanvas()
        {
            // Create a Canvas and set it to Screen Space - Overlay
            GameObject canvasObject = new GameObject("CheckpointCanvas");
            checkpointCanvas = canvasObject.AddComponent<Canvas>();
            checkpointCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

            DontDestroyOnLoad(canvasObject);

            // Create 3 checkpoint UI images
            checkpointImages = new Image[3];
            for (int i = 0; i < 3; i++)
            {
                GameObject checkpointObj = new GameObject("Checkpoint" + (i + 1));
                checkpointObj.transform.SetParent(checkpointCanvas.transform);

                // Create the Image component for the checkpoint
                Image checkpointImage = checkpointObj.AddComponent<Image>();
                checkpointImage.sprite = checkpointSprite;

                // Position the checkpoint squares (adjust X and Y coordinates as needed)
                RectTransform rectTransform = checkpointImage.GetComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(0, 0);
                rectTransform.pivot = new Vector2(0, 0);

                // Size of each square
                rectTransform.sizeDelta = new Vector2(10, 10);
                rectTransform.anchoredPosition = new Vector2(20 + (i * 20), 20);

                checkpointImages[i] = checkpointImage;
            }
            // Initialize the display
            UpdateCheckpoints();
            this.checkpointCanvas.enabled = false;

        }
        void OnDestroy()
        {
            // Unsubscribe from the event to avoid memory leaks
            checkpointManager.OnCheckpointUpdated -= UpdateCheckpoints;
        }
        void Update()
        {
            if (characterMovement == null)
            {
                characterMovement = FindObjectOfType<CharacterMovement>();
            }
            if (this.climber == null)
            {
                this.climber = GameObject.Find("Climber");
            }

            // Only call HandleInput if both references are not null
            if (characterMovement != null && this.climber != null)
            {
                this.checkpointManager.HandleInput(this.climber.transform, characterMovement);
                checkpointCanvas.enabled = displayCheckpoints.Value;
            }
        }

        void UpdateCheckpoints()
        {
            Logger.LogInfo("Updating checkpoints on GUI");
            if (checkpointCanvas.enabled)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (checkpointImages[i] == null)
                    {
                        Logger.LogError($"Checkpoint image {i + 1} is not initialized.");
                        continue;  // Skip this image if it wasn't initialized
                    }
                    // Check if the GameObject is active in the hierarchy
                    if (!checkpointImages[i].gameObject.activeInHierarchy)
                    {
                        Logger.LogError($"Checkpoint image {i + 1} is inactive.");
                        continue;  // Skip if the GameObject is disabled
                    }
                    bool isCheckpointSet = checkpointManager.IsCheckpointSet(i+1);
                    checkpointImages[i].color = isCheckpointSet ? setColor : unsetColor; // Set color based on checkpoint state
                    Logger.LogInfo($"{i}done");
                }
            }
        }
    }
}