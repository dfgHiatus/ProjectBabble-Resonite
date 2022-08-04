# ProjectBabble-Neos

A [NeosModLoader](https://github.com/zkxs/NeosModLoader) mod for [Neos VR](https://neos.com/) that enables the use of [Babble Face Tracking](https://github.com/SummerSigh/ProjectBabble).

## Usage
1. Install [NeosModLoader](https://github.com/zkxs/NeosModLoader).
1. Place [ProjectBabble-Neos.dll](https://github.com/dfgHiatus/ProjectBabble-Neos/releases) into your `nml_mods` folder. This folder should be at `C:\Program Files (x86)\Steam\steamapps\common\NeosVR\nml_mods` for a default install. You can create it if it's missing, or if you launch the game once with NeosModLoader installed it will create the folder for you.
1. Place [Rug-OSC.dll](https://github.com/dfgHiatus/ProjectBabble-Neos/releases) into your Neos base folder, one above your 'nml_mods' folder. This folder should be at `C:\Program Files (x86)\Steam\steamapps\common\NeosVR` for a default install.
1. Load and run Project Babble's face tracking model before starting Neos.
1. Start the game!

If you want to verify that the mod is working you can check your Neos logs, or create an EmptyObject with an AvatarRawMouthData Component (Found under Users -> Common Avatar System -> Face -> AvatarRawEyeData/AvatarRawMouthData).

# Libraries used:
- [Rug-OSC](https://bitbucket.org/rugcode/rug.osc/) - MIT License
