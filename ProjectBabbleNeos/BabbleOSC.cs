using BaseX;
using Rug.Osc;
using System;
using System.Net;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectBabbleNeos
{
    public class BabbleOSC
    {
        public static Dictionary<string, float> MouthShapesWithAddress = new Dictionary<string, float>();
        private static OscReceiver? _receiver;
        private static Thread? _thread;
        private const int DEFAULT_PORT = 8888;

        public BabbleOSC(int? port = null)
        {
            if (_receiver != null)
            {
                UniLog.Error("BabbleOSC connection already exists.");
                return;
            }

            IPAddress canidate;
            IPAddress.TryParse("127.0.0.1", out canidate);

            if (port.HasValue)
                _receiver = new OscReceiver(canidate, port.Value, 300, 1024);
            else
                _receiver = new OscReceiver(canidate, DEFAULT_PORT, 300, 1024);

            foreach (var shape in BabbleExpressions.MouthShapesWithAddress)
                MouthShapesWithAddress.Add(shape, 0f);

            _thread = new Thread(new ThreadStart(ListenLoop));
            _receiver.Connect();
            _thread.Start();
        }

        private static void ListenLoop()
        {
            while (_receiver.State != OscSocketState.Closed)
            {
                try
                {
                    if (_receiver.State == OscSocketState.Connected)
                    {
                        OscPacket packet = _receiver.Receive();
                        if (packet == null)
                        {
                            continue;
                        }
                        if (OscMessage.TryParse(packet.ToString(), out OscMessage message))
                        {
                            if (MouthShapesWithAddress.ContainsKey(message.Address))
                            {
                                if (float.TryParse(message[0].ToString(), out float candidate))
                                {
                                    UniLog.Warning($"Receiving message from {message.Address} with value {candidate}");
                                    MouthShapesWithAddress[message.Address] = candidate;
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
    }
}
