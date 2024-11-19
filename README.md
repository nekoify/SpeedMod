<div align="center">
  <img src="https://github.com/user-attachments/assets/ac3598f0-f694-47f6-87c0-e4c47d73d452" alt="SpeedMod" width="250" />
  <h1>Lorn's Lure SpeedMod</h1>
  A helper for Lorn's Lure speedruns, using <a href="https://github.com/LavaGang/MelonLoader">MelonLoader</a>.
</div>

## Video Demo

[![SpeedMod Demo](https://img.youtube.com/vi/-zkYbQj1EGE/0.jpg)](https://www.youtube.com/watch?v=-zkYbQj1EGE "SpeedMod Demo")

**These are the current features available in version 1.0.0 of SpeedMod:**

## Speed display

The current velocity of the player can be displayed with 2 main elements:
- **Speed Bar**: An orange bar at the top of the screen that expands according to the player's current velocity;
- **Speed Text**: The floating value of the player's current velocity (from 0 to 42);

## Manual checkpoints

Unlimited checkpoints can be set by the player, by default:
- `1` will create a checkpoint;
- `2` will load the checkpoint;
- `3` will remove the current checkpoint, allowing you to go back to the previous checkpoint;
- `Z` will reset the 3 checkpoints.

# Installation

In order to use this mod, you first need to [install MelonLoader](https://github.com/LavaGang/MelonLoader.Installer/blob/master/README.md#how-to-install-re-install-or-update-melonloader).

There are 2 methods to install the mod from here:  

### Quick way

You can simply copy the `SpeedMod.dll` from the Releases page inside the `YOUR_GAME_DIR/Mods/` folder, and run the game!  

### Build from source
You can also build the project yourself by cloning the repository, and building with dotnet:  
```powershell
dotnet build
```

### Troubleshooting

For the BepinEx release, if the mod doesn't start, you can set `HideManagerGameObject = true` in the `BepinEx/config/BepinEx.cfg` file.

## Notes

This is my first mod ever, if you have either tips, feature requests or other demands don't hesitate to contact me
in the Lorn's Lure discord!  
