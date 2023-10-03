using Elements.Core;
using OscCore;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ProjectBabbleResonite
{
    public class BabbleOsc
    {
        private static bool _oscSocketState;
        public static readonly Dictionary<string, float> MouthShapesWithAddress = new();
        private static UdpClient? _receiver;
        private static Task? _task;
        private const int DefaultPort = 8888;

        public BabbleOsc(int? port = null)
        {
            if (_receiver != null)
            {
                UniLog.Error("BabbleOSC connection already exists.");
                return;
            }

            IPAddress.TryParse("127.0.0.1", out var candidate);

            _receiver = port.HasValue
                ? new UdpClient(new IPEndPoint(candidate, port.Value))
                : new UdpClient(new IPEndPoint(candidate, DefaultPort));

            foreach (var shape in BabbleExpressions.MouthShapesWithAddress)
                MouthShapesWithAddress.Add(shape, 0f);

            _oscSocketState = true;
            _task = Task.Run(ListenLoop);
        }

        private static async void ListenLoop()
        {
            UniLog.Log("Started Babble loop");
            while (_oscSocketState)
            {
                var result = await _receiver.ReceiveAsync();
                var message = OscMessage.Read(result.Buffer, 0, result.Buffer.Length);
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
            _oscSocketState = false;
            _receiver.Close();
            _task.Wait();
            UniLog.Log("Babble teardown completed");
        }
    }
}