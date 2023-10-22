using System;
using Elements.Core;
using FrooxEngine;
using HarmonyLib;
using ResoniteModLoader;

namespace ProjectBabbleNeos
{
    public class BabbleNeos : ResoniteMod
    {
        public override string Name => "ProjectBabble-Neos";
        public override string Author => "dfgHiatus, PLYSHKA";
        public override string Version => "3.0.0";
        public override string Link => "https://github.com/dfgHiatus/Neos-Eye-Face-API/";
        public override void OnEngineInit()
        {
            Config = GetConfiguration();
            Engine.Current.OnShutdown += () => BOSC.Teardown();
            new Harmony("net.dfgHiatus.ProjectBabble-Neos").PatchAll();
        }

        internal static BabbleOSC BOSC;
        internal static ModConfiguration Config;

        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<bool> Enabled = new ModConfigurationKey<bool>("enabled", "Enabled", () => true);
        [AutoRegisterConfigKey]
        internal static ModConfigurationKey<int> OscPort = new ModConfigurationKey<int>("osc_port", "Babble OSC Port (Requires restart)", () => 8888);

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
                    Error("Module failed to initialize.");
                    Error(e.ToString());
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
                if (!Config.GetValue(Enabled))
                {
                    _mouth.IsTracking = false;
                    return;
                }

                _mouth.IsTracking = true;
                _mouth.Jaw = new float3(
                    BOSC.MouthShapesWithAddress["/jawRight"] - BOSC.MouthShapesWithAddress["/jawLeft"],
                    BOSC.MouthShapesWithAddress["/mouthClose"] * -1,
                    BOSC.MouthShapesWithAddress["/jawForward"]);
                _mouth.Tongue = new float3(
                    BOSC.MouthShapesWithAddress["/tongueRight"] - BOSC.MouthShapesWithAddress["/tongueLeft"],
                    BOSC.MouthShapesWithAddress["/tongueUp"] - BOSC.MouthShapesWithAddress["/tongueDown"],
                    BOSC.MouthShapesWithAddress["/tongueOut"]);
                _mouth.JawOpen = BOSC.MouthShapesWithAddress["/jawOpen"] - BOSC.MouthShapesWithAddress["/mouthClose"];
                _mouth.MouthPout = BOSC.MouthShapesWithAddress["/mouthPucker"];
                _mouth.TongueRoll = BOSC.MouthShapesWithAddress["/tongueRoll"];

                _mouth.LipBottomOverUnder = BOSC.MouthShapesWithAddress["/mouthRollLower"] * -1;
                _mouth.LipBottomOverturn = 0f;
                _mouth.LipTopOverUnder = BOSC.MouthShapesWithAddress["/mouthRollUpper"] * -1;
                _mouth.LipTopOverturn = 0f;

                _mouth.LipLowerHorizontal = BOSC.MouthShapesWithAddress["/mouthStretchRight"] - BOSC.MouthShapesWithAddress["/mouthStretchLeft"];
                _mouth.LipUpperHorizontal = BOSC.MouthShapesWithAddress["/mouthDimpleRight"] - BOSC.MouthShapesWithAddress["/mouthDimpleLeft"];

                _mouth.LipLowerLeftRaise = BOSC.MouthShapesWithAddress["/mouthLowerDownLeft"];
                _mouth.LipLowerRightRaise = BOSC.MouthShapesWithAddress["/mouthLowerDownRight"];
                _mouth.LipUpperRightRaise = BOSC.MouthShapesWithAddress["/mouthUpperUpRight"];
                _mouth.LipUpperLeftRaise = BOSC.MouthShapesWithAddress["/mouthUpperUpLeft"];

                _mouth.MouthLeftSmileFrown = BOSC.MouthShapesWithAddress["/mouthSmileLeft"] - BOSC.MouthShapesWithAddress["/mouthFrownLeft"];
                _mouth.MouthRightSmileFrown = BOSC.MouthShapesWithAddress["/mouthSmileRight"] - BOSC.MouthShapesWithAddress["/mouthFrownRight"];
                _mouth.CheekLeftPuffSuck = BOSC.MouthShapesWithAddress["/cheekPuffLeft"] - BOSC.MouthShapesWithAddress["/cheekSuckLeft"];
                _mouth.CheekRightPuffSuck = BOSC.MouthShapesWithAddress["/cheekPuffRight"] - BOSC.MouthShapesWithAddress["/cheekSuckRight"];
            }
        }
    }
}