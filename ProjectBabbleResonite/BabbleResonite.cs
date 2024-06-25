﻿using System;
using System.Collections.Generic;
using Elements.Core;
using FrooxEngine;
using HarmonyLib;
using ResoniteModLoader;

namespace ProjectBabbleResonite;

public class BabbleResonite : ResoniteMod
{
    public override string Name => "ProjectBabbleResonite";
    public override string Author => "PLYSHKA + dfgHiatus";
    public override string Version => "2.2.0";

    public override string Link => "https://github.com/Meister1593/ProjectBabbleResonite";

    private static BabbleOsc _babbleOsc;
    private static ModConfiguration _config;

    public override void OnEngineInit()
    {
        _config = GetConfiguration();
        Engine.Current.OnShutdown += () => _babbleOsc.Teardown();
        new Harmony("net.plyshka.ProjectBabbleResonite").PatchAll();
    }

    [AutoRegisterConfigKey]
    private static readonly ModConfigurationKey<bool> ModEnabled = new("enabled", "Mod Enabled", () => true);

    [AutoRegisterConfigKey]
    private static readonly ModConfigurationKey<int> OscPort = new("osc_port", "Babble OSC port", () => 8888);

    [HarmonyPatch(typeof(InputInterface), MethodType.Constructor)]
    [HarmonyPatch(new[] { typeof(Engine) })]
    public class InputInterfaceCtorPatch
    {
        public static void Postfix(InputInterface __instance)
        {
            try
            {
                _babbleOsc = new BabbleOsc(_config.GetValue(OscPort));
                var gen = new ProjectBabbleInterface();
                __instance.RegisterInputDriver(gen);
            }
            catch (Exception e)
            {
                Warn("Module failed to initialize.");
                Warn(e.ToString());
            }
        }
    }

    private class ProjectBabbleInterface : IInputDriver
    {
        public int UpdateOrder => 100;
        private Mouth _mouth;

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
            var mouthParameterGroup = new HashSet<MouthParameterGroup>();
            mouthParameterGroup.UnionWith(new[]
            {
                MouthParameterGroup.JawPose,
                MouthParameterGroup.JawOpen,
                MouthParameterGroup.TonguePose,
                MouthParameterGroup.TongueRoll,
                MouthParameterGroup.LipRaise,
                MouthParameterGroup.LipHorizontal,
                MouthParameterGroup.SmileFrown,
                MouthParameterGroup.MouthDimple,
                MouthParameterGroup.MouthPout,
                MouthParameterGroup.LipOverturn,
                MouthParameterGroup.LipOverUnder,
                MouthParameterGroup.LipStretchTighten,
                MouthParameterGroup.LipsPress,
                MouthParameterGroup.CheekPuffSuck,
                MouthParameterGroup.CheekRaise,
                MouthParameterGroup.ChinRaise,
                MouthParameterGroup.NoseWrinkle
            });
            _mouth = new Mouth(inputInterface, "Project Babble Mouth Tracking", mouthParameterGroup);
        }

        public void UpdateInputs(float deltaTime)
        {
            if (!_config.GetValue(ModEnabled))
            {
                _mouth.IsTracking = false;
                return;
            }

            _mouth.IsTracking = true;

            // Assuming x is left/right, y is up/down, z is forward/backwards
            _mouth.Jaw = new float3(
                BabbleOsc.MouthShapesWithAddress["/jawRight"] - BabbleOsc.MouthShapesWithAddress["/jawLeft"],
                BabbleOsc.MouthShapesWithAddress["/mouthClose"] * -1,
                BabbleOsc.MouthShapesWithAddress["/jawForward"]);
            _mouth.Tongue = new float3(
                BabbleOsc.MouthShapesWithAddress["/tongueRight"] - BabbleOsc.MouthShapesWithAddress["/tongueLeft"],
                BabbleOsc.MouthShapesWithAddress["/tongueUp"] - BabbleOsc.MouthShapesWithAddress["/tongueDown"],
                BabbleOsc.MouthShapesWithAddress["/tongueOut"]);
            _mouth.JawOpen = BabbleOsc.MouthShapesWithAddress["/jawOpen"] -
                             BabbleOsc.MouthShapesWithAddress["/mouthClose"];
            var pucker = BabbleOsc.MouthShapesWithAddress["/mouthPucker"];
            _mouth.LipsLeftPress = pucker;
            _mouth.LipsRightPress = pucker;
            _mouth.TongueRoll = BabbleOsc.MouthShapesWithAddress["/tongueRoll"];

            var rollLower = BabbleOsc.MouthShapesWithAddress["/mouthRollLower"] * -1;
            _mouth.LipBottomLeftOverUnder = rollLower;
            _mouth.LipBottomRightOverUnder = rollLower;

            var rollUpper = BabbleOsc.MouthShapesWithAddress["/mouthRollUpper"] * -1;
            _mouth.LipTopLeftOverUnder = rollUpper;
            _mouth.LipTopRightOverUnder = rollUpper;

            _mouth.LipLowerHorizontal = BabbleOsc.MouthShapesWithAddress["/mouthStretchRight"] -
                                        BabbleOsc.MouthShapesWithAddress["/mouthStretchLeft"];
            _mouth.LipUpperHorizontal = BabbleOsc.MouthShapesWithAddress["/mouthDimpleRight"] -
                                        BabbleOsc.MouthShapesWithAddress["/mouthDimpleLeft"];

            _mouth.LipLowerLeftRaise = BabbleOsc.MouthShapesWithAddress["/mouthLowerDownLeft"];
            _mouth.LipLowerRightRaise = BabbleOsc.MouthShapesWithAddress["/mouthLowerDownRight"];
            _mouth.LipUpperRightRaise = BabbleOsc.MouthShapesWithAddress["/mouthUpperUpRight"];
            _mouth.LipUpperLeftRaise = BabbleOsc.MouthShapesWithAddress["/mouthUpperUpLeft"];

            _mouth.MouthLeftSmileFrown = BabbleOsc.MouthShapesWithAddress["/mouthSmileLeft"] -
                                         BabbleOsc.MouthShapesWithAddress["/mouthFrownLeft"];
            _mouth.MouthRightSmileFrown = BabbleOsc.MouthShapesWithAddress["/mouthSmileRight"] -
                                          BabbleOsc.MouthShapesWithAddress["/mouthFrownRight"];
            _mouth.CheekLeftPuffSuck = BabbleOsc.MouthShapesWithAddress["/cheekPuffLeft"] -
                                       BabbleOsc.MouthShapesWithAddress["/cheekSuckLeft"];
            _mouth.CheekRightPuffSuck = BabbleOsc.MouthShapesWithAddress["/cheekPuffRight"] -
                                        BabbleOsc.MouthShapesWithAddress["/cheekSuckRight"];
        }
    }
}