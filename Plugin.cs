using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using ECM.Components;
using SpeedMod.Checkpoints;
using SpeedMod.Speed;
using BepInEx.Configuration;

namespace SpeedMod;

[BepInProcess("LornsLure.exe")]
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    // Logging
    internal static new ManualLogSource Logger;
    // For Checkpoint handling
    private CheckpointDisplay checkpointDisplay;
    private CheckpointManager checkpointManager;
    private GameObject displayObject;
    private GameObject speedObject;
    private SpeedDisplay speedDisplay;

    // Player and Character movement
    private CharacterMovement characterMovement;
    private GameObject climber;
    
    // Configuration
    public static ConfigEntry<bool> DisplayBar;
    public static ConfigEntry<bool> DisplaySpeed;
    public static ConfigEntry<bool> DisplayCheckpoints;
    public static ConfigEntry<KeyboardShortcut> ResetCheckpoints;
    public static ConfigEntry<KeyboardShortcut> SetCheckpoint1;
    public static ConfigEntry<KeyboardShortcut> SetCheckpoint2;
    public static ConfigEntry<KeyboardShortcut> SetCheckpoint3;
    public static ConfigEntry<KeyboardShortcut> LoadCheckpoint1;
    public static ConfigEntry<KeyboardShortcut> LoadCheckpoint2;
    public static ConfigEntry<KeyboardShortcut> LoadCheckpoint3;

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        this.enabled = true;
        this.gameObject.SetActive(true);
        DontDestroyOnLoad(this.gameObject);
        AddConfig();
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded.");
    }

    /// <summary>
    /// Adds the configuration keys to BepinEx Configuration.
    /// </summary>
    private void AddConfig()
    {
        
        DisplayBar = Config.Bind("General.Toggles", "Display the speed bar:", true);
        DisplaySpeed = Config.Bind("General.Toggles", "Display speed units:", true);
        DisplayCheckpoints = Config.Bind("General.Toggles", "Display the checkpoints:", true);

        SetCheckpoint1 = Config.Bind("General", "Set checkpoint 1:", new KeyboardShortcut(KeyCode.F2));
        SetCheckpoint2 = Config.Bind("General", "Set checkpoint 2:", new KeyboardShortcut(KeyCode.F3));
        SetCheckpoint3 = Config.Bind("General", "Set checkpoint 3:", new KeyboardShortcut(KeyCode.F4));

        LoadCheckpoint1 = Config.Bind("General", "Load checkpoint 1", new KeyboardShortcut(KeyCode.Alpha2));
        LoadCheckpoint2 = Config.Bind("General", "Load checkpoint 2", new KeyboardShortcut(KeyCode.Alpha3));
        LoadCheckpoint3 = Config.Bind("General", "Load checkpoint 3", new KeyboardShortcut(KeyCode.Alpha4));

        ResetCheckpoints = Config.Bind("General", "Reset checkpoints:", new KeyboardShortcut(KeyCode.Y));
    }

    void Start()
    {
        // Initializing a CheckpointManager
        checkpointManager = new CheckpointManager(Logger);

        // Initializing CheckpointDisplay
        displayObject = new GameObject("CheckpointDisplay");
        checkpointDisplay = displayObject.AddComponent<CheckpointDisplay>();
        checkpointDisplay.Initialize(Logger, checkpointManager);
        
        // Initializing SpeedDisplay
        speedObject = new GameObject("SpeedDisplay");
        speedDisplay = speedObject.AddComponent<SpeedDisplay>();
        speedDisplay.Initialize(Logger);
    }

    /// <summary>
    /// Handles checkpoints when related keys are pressed.
    /// </summary>
    void HandleInput(Transform climberTransform, CharacterMovement characterMovement)
    {
        if (Input.GetKeyDown(SetCheckpoint1.Value.MainKey))
        {
            checkpointManager.SetCheckpoint(1, climberTransform);
        }
        if (Input.GetKeyDown(SetCheckpoint2.Value.MainKey))
        {
            checkpointManager.SetCheckpoint(2, climberTransform);
        }
        if (Input.GetKeyDown(SetCheckpoint3.Value.MainKey))
        {
            checkpointManager.SetCheckpoint(3, climberTransform);
        }

        if (Input.GetKeyDown(LoadCheckpoint1.Value.MainKey))
        {
            checkpointManager.LoadCheckpoint(1, climberTransform, characterMovement);
        }

        if (Input.GetKeyDown(LoadCheckpoint2.Value.MainKey))
        {
            checkpointManager.LoadCheckpoint(2, climberTransform, characterMovement);
        }

        if (Input.GetKeyDown(LoadCheckpoint3.Value.MainKey))
        {
            checkpointManager.LoadCheckpoint(3, climberTransform, characterMovement);
        }

        if (Input.GetKeyDown(ResetCheckpoints.Value.MainKey))
        {
            checkpointManager.ResetCheckpoints();
        }
    }

    void Update()
    {
        // Loading character movement and climber to get velocity
        if (characterMovement == null)
        {
            characterMovement = FindObjectOfType<CharacterMovement>();
        }
        if (climber == null)
        {
            climber = GameObject.Find("Climber");
        }

        // Only create UI elements if both references are not null
        if (characterMovement != null && climber != null)
        {
            HandleInput(climber.transform, characterMovement);

            if (DisplayBar.Value)
            {
                speedDisplay.DisplayBarOnGUI(characterMovement.velocity.magnitude);
            }
            if (DisplaySpeed.Value)
            {
                speedDisplay.DisplayTextOnGUI(characterMovement.velocity.magnitude);
            }

            // Config update
            checkpointDisplay.checkpointCanvas.enabled = DisplayCheckpoints.Value;
            speedDisplay.speedText.enabled = DisplaySpeed.Value;
            speedDisplay.speedBar.enabled = DisplayBar.Value;
        }
    }
}
