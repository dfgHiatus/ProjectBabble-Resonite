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
                Console.WriteLine("BabbleOSC connection already exists.");
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
                Console.WriteLine("BabbleOSC connection already exists.");
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
                MouthShapes.Add("/" + shape, 0f);
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
                                Console.WriteLine($"Key recognized {message.Address}");
                                if (float.TryParse(message[0].ToString(), out candidate)) {
                                    MouthShapes[message.Address] = candidate;
                                    Console.WriteLine($"{message.Address}: {candidate}");
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    if (_receiver.State == OscSocketState.Connected)
                        Console.WriteLine(e.Message);
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
            "cheekPuff",
            "cheekSquintLeft",
            "cheekSquintRight",
            "noseSneerLeft",
            "noseSneerRight",
            "jawOpen",
            "jawForward",
            "jawLeft",
            "jawRight",
            "mouthFunnel",
            "mouthPucker",
            "mouthLeft",
            "mouthRight",
            "mouthRollUpper",
            "mouthRollLower",
            "mouthShrugUpper",
            "mouthShrugLower",
            "mouthClose",
            "mouthSmileLeft",
            "mouthSmileRight",
            "mouthFrownLeft",
            "mouthFrownRight",
            "mouthDimpleLeft",
            "mouthDimpleRight",
            "mouthUpperUpLeft",
            "mouthUpperUpRight",
            "mouthLowerDownLeft",
            "mouthLowerDownRight",
            "mouthPressLeft",
            "mouthPressRight",
            "mouthStretchLeft",
            "mouthStretchRight",
            "tongueOut",
        };
    }
}
