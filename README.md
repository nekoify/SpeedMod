# Lorn's Lure SpeedMod
A helper for Lorn's Lure speedruns, using [BepinEx](https://github.com/BepInEx/BepInEx).

## Features
**These are the current features available in version 1.0.0 of SpeedMod:**

### Speed display

The current velocity of the player can be displayed with 2 main elements:
- **Speed Bar**: An orange bar at the top of the screen that expands according to the player's current velocity;
- **Speed Text**: The floating value of the player's current velocity (from 0 to 42);

Whether these elements are displayed can be configured in the BepinEx Configuration.

### Manual checkpoints

A total of **3 manual checkpoints** can be set by the player, by default:
- `F2`, `F3` and `F4` will set the first, second and third checkpoint;
- `2`, `3` and `4` will load the first, second and third checkpoint;
- `Z` will reset the 3 checkpoints.

These key bindings can be configured in the BepinEx Configuration.


## BepinEx Configuration

This mod uses the BepinEx Configuration module. This allows the user to change settings
by pressing `F1` (by default). Available settings include:
- Presence of the **Speed Bar**;
- Presence of the **Speed Text**;
- Key bindings related to setting, loading and resetting checkpoints.

## Installation

In order to use this mod, you first need to [install BepinEx](https://docs.bepinex.dev/articles/user_guide/installation/index.html) in your Lorn's Lure install path.
There are 2 methods to install the mod from there:

### Quick way

You can simply copy the `SpeedMod.dll` inside the `YOUR_GAME_DIR/BepinEx/plugins/` folder, and run the game!

### Build from source
You can also build the project yourself by cloning the repository, and building with dontnet:
```powershell
dotnet build -p:INSTALL_PATH="C:\Path\To\LornsLureDirectory"
```

## Notes

This is my first mod ever, if you have either tips, feature requests or other demands don't hesitate to contact me
in the Lorn's Lure discord!
