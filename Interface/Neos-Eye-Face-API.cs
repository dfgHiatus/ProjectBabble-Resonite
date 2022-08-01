using HarmonyLib;
using NeosModLoader;
using FrooxEngine;
using BaseX;
using System;

namespace Neos_EyeFace_API
{
	public class Neos_EyeFace_API : NeosMod
	{
		public override string Name => "Neos_EyeFace_API";
		public override string Author => "dfgHiatus";
		public override string Version => "1.1.1";
		public override string Link => "https://github.com/dfgHiatus/Neos-Eye-Face-API/";
		public override void OnEngineInit()
		{
			// Harmony.DEBUG = true;
			new Harmony("net.dfgHiatus.Neos-EyeFace-API").PatchAll();
			Engine.Current.OnShutdown += () => Shutdown();
        }

		[HarmonyPatch(typeof(InputInterface), MethodType.Constructor)]
		[HarmonyPatch(new[] { typeof(Engine)})]
		public class InputInterfaceCtorPatch
		{
			public static void Postfix(InputInterface __instance)
			{
				try
				{
					GenericInputDevice gen = new GenericInputDevice();
                    __instance.RegisterInputDriver(gen);
				}
				catch (Exception e)
				{
					Warn("Module failed to initiallize.");
					Warn(e.ToString());
				}
			}
		}
		private void Shutdown()
		{
			// Shutdown code goes here
			// ...
			// ...
		}
		public class GenericInputDevice : IInputDriver
		{
			private Eyes _eyes;
			private Mouth _mouth;
			public int UpdateOrder => 100;

			public void CollectDeviceInfos(DataTreeList list)
			{
				DataTreeDictionary eyeDataTreeDictionary = new DataTreeDictionary();
				eyeDataTreeDictionary.Add("Name", "Generic Eye Tracking");
				eyeDataTreeDictionary.Add("Type", "Eye Tracking");
				eyeDataTreeDictionary.Add("Model", "Generic Eye Model");
				list.Add(eyeDataTreeDictionary);

				DataTreeDictionary mouthDataTreeDictionary = new DataTreeDictionary();
				mouthDataTreeDictionary.Add("Name", "Generic Face Tracking");
				mouthDataTreeDictionary.Add("Type", "Face Tracking");
				mouthDataTreeDictionary.Add("Model", "Generic Face Model");
				list.Add(mouthDataTreeDictionary);
			}

			public void RegisterInputs(InputInterface inputInterface)
			{
				_eyes = new Eyes(inputInterface, "Generic Eye Tracking");
				_mouth = new Mouth(inputInterface, "Generic Mouth Tracking");
			}

			public void UpdateInputs(float deltaTime)
			{
				UpdateEyes(deltaTime);
				UpdateMouth(deltaTime);
			}

			private void UpdateEyes(float deltaTime)
			{
				_eyes.IsEyeTrackingActive = _eyes.IsEyeTrackingActive;

				UpdateEye(float3.Zero, float3.Zero, true, 0.003f,
					1f, 0f, 0f, 0f, deltaTime, _eyes.LeftEye);
				UpdateEye(float3.Zero, float3.Zero, true, 0.003f,
					1f, 0f, 0f, 0f, deltaTime, _eyes.RightEye);

				UpdateEye(float3.Zero, float3.Zero, true, 0.003f,
					1f, 0f, 0f, 0f, deltaTime, _eyes.CombinedEye);
				_eyes.ComputeCombinedEyeParameters();

				_eyes.ConvergenceDistance = 0f;
				_eyes.Timestamp += deltaTime;
				_eyes.FinishUpdate();
			}
			private void UpdateEye(float3 gazeDirection, float3 gazeOrigin, bool status, float pupilSize, float openness,
				float widen, float squeeze, float frown, float deltaTime, Eye eye)
			{
				eye.IsDeviceActive = Engine.Current.InputInterface.VR_Active;
				eye.IsTracking = status;

				if (eye.IsTracking)
				{
					eye.UpdateWithDirection(gazeDirection);
					eye.RawPosition = gazeOrigin;
					eye.PupilDiameter = pupilSize;
				}

				eye.Openness = openness;
				eye.Widen = widen;
				eye.Squeeze = squeeze;
				eye.Frown = frown;
			}

			private void UpdateMouth(float deltaTime)
			{
				_mouth.IsDeviceActive = true;

				_mouth.IsTracking = true;

				_mouth.Jaw = float3.Zero;
				_mouth.Tongue = float3.Zero;

				_mouth.JawOpen = 0f;
				_mouth.MouthPout = 0f;
				_mouth.TongueRoll = 0f;

				_mouth.LipBottomOverUnder = 0f;
				_mouth.LipBottomOverturn = 0f;
				_mouth.LipTopOverUnder = 0f;
				_mouth.LipTopOverturn = 0f;

				_mouth.LipLowerHorizontal = 0f;
				_mouth.LipUpperHorizontal = 0f;

				_mouth.LipLowerLeftRaise = 0f;
				_mouth.LipLowerRightRaise = 0f;
				_mouth.LipUpperRightRaise = 0f;
				_mouth.LipUpperLeftRaise = 0f;

				_mouth.MouthRightSmileFrown = 0f;
				_mouth.MouthLeftSmileFrown = 0f;
				_mouth.CheekLeftPuffSuck = 0f;
				_mouth.CheekRightPuffSuck = 0f;
			}
		}
	}
}
