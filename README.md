Sync
====

Client/Server Entity Synchronization Wireframe

1. Implement ISyncEntity and ISyncServer on server
2. Implement ISyncEntity and ISyncClient on client
3. Use SyncEngine for entities synchronization

Sample:

            client1 = new ClientImpl();
            client2 = new ClientImpl();

            client1.List.Add(new Entity(Guid.NewGuid()) {Data = "data1"});
            client2.List.Add(new Entity(Guid.NewGuid()) {Data = "data2"});

            Console.WriteLine("Before ...");
            ToConsole();

            SyncEngine.PullAndPushChanges(client1, ServerProxy.Instance);
            SyncEngine.PullAndPushChanges(client2, ServerProxy.Instance);

            Console.WriteLine("After sync (Client1 <-> Server, Client2 <-> Server) ...");
            ToConsole();
            
            
Output:

            Before ...
             Client1:
              Entity ID:913153ea, Modified:Y Deleted:N (data1)
             Client2:
              Entity ID:b982c6ca, Modified:Y Deleted:N (data2)
             Server:
            
            After sync (Client1 <-> Server, Client2 <-> Server) ...
             Client1:
              Entity ID:913153ea, Modified:N Deleted:N (data1)
             Client2:
              Entity ID:b982c6ca, Modified:N Deleted:N (data2)
              Entity ID:913153ea, Modified:N Deleted:N (data1)
             Server:
              Entity ID:913153ea, Mod(UTC):08:55:32 Sync(UTC):08:55:32 Deleted:N (data1)
              Entity ID:b982c6ca, Mod(UTC):08:55:32 Sync(UTC):08:55:32 Deleted:N (data2)
