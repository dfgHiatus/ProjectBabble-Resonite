# ProjectBabble-Neos

A [NeosModLoader](https://github.com/zkxs/NeosModLoader) mod for [Neos VR](https://neos.com/) that enables the use of [Babble Face Tracking](https://github.com/SummerSigh/ProjectBabble).

## Usage
1. Install [NeosModLoader](https://github.com/zkxs/NeosModLoader).
1. Place [ProjectBabbleNeos.dll](https://github.com/dfgHiatus/ProjectBabble-Neos/releases/latest) into your `nml_mods` folder. This folder should be at `C:\Program Files (x86)\Steam\steamapps\common\NeosVR\nml_mods` on windows or `$HOME/.steam/steam/steamapps/common/NeosVR/nml_mods` on linux for a default installation. You can create it if it's missing, or if you launch the game once with NeosModLoader installed it will create the folder for you.
1. Place [OscCore.dll](https://github.com/dfgHiatus/ProjectBabble-Neos/releases/latest) into your Neos base folder, one above your 'nml_mods' folder. This folder should be at `C:\Program Files (x86)\Steam\steamapps\common\NeosVR` on windows or `$HOME/.steam/steam/steamapps/common/NeosVR` on linux for a default installation.
1. Load and run Project Babble's face tracking before starting Neos. All defaults should be fine unless you have more than 1 camera.
1. Start the game!

Mod has controls to enable/disable it completely (no hooks on avatar values) or to enable/disable it just for VR in mod settings.

If you want to verify that the mod is working you can check your Neos logs, or create an EmptyObject with an AvatarRawMouthData Component (Found under Users -> Common Avatar System -> Face -> AvatarRawEyeData/AvatarRawMouthData).

# Libraries used:
- [OSCCore](https://github.com/tilde-love/osc-core) - MIT License
