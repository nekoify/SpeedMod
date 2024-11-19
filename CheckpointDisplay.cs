using UnityEngine;
using UnityEngine.UI;
using ECM.Components;
using MelonLoader;

namespace SpeedMod.Checkpoints
{
    /// <summary>
    /// Displays checkpoints stored in the CheckpointManager class.
    /// </summary>
    public class CheckpointDisplay: MonoBehaviour
    {
        // Checkpoint & Player
        private CheckpointManager checkpointManager;

        // Checkpoint display
        public Canvas checkpointCanvas;
        private Image[] checkpointImages;
        public Sprite checkpointSprite;

        // Checkpoint display color
        private Color setColor = Color.green;  // Green for set checkpoints
        private Color unsetColor = new Color(1f, 0.53f, 0); // Orange for unset checkpoints

        /// <summary>
        /// Initialization function.
        /// </summary>
        /// <param name="cm">CheckpointManager object.</param>
        public void Initialize(CheckpointManager cm)
        {
            checkpointManager = cm;
            // Subscribe to the OnCheckpointUpdated action
            checkpointManager.OnCheckpointUpdated += UpdateCheckpoints;
            InitializeCanvas();
        }

        /// <summary>
        /// Initializes canvas objects to display checkpoints.
        /// </summary>
        private void InitializeCanvas()
        {
            // Create a Canvas and set it to Screen Space - Overlay
            GameObject canvasObject = new GameObject("CheckpointCanvas");
            checkpointCanvas = canvasObject.AddComponent<Canvas>();
            checkpointCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

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
        }

        void UpdateCheckpoints()
        {
            // Only calculate if canvas is enabled!
            if (checkpointCanvas.enabled)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (checkpointImages[i] == null)
                    {
                        continue;  // Skip this image if it wasn't initialized
                    }
                    // Check if the GameObject is active in the hierarchy
                    if (!checkpointImages[i].gameObject.activeInHierarchy)
                    {
                        continue;  // Skip if the GameObject is disabled
                    }
                    bool isCheckpointSet = checkpointManager.IsCheckpointSet(i + 1);
                    checkpointImages[i].color = isCheckpointSet ? setColor : unsetColor; // Set color based on checkpoint state
                }
            }
        }
    }
}