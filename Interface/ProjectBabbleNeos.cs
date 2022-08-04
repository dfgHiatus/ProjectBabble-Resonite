using BaseX;
using FrooxEngine;
using HarmonyLib;
using NeosModLoader;
using System;

namespace ProjectBabbleNeos
{
    public class ProjectBabbleNeos : NeosMod
	{
		public override string Name => "ProjectBabble-Neos";
		public override string Author => "dfgHiatus";
		public override string Version => "1.0.0";
		public override string Link => "https://github.com/dfgHiatus/Neos-Eye-Face-API/";

		private static BabbleOSC _bosc;
		private static ModConfiguration _config;

		[AutoRegisterConfigKey]
		private static ModConfigurationKey<bool> vrOnly = 
			new ModConfigurationKey<bool>("vrOnly", "Only run face tracking in VR Mode", () => true);

		public override void OnEngineInit()
		{
			new Harmony("net.dfgHiatus.ProjectBabble-Neos").PatchAll();
			_config = GetConfiguration();

			_bosc = new BabbleOSC();
			Engine.Current.OnReady += () =>
				Engine.Current.InputInterface.RegisterInputDriver(new ProjectBabbleDevice());
			Engine.Current.OnShutdown += () => _bosc.Teardown();
        }

		public class ProjectBabbleDevice : IInputDriver
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
				_mouth.IsTracking = _config.GetValue(vrOnly);

				// Assuming x is left/right, y is up/down, z is forward/backwards
				_mouth.Jaw = new float3(
					BabbleOSC.MouthShapes["/jawLeft"] - BabbleOSC.MouthShapes["/jawRight"],
					BabbleOSC.MouthShapes["/jawOpen"] + BabbleOSC.MouthShapes["/mouthClose"] * -1,
					BabbleOSC.MouthShapes["/jawForward"]);
				_mouth.Tongue = new float3(
					0f,
					0f,
					BabbleOSC.MouthShapes["/tongueOut"]);

				_mouth.JawOpen = BabbleOSC.MouthShapes["/jawOpen"];
				_mouth.MouthPout = BabbleOSC.MouthShapes["/mouthPucker"] - BabbleOSC.MouthShapes["/mouthFunnel"];
				_mouth.TongueRoll = 0f;

				_mouth.LipBottomOverUnder = BabbleOSC.MouthShapes["/mouthRollLower"] * -1;
				_mouth.LipBottomOverturn = 0f;
				_mouth.LipTopOverUnder = BabbleOSC.MouthShapes["/mouthRollUpper"] * -1;
				_mouth.LipTopOverturn = 0f;

				// Assuming a tug face like this? => 0_0
				_mouth.LipLowerHorizontal = BabbleOSC.MouthShapes["/mouthStretch_L"] - BabbleOSC.MouthShapes["/mouthStretch_R"];
				_mouth.LipUpperHorizontal = BabbleOSC.MouthShapes["/mouthDimple_L"] - BabbleOSC.MouthShapes["/mouthDimple_R"];

				_mouth.LipLowerLeftRaise = BabbleOSC.MouthShapes["/mouthLowerDown_L"];
				_mouth.LipLowerRightRaise = BabbleOSC.MouthShapes["/mouthLowerDown_R"];
				_mouth.LipUpperRightRaise = BabbleOSC.MouthShapes["/mouthUpperUp_R"];
				_mouth.LipUpperLeftRaise = BabbleOSC.MouthShapes["/mouthUpperUp_L"];

				_mouth.MouthRightSmileFrown = BabbleOSC.MouthShapes["/mouthSmile_L"] - BabbleOSC.MouthShapes["/mouthFrown_L"];
				_mouth.MouthLeftSmileFrown = BabbleOSC.MouthShapes["/mouthSmile_R"] - BabbleOSC.MouthShapes["/mouthFrown_R"];
				_mouth.CheekLeftPuffSuck = BabbleOSC.MouthShapes["/cheekPuff"];
				_mouth.CheekRightPuffSuck = BabbleOSC.MouthShapes["/cheekPuff"];
			}

		}
	}
}
