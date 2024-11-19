using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ECM.Components;
using MelonLoader;


namespace SpeedMod.Speed
{
    public class SpeedDisplay : MonoBehaviour
    {
        // For displaying speed
        private float maxSpeed = 42.72002f;

        // UI elements for the speed text & bar
        public Text speedText;
        public Image speedBar;
        public Canvas speedCanvas;
        
        public void Initialize()
        {

            GameObject canvasObject = new GameObject("SpeedCanvas");
            speedCanvas = canvasObject.AddComponent<Canvas>();
            speedCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

            // Speed text
            GameObject textObject = new GameObject("SpeedText");
            textObject.transform.SetParent(speedCanvas.transform);
            speedText = textObject.AddComponent<Text>();

            // Set the text properties (font, size, color)
            speedText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            speedText.fontSize = 24;
            speedText.color = Color.white;
            speedText.alignment = TextAnchor.UpperLeft;
            speedText.horizontalOverflow = HorizontalWrapMode.Overflow;
            speedText.verticalOverflow = VerticalWrapMode.Truncate;

            // Position the speed display on the canvas (top-left corner)
            RectTransform textRectTransform = speedText.GetComponent<RectTransform>();
            textRectTransform.anchorMin = new Vector2(0, 1);  // Top-left anchor
            textRectTransform.anchorMax = new Vector2(0, 1);
            textRectTransform.pivot = new Vector2(0, 1);      // Pivot from the top-left
            textRectTransform.anchoredPosition = new Vector2(20, -30);  // Offset from top-left

            // Create an Image element to represent the speed bar
            GameObject barObject = new GameObject("SpeedBar");
            barObject.transform.SetParent(speedCanvas.transform);
            speedBar = barObject.AddComponent<Image>();

            // Create the texture for the speed bar and set it as the background
            speedBar.color = new Color(1f, 0.53f, 0);  // Orange color for the bar

            // Position and size the speed bar (just below the text)
            RectTransform barRectTransform = speedBar.GetComponent<RectTransform>();
            barRectTransform.anchorMin = new Vector2(0, 1);  // Top-left anchor
            barRectTransform.anchorMax = new Vector2(0, 1);
            barRectTransform.pivot = new Vector2(0, 1);      // Pivot from the top-left
            barRectTransform.sizeDelta = new Vector2(0, 5);  // Initial width of 0, height of 5
            barRectTransform.anchoredPosition = new Vector2(0, 0);

            // Initial speed value
            speedText.text = "Speed: 0";
        }

        /// <summary>
        /// Updates the bar's width according to the player's speed.
        /// </summary>
        /// <param name="speed">Player's speed.</param>
        public void DisplayBarOnGUI(float speed)
        {
            if (speedBar != null)
            {
                // Update the speed bar width dynamically based on the current speed
                float barWidth = Mathf.Clamp((speed / maxSpeed) * Screen.width, 0, Screen.width);  // Clamp width between 0 and 200
                RectTransform barRectTransform = speedBar.GetComponent<RectTransform>();
                barRectTransform.sizeDelta = new Vector2(barWidth, 5);
            }
        }
        
        /// <summary>
        /// Updates the speed text according to the player's speed.
        /// </summary>
        /// <param name="speed">Player's speed.</param>
        public void DisplayTextOnGUI(float speed)
        {
            if (speedText != null)
            {
                speedText.text = $"Speed: {speed:F2}";
            }
        }
    }
}