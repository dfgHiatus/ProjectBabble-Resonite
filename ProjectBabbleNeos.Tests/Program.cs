using System;

namespace ProjectBabbleNeos.Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BabbleOSC client = new BabbleOSC();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey(true);
            client.Teardown();
        }
    }
}
