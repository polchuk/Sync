using System;

namespace Sync
{
    public class SyncEngine
    {
        public static void PullAndPushChanges(ISyncClient client, ISyncServer server)
        {
            PullChanges(client, server);
            PushChanges(client, server);
        }

        private static void PullChanges(ISyncClient client, ISyncServer server)
        {
            var date = client.GetMaxServerSyncDate();
            while(true)
            {
                var proxy = server.GetChangedFromServer(date);
                if (proxy == null)
                    break;

                client.ApplyChangeFromServer(proxy);
                date = proxy.ServerSyncDate;
            }
        }

        public static void PushChanges(ISyncClient client, ISyncServer server)
        {
            while(true)
            {
                var entity = client.GetChangedOnClient();
                if (entity == null)
                    break;

                var server_sync_date = server.SendChangedToServer(entity);
                client.SetClientEntitySynced(entity, server_sync_date);
            }
        }
    }
}
