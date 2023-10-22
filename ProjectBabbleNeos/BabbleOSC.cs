using Elements.Core;
using OscCore;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ProjectBabbleNeos
{
    public class BabbleOSC
    {
        public readonly Dictionary<string, float> MouthShapesWithAddress = new Dictionary<string, float>();
        private static UdpClient? _receiver;
        private readonly Task? _task;
        private bool OscSocketState;
        private const int DEFAULT_PORT = 8888;

        public BabbleOSC(int? port = null)
        {
            if (_receiver != null)
            {
                UniLog.Error("BabbleOSC connection already exists.");
                return;
            }

            IPAddress.TryParse("127.0.0.1", out var candidate);

            if (port.HasValue)
                _receiver = new UdpClient(new IPEndPoint(candidate, port.Value));
            else
                _receiver = new UdpClient(new IPEndPoint(candidate, DEFAULT_PORT));

            foreach (var shape in BabbleExpressions.MouthShapesWithAddress)
                MouthShapesWithAddress.Add(shape, 0f);

            OscSocketState = true;
            _task = Task.Run(() => ListenLoop());
        }

        private async void ListenLoop()
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
            OscSocketState = false;
            _receiver.Close();
            _task.Wait();
        }
    }
}
