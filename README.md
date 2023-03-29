# ProjectBabble-Neos

A [NeosModLoader](https://github.com/zkxs/NeosModLoader) mod for [Neos VR](https://neos.com/) that enables the use of [Babble Face Tracking](https://github.com/SummerSigh/ProjectBabble).

## Usage
1. Install [NeosModLoader](https://github.com/zkxs/NeosModLoader).
1. Place [ProjectBabble-Neos.dll](https://github.com/Meister1593/ProjectBabble-Neos/releases) into your `nml_mods` folder. This folder should be at `C:\Program Files (x86)\Steam\steamapps\common\NeosVR\nml_mods` for a default install. You can create it if it's missing, or if you launch the game once with NeosModLoader installed it will create the folder for you.
1. Place [Rug-OSC.dll](https://github.com/Meister1593/ProjectBabble-Neos/releases) into your Neos base folder, one above your 'nml_mods' folder. This folder should be at `C:\Program Files (x86)\Steam\steamapps\common\NeosVR` for a default install.
1. Load and run Project Babble's face tracking before starting Neos. All defaults should be fine unless you have more than 1 camera.
1. Start the game!

Mod has controls to enable/disable it completely (no hooks on avatar values) or to disable it just for VR in mod settings.

If you want to verify that the mod is working you can check your Neos logs, or create an EmptyObject with an AvatarRawMouthData Component (Found under Users -> Common Avatar System -> Face -> AvatarRawEyeData/AvatarRawMouthData).

# Libraries used:
- [Rug-OSC](https://bitbucket.org/rugcode/rug.osc/) - MIT License
