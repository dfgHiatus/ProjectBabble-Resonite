# ProjectBabbleResonite

A [ResoniteModLoader](https://github.com/resonite-modding-group/ResoniteModLoader) mod for [Resonite](https://resonite.com/) that enables the use of [Babble Face Tracking](https://github.com/SummerSigh/ProjectBabble).

## Usage
1. Install [ResoniteModLoader](https://github.com/resonite-modding-group/ResoniteModLoader).
2. Place [ProjectBabbleResonite.dll](https://github.com/Meister1593/ProjectBabbleResonite/releases) into your `rml_mods` folder. This folder should be at `C:\Program Files (x86)\Steam\steamapps\common\Resonite\rml_mods` on windows or `$HOME/.steam/steam/steamapps/common/Resonite/rml_mods` on linux for a default installation. You can create it if it's missing, or if you launch the game once with ResoniteModLoader installed it will create the folder for you.
3. Place [OscCore.dll](https://github.com/Meister1593/ProjectBabbleResonite/releases) into your Resonite base folder, one above your 'rml_mods' folder. This folder should be at `C:\Program Files (x86)\Steam\steamapps\common\Resonite` on windows or `$HOME/.steam/steam/steamapps/common/Resonite` on linux for a default installation.
4. Load and run Project Babble's face tracking before starting Resonite.
5. Start the game!

Mod has controls to enable/disable it completely (no hooks on avatar values) or to enable/disable it just for VR in mod settings.

If you want to verify that the mod is working you can check your Resonite logs, or create an EmptyObject with an AvatarRawMouthData Component (Found under Users -> Common Avatar System -> Face -> AvatarRawEyeData/AvatarRawMouthData).

# Libraries used:
- [OscCore](https://github.com/tilde-love/osc-core) - MIT License
