using BaseX;
using OscCore;
using System;
using System.Net;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ProjectBabbleNeos
{
    public class BabbleOSC
    {
        public static bool OscSocketState;
        public static Dictionary<string, float> MouthShapesWithAddress = new Dictionary<string, float>();
        private static UdpClient? _receiver;
        private static Task? _task;
        private const int DEFAULT_PORT = 8888;

        public BabbleOSC(int? port = null)
        {
            if (_receiver != null)
            {
                UniLog.Error("BabbleOSC connection already exists.");
                return;
            }

            IPAddress candidate;
            IPAddress.TryParse("127.0.0.1", out candidate);

            if (port.HasValue)
                _receiver = new UdpClient(new IPEndPoint(candidate, port.Value));
            else
                _receiver = new UdpClient(new IPEndPoint(candidate, DEFAULT_PORT));

            foreach (var shape in BabbleExpressions.MouthShapesWithAddress)
                MouthShapesWithAddress.Add(shape, 0f);

            OscSocketState = true;
            _task = Task.Run(() => ListenLoop());
        }

        private static async void ListenLoop()
        {
            UniLog.Log("Started Babble loop");
            while (OscSocketState)
            {
                var result = await _receiver.ReceiveAsync();
                OscMessage message = OscMessage.Read(result.Buffer, 0, result.Buffer.Length);
                if (!MouthShapesWithAddress.ContainsKey(message.Address))
                {
                    continue;
                }
                if (float.TryParse(message[0].ToString(), out float candidate))
                {
                    MouthShapesWithAddress[message.Address] = candidate;
                }
            }
        }

        public void Teardown()
        {
            UniLog.Log("Babble teardown called");
            OscSocketState = false;
            _receiver.Close();
            _task.Wait();
            UniLog.Log("Babble teardown completed");
        }
    }
}
