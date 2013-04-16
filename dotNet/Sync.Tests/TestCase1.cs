using System;
using System.Threading;
using Sync.Client;

namespace Sync.Tests
{
    class TestCase1
    {
        static ClientImpl client1;
        static ClientImpl client2;

        public static void Test()
        {
            client1 = new ClientImpl();
            client2 = new ClientImpl();

            client1.List.Add(new Entity(Guid.NewGuid()) {Data = "data1"});
            client2.List.Add(new Entity(Guid.NewGuid()) {Data = "data2"});

            Console.WriteLine();
            Console.WriteLine("Before...");
            ToConsole();

            SyncEngine.PullAndPushChanges(client1, ServerProxy.Instance);
            SyncEngine.PullAndPushChanges(client2, ServerProxy.Instance);

            Console.WriteLine();
            Console.WriteLine("After sync (Client1 <-> Server, Client2 <-> Server) ...");
            ToConsole();

            Thread.Sleep(2000);

            client1.List[0].Data = "modified";
            client1.List[0].SetModified();
            Console.WriteLine();
            Console.WriteLine("Modified on client1...");
            ToConsole();

            SyncEngine.PullAndPushChanges(client1, ServerProxy.Instance);
            SyncEngine.PullAndPushChanges(client2, ServerProxy.Instance);

            Console.WriteLine();
            Console.WriteLine("After sync (Client1 <-> Server, Client2 <-> Server) ...");
            ToConsole();

            Thread.Sleep(2000);

            client1.List[0].IsDeleted = true;
            client1.List[0].SetModified();
            Console.WriteLine();
            Console.WriteLine("Deleted on client...");
            ToConsole();

            SyncEngine.PullAndPushChanges(client1, ServerProxy.Instance);
            SyncEngine.PullAndPushChanges(client2, ServerProxy.Instance);

            Console.WriteLine();
            Console.WriteLine("After sync (Client1 <-> Server, Client2 <-> Server) ...");
            ToConsole();
        }

        private static void ToConsole()
        {
            client1.ToConsole("Client1");
            client2.ToConsole("Client2");
            ServerProxy.Instance.ToConsole();
        }
    }
}
