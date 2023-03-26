using BaseX;
using FrooxEngine;
using HarmonyLib;
using NeosModLoader;

namespace ProjectBabbleNeos
{
    public class BabbleNeos : NeosMod
	{
		public override string Name => "ProjectBabble-Neos";
		public override string Author => "dfgHiatus";
		public override string Version => "1.0.1";
		public override string Link => "https://github.com/dfgHiatus/Neos-Eye-Face-API/";

		private static BabbleOSC _bosc;
		private static ModConfiguration _config;
		
		[AutoRegisterConfigKey]
		private static ModConfigurationKey<bool> enableMod = 
			new ModConfigurationKey<bool>("enableMod", "Enable mod", () => true);

		[AutoRegisterConfigKey]
		private static ModConfigurationKey<bool> vrOnly = 
			new ModConfigurationKey<bool>("vrOnly", "Only run face tracking in VR Mode", () => true);

		public override void OnEngineInit()
		{
			new Harmony("net.dfgHiatus.ProjectBabble-Neos").PatchAll();
			_config = GetConfiguration();
			if (!_config.GetValue(enableMod)) {
				return;
			}

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
					BabbleOSC.MouthShapesWithAddress["/jawRight"] - BabbleOSC.MouthShapesWithAddress["/jawLeft"],
					BabbleOSC.MouthShapesWithAddress["/jawOpen"], // + BabbleOSC.MouthShapesWithAddress["/mouthClose"] * -1,
					BabbleOSC.MouthShapesWithAddress["/jawForward"]);
				_mouth.Tongue = new float3(
					0f,
					0f,
					BabbleOSC.MouthShapesWithAddress["/tongueOut"]);

				_mouth.JawOpen = BabbleOSC.MouthShapesWithAddress["/jawOpen"];
				_mouth.MouthPout = BabbleOSC.MouthShapesWithAddress["/mouthFunnel"] - BabbleOSC.MouthShapesWithAddress["/mouthPucker"];
				_mouth.TongueRoll = 0f;

				_mouth.LipBottomOverUnder = BabbleOSC.MouthShapesWithAddress["/mouthRollLower"] * 1;
				_mouth.LipBottomOverturn = 0f;
				_mouth.LipTopOverUnder = BabbleOSC.MouthShapesWithAddress["/mouthRollUpper"] * 1;
				_mouth.LipTopOverturn = 0f;

				// Assuming a tug face like this? => 0_0
				_mouth.LipLowerHorizontal = BabbleOSC.MouthShapesWithAddress["/mouthStretchLeft"] - BabbleOSC.MouthShapesWithAddress["/mouthStretchRight"];
				_mouth.LipUpperHorizontal = BabbleOSC.MouthShapesWithAddress["/mouthDimpleLeft"] - BabbleOSC.MouthShapesWithAddress["/mouthDimpleRight"];

				_mouth.LipLowerLeftRaise = BabbleOSC.MouthShapesWithAddress["/mouthLowerDownLeft"];
				_mouth.LipLowerRightRaise = BabbleOSC.MouthShapesWithAddress["/mouthLowerDownRight"];
				_mouth.LipUpperRightRaise = BabbleOSC.MouthShapesWithAddress["/mouthUpperUpRight"];
				_mouth.LipUpperLeftRaise = BabbleOSC.MouthShapesWithAddress["/mouthUpperUpLeft"];

				_mouth.MouthRightSmileFrown = BabbleOSC.MouthShapesWithAddress["/mouthSmileLeft"] - BabbleOSC.MouthShapesWithAddress["/mouthFrownLeft"];
				_mouth.MouthLeftSmileFrown = BabbleOSC.MouthShapesWithAddress["/mouthSmileRight"] - BabbleOSC.MouthShapesWithAddress["/mouthFrownRight"];
				_mouth.CheekLeftPuffSuck = BabbleOSC.MouthShapesWithAddress["/cheekPuff"];
				_mouth.CheekRightPuffSuck = BabbleOSC.MouthShapesWithAddress["/cheekPuff"];
			}
		}
	}
}
