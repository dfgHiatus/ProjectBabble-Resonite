# Neos-Eye-Face-API

A [NeosModLoader](https://github.com/zkxs/NeosModLoader) mod for [Neos VR](https://neos.com/)  
Enables developers to add implement their own eye and face tracking solutions for NeosVR.

Related issue on the Neos Github:
https://github.com/Neos-Metaverse/NeosPublic/issues/1140

## Usage
1. Install [NeosModLoader](https://github.com/zkxs/NeosModLoader).
1. Clone this Repository
1. Update `_eyes` and `_mouth` with values of your own.
1. Build and place your DLL under "nml_mods". This folder should be at `C:\Program Files (x86)\Steam\steamapps\common\NeosVR\nml_mods` for a default install. You can create it if it's missing, or if you launch the game once with NeosModLoader.
1. Start the game!

If you want to verify that the mod is working you can check your Neos logs, or create an EmptyObject with an AvatarRawEyeData/AvatarRawMouthData Component (Found under Users -> Common Avatar System -> Face -> AvatarRawEyeData/AvatarRawMouthData).

Thanks to those who helped me test this!
