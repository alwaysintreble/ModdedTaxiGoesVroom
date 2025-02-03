# Chauffeur

Minimal modding framework for Yellow Taxi Goes Vroom.

## Features

* A custom logger that can be modified via config file to change the amount of saved log files and the log level.
  Default history of 5 files, and only writes user relevant messages.
* In game menu modifications allowing any mod to hook into and add entries to the main and pause menus, and add their
  own sub menus.
* Reflection utilities.
* In game mod menu that shows all currently installed mods

For mods hoping to use this library, [it can be found on nuget](https://www.nuget.org/packages/com.alwaysintreble.Chauffeur/),
and some examples using it:
* [TaxiTrainer](https://github.com/alwaysintreble/TaxiTrainer)
* [TaxiAssetManager](https://github.com/alwaysintreble/TaxiAssetManager)

## Installation

1. Install the latest version of [BepInEx 5](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.23.2).
    * Depending on your system, `get BepInEx_<system>_x64.zip`
    * Extract the contents to your game folder
2. Launch and close the game so that BepInEx can do its initial setup.
3. Install the latest version of [MonoMod HookGenPatcher](https://github.com/harbingerofme/Bepinex.Monomod.HookGenPatcher/releases).
    * Download the `Release.zip`
    * From your game folder go to `/BepInEx/patchers/` and extract everything.
4. Install the latest version of [Chauffeur](https://github.com/alwaysintreble/Chauffeur/releases/latest).
    * Download the `Chauffeur.dll`
    * From your game folder go to `/BepInEx/plugins/` and place the dll there.
