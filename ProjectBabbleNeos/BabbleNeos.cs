using System;
using BaseX;
using FrooxEngine;
using HarmonyLib;
using NeosModLoader;

namespace ProjectBabbleNeos
{
    public class BabbleNeos : NeosMod
    {
        public override string Name => "ProjectBabble-Neos";
        public override string Author => "dfgHiatus + PLYSHKA";
        public override string Version => "1.0.2";
        public override string Link => "https://github.com/dfgHiatus/Neos-Eye-Face-API/";
        public override void OnEngineInit()
        {
            Config = GetConfiguration();
            new Harmony("net.dfgHiatus.plyshka.ProjectBabble-Neos").PatchAll();
            Engine.Current.OnShutdown += () => BOSC.Teardown();
        }
        private static BabbleOSC BOSC;
        private static ModConfiguration Config;

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> ModEnabled = new ModConfigurationKey<bool>("enabled", "Mod Enabled", () => true);

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<int> OscPort = new ModConfigurationKey<int>("osc_port", "Babble OSC port", () => 8888);

        [HarmonyPatch(typeof(InputInterface), MethodType.Constructor)]
        [HarmonyPatch(new[] { typeof(Engine) })]
        public class InputInterfaceCtorPatch
        {
            public static void Postfix(InputInterface __instance)
            {
                try
                {
                    BOSC = new BabbleOSC(Config.GetValue(OscPort));
                    ProjectBabbleInterface gen = new ProjectBabbleInterface();
                    __instance.RegisterInputDriver(gen);
                }
                catch (Exception e)
                {
                    Warn("Module failed to initiallize.");
                    Warn(e.ToString());
                }
            }
        }

        public class ProjectBabbleInterface : IInputDriver
        {
            public int UpdateOrder => 100;
            public Mouth _mouth;

            public void CollectDeviceInfos(DataTreeList list)
            {
                var mouthDataTreeDictionary = new DataTreeDictionary();
                mouthDataTreeDictionary.Add("Name", "Project Babble Face Tracking");
                mouthDataTreeDictionary.Add("Type", "Lip Tracking");
                mouthDataTreeDictionary.Add("Model", "Project Babble Model");
                list.Add(mouthDataTreeDictionary);
            }

            public void RegisterInputs(InputInterface inputInterface)
            {
                _mouth = new Mouth(inputInterface, "Project Babble Mouth Tracking");
            }

            public void UpdateInputs(float deltaTime)
            {
                if (!Config.GetValue(ModEnabled))
                {
                    _mouth.IsTracking = false;
                    return;
                }
                else
                {
                    _mouth.IsTracking = Engine.Current.InputInterface.VR_Active || Engine.Current.InputInterface.ScreenActive;
                }

                // Assuming x is left/right, y is up/down, z is forward/backwards
                _mouth.Jaw = new float3(
                    BabbleOSC.MouthShapesWithAddress["/jawRight"] - BabbleOSC.MouthShapesWithAddress["/jawLeft"],
                    BabbleOSC.MouthShapesWithAddress["/mouthClose"] * -1,
                    BabbleOSC.MouthShapesWithAddress["/jawForward"]);
                _mouth.Tongue = new float3(
                    BabbleOSC.MouthShapesWithAddress["/tongueRight"] - BabbleOSC.MouthShapesWithAddress["/tongueLeft"],
                    BabbleOSC.MouthShapesWithAddress["/tongueUp"] - BabbleOSC.MouthShapesWithAddress["/tongueDown"],
                    BabbleOSC.MouthShapesWithAddress["/tongueOut"]);
                _mouth.JawOpen = BabbleOSC.MouthShapesWithAddress["/jawOpen"] - BabbleOSC.MouthShapesWithAddress["/mouthClose"];
                _mouth.MouthPout = BabbleOSC.MouthShapesWithAddress["/mouthPucker"];
                _mouth.TongueRoll = BabbleOSC.MouthShapesWithAddress["/tongueRoll"];

                _mouth.LipBottomOverUnder = BabbleOSC.MouthShapesWithAddress["/mouthRollLower"] * -1;
                _mouth.LipBottomOverturn = 0f;
                _mouth.LipTopOverUnder = BabbleOSC.MouthShapesWithAddress["/mouthRollUpper"] * -1;
                _mouth.LipTopOverturn = 0f;

                // Assuming a tug face like this? => 0_0
                _mouth.LipLowerHorizontal = BabbleOSC.MouthShapesWithAddress["/mouthStretchRight"] - BabbleOSC.MouthShapesWithAddress["/mouthStretchLeft"];
                _mouth.LipUpperHorizontal = BabbleOSC.MouthShapesWithAddress["/mouthDimpleRight"] - BabbleOSC.MouthShapesWithAddress["/mouthDimpleLeft"];

                _mouth.LipLowerLeftRaise = BabbleOSC.MouthShapesWithAddress["/mouthLowerDownLeft"];
                _mouth.LipLowerRightRaise = BabbleOSC.MouthShapesWithAddress["/mouthLowerDownRight"];
                _mouth.LipUpperRightRaise = BabbleOSC.MouthShapesWithAddress["/mouthUpperUpRight"];
                _mouth.LipUpperLeftRaise = BabbleOSC.MouthShapesWithAddress["/mouthUpperUpLeft"];

                _mouth.MouthLeftSmileFrown = BabbleOSC.MouthShapesWithAddress["/mouthSmileLeft"] - BabbleOSC.MouthShapesWithAddress["/mouthFrownLeft"];
                _mouth.MouthRightSmileFrown = BabbleOSC.MouthShapesWithAddress["/mouthSmileRight"] - BabbleOSC.MouthShapesWithAddress["/mouthFrownRight"];
                _mouth.CheekLeftPuffSuck = BabbleOSC.MouthShapesWithAddress["/cheekPuffLeft"] - BabbleOSC.MouthShapesWithAddress["/cheekSuckLeft"];
                _mouth.CheekRightPuffSuck = BabbleOSC.MouthShapesWithAddress["/cheekPuffRight"] - BabbleOSC.MouthShapesWithAddress["/cheekSuckRight"];
            }
        }
    }
}
