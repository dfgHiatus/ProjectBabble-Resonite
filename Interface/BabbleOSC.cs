using BaseX;
using Rug.Osc;
using System;
using System.Net;
using System.Threading;
using System.Collections.Generic;

namespace ProjectBabbleNeos
{
    public class BabbleOSC
    {
        public static Dictionary<string, float> MouthShapes = new Dictionary<string, float>();
        private static OscReceiver _receiver;
        private static Thread _thread;
        private const int DEFAULT_PORT = 9000;

        public BabbleOSC()
        {
            if (_receiver != null)
            {
                UniLog.Error("BabbleOSC connection already exists.");
                return;
            }

            IPAddress canidate;
            IPAddress.TryParse("127.0.0.1", out canidate);
            _receiver = new OscReceiver(canidate, DEFAULT_PORT);
            ConstructorHelper();
        }

        public BabbleOSC(int port)
        {
            if (_receiver != null)
            {
                UniLog.Error("BabbleOSC connection already exists.");
                return;
            }

            IPAddress canidate;
            IPAddress.TryParse("127.0.0.1", out canidate);
            _receiver = new OscReceiver(canidate, port);
            ConstructorHelper();
        }

        private void ConstructorHelper()
        {
            foreach (var shape in MouthShape)
            {
                MouthShapes.Add(shape, 0f);
            }
            _thread = new Thread(new ThreadStart(ListenLoop));
            _receiver.Connect();
            _thread.Start();
        }

        private static void ListenLoop()
        {
            OscPacket packet;
            OscMessage message;
            float candidate = 0;

            while (_receiver.State != OscSocketState.Closed) {
                try {
                    if (_receiver.State == OscSocketState.Connected) {
                        packet = _receiver.Receive();
                        if (OscMessage.TryParse(packet.ToString(), out message)) {
                            if (MouthShapes.ContainsKey(message.Address)) {
                                if (float.TryParse(message[0].ToString(), out candidate)) {
                                    MouthShapes[message.Address] = candidate;
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    if (_receiver.State == OscSocketState.Connected)
                        UniLog.Error(e.Message);
                }
            }
        }

        public void Teardown()
        {
            _receiver.Close();
            _thread.Join();
        }

        // Ideally this should be in its own class
        private static string[] MouthShape = new string[]
        {
            "/cheekPuff",
            "/cheekSquint_L",
            "/cheekSquint_R",
            "/noseSneer_L",
            "/noseSneer_R",
            "/jawOpen",
            "/jawForward",
            "/jawLeft",
            "/jawRight",
            "/mouthFunnel",
            "/mouthPucker",
            "/mouthLeft",
            "/mouthRight",
            "/mouthRollUpper",
            "/mouthRollLower",
            "/mouthShrugUpper",
            "/mouthShrugLower",
            "/mouthClose",
            "/mouthSmile_L",
            "/mouthSmile_R",
            "/mouthFrown_L",
            "/mouthFrown_R",
            "/mouthDimple_L",
            "/mouthDimple_R",
            "/mouthUpperUp_L",
            "/mouthUpperUp_R",
            "/mouthLowerDown_L",
            "/mouthLowerDown_R",
            "/mouthPress_L",
            "/mouthPress_R",
            "/mouthStretch_L",
            "/mouthStretch_R",
            "/tongueOut",
        };
    }
}
