# ProjectBabble-Resonite

A [ResoniteModLoader](https://github.com/zkxs/ResoniteModLoader) mod for [Resonite](https://Resonite.com/) that enables the use of [Babble Face Tracking](https://github.com/SummerSigh/ProjectBabble).

## Usage
1. Install [ResoniteModLoader](https://github.com/zkxs/ResoniteModLoader).
1. Place [ProjectBabbleResonite.dll](https://github.com/dfgHiatus/ProjectBabble-Resonite/releases/latest) into your `rml_mods` folder. This folder should be at `C:\Program Files (x86)\Steam\steamapps\common\Resonite\rml_mods` on windows or `$HOME/.steam/steam/steamapps/common/Resonite/rml_mods` on linux for a default installation. You can create it if it's missing, or if you launch the game once with ResoniteModLoader installed it will create the folder for you.
1. Place [OscCore.dll](https://github.com/dfgHiatus/ProjectBabble-Resonite/releases/latest) into your Resonite base folder, one above your 'rml_mods' folder. This folder should be at `C:\Program Files (x86)\Steam\steamapps\common\Resonite` on windows or `$HOME/.steam/steam/steamapps/common/Resonite` on linux for a default installation.
1. Load and run Project Babble's face tracking before starting Resonite. All defaults should be fine unless you have more than 1 camera.
1. Start the game!

Mod has controls to enable/disable it completely (no hooks on avatar values) or to enable/disable it just for VR in mod settings.

If you want to verify that the mod is working you can check your Resonite logs, or create an EmptyObject with an AvatarRawMouthData Component (Found under Users -> Common Avatar System -> Face -> AvatarRawEyeData/AvatarRawMouthData).

# Libraries used:
- [OSCCore](https://github.com/tilde-love/osc-core) - MIT License
