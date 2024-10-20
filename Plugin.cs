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
    private SpeedManager speedManager;
    
    // Configuration
    public static string DisplayBarKey = "Display the speed bar:";
    public static ConfigEntry<bool> DisplayBar;
    public static string DisplaySpeedKey = "Display speed units:";
    public static ConfigEntry<bool> DisplaySpeed;
    public static string DisplayCheckpointsKey = "Display the checkpoints:";
    public static ConfigEntry<bool> DisplayCheckpoints;

    public static string ResetCheckpointsKey = "Reset checkpoints:";
    public static ConfigEntry<KeyboardShortcut> ResetCheckpoints;

    public static string SetCheckpoint1Key = "Set checkpoint 1";
    public static ConfigEntry<KeyboardShortcut> SetCheckpoint1;
    public static string LoadCheckpoint1Key = "Load checkpoint 1";
    public static ConfigEntry<KeyboardShortcut> LoadCheckpoint1;

    public static string SetCheckpoint2Key = "Set checkpoint 2";
    public static ConfigEntry<KeyboardShortcut> SetCheckpoint2;
    public static string LoadCheckpoint2Key = "Load checkpoint 2";
    public static ConfigEntry<KeyboardShortcut> LoadCheckpoint2;

    public static string SetCheckpoint3Key = "Set checkpoint 3";
    public static ConfigEntry<KeyboardShortcut> SetCheckpoint3;
    public static string LoadCheckpoint3Key = "Load checkpoint 3";
    public static ConfigEntry<KeyboardShortcut> LoadCheckpoint3;

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        this.enabled = true;
        this.gameObject.SetActive(true);
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded.");

        DisplayBar = Config.Bind("General.Toggles", DisplayBarKey, true, 
                                 new ConfigDescription("Display the speed bar at the top of the screen."));
        DisplaySpeed = Config.Bind("General.Toggles", DisplaySpeedKey, true,
                                   new ConfigDescription("Display the speed at the top left of the screen."));
        DisplayCheckpoints = Config.Bind("General.Toggles", DisplayCheckpointsKey, true,
                                   new ConfigDescription("Display the user checkpoints."));

        SetCheckpoint1 = Config.Bind("General", SetCheckpoint1Key, 
                                     new KeyboardShortcut(KeyCode.F2));
        SetCheckpoint2 = Config.Bind("General", SetCheckpoint2Key, 
                                     new KeyboardShortcut(KeyCode.F3));
        SetCheckpoint3 = Config.Bind("General", SetCheckpoint3Key, 
                                     new KeyboardShortcut(KeyCode.F4));

        LoadCheckpoint1 = Config.Bind("General", LoadCheckpoint1Key, 
                                     new KeyboardShortcut(KeyCode.Alpha2));
        LoadCheckpoint2 = Config.Bind("General", LoadCheckpoint2Key, 
                                     new KeyboardShortcut(KeyCode.Alpha3));
        LoadCheckpoint3 = Config.Bind("General", LoadCheckpoint3Key, 
                                     new KeyboardShortcut(KeyCode.Alpha4));

        ResetCheckpoints = Config.Bind("General", ResetCheckpointsKey, 
                                     new KeyboardShortcut(KeyCode.W));
    }

    void Start()
    {
        // Initializing a CheckpointManager
        checkpointManager = new CheckpointManager(Logger,
                                                  SetCheckpoint1,
                                                  SetCheckpoint2,
                                                  SetCheckpoint3,
                                                  LoadCheckpoint1,
                                                  LoadCheckpoint2,
                                                  LoadCheckpoint3,
                                                  ResetCheckpoints);

        // Initializing CheckpointDisplay
        displayObject = new GameObject("CheckpointDisplay");
        checkpointDisplay = displayObject.AddComponent<CheckpointDisplay>();
        checkpointDisplay.Initialize(Logger, checkpointManager, DisplayCheckpoints);

        // Initializing SpeedManager
        speedObject = new GameObject("SpeedManager");
        speedManager = displayObject.AddComponent<SpeedManager>();
        speedManager.Initialize(Logger, DisplaySpeed, DisplayBar);
    }
}
