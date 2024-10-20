using UnityEngine;
using BepInEx.Logging;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ECM.Components;
using BepInEx.Configuration;


namespace SpeedMod.Speed
{
    public class SpeedManager : MonoBehaviour
    {
        // Logging
        internal static ManualLogSource Logger;
        // For displaying speed
        private CharacterMovement characterMovement; // This object stores the player's velocity
        private float maxSpeed = 42.72002f;
        private float speed;
        private ConfigEntry<bool> displayBar;
        private ConfigEntry<bool> displaySpeed;

        private Text speedText;
        private Image speedBar;  // UI Image element for the speed bar
        private Canvas speedCanvas;
        
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
        void Start()
        {
            GameObject canvasObject = new GameObject("SpeedCanvas");
            speedCanvas = canvasObject.AddComponent<Canvas>();
            speedCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

            DontDestroyOnLoad(canvasObject);

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
            speedText.enabled = false;
            speedBar.enabled = false;
        }

        public void Initialize(ManualLogSource logger,
                               ConfigEntry<bool> dSpeed,
                               ConfigEntry<bool> dBar)
        {
            Logger = logger;
            displaySpeed = dSpeed;
            displayBar = dBar;
        }

        void Update()
        {
            if (characterMovement == null)
            {
                characterMovement = FindObjectOfType<CharacterMovement>();
            }
            else
            {
                // Get the player's speed
                speed = characterMovement.velocity.magnitude;
                
                // Update the speed text only if it's enabled
                if (displaySpeed.Value)
                {
                    speedText.text = $"Speed: {speed:F2}";
                    speedText.enabled = true;
                }
                else
                {
                    speedText.enabled = false;
                }

                if (displayBar.Value)
                {
                    // Update the speed bar width dynamically based on the current speed
                    float barWidth = Mathf.Clamp((speed / maxSpeed) * Screen.width, 0, Screen.width);  // Clamp width between 0 and 200
                    RectTransform barRectTransform = speedBar.GetComponent<RectTransform>();
                    barRectTransform.sizeDelta = new Vector2(barWidth, 5);
                    speedBar.enabled = true;
                }
                else
                {
                    speedBar.enabled = false;
                }
            }
        }
    }
}