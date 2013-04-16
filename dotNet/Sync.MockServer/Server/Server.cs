using System;
using System.Collections.Generic;

namespace Sync.Server
{
    class Server : ISyncServer, IToConsole
    {
        public readonly Dictionary<Guid, Entity> List = new Dictionary<Guid, Entity>();

        public Server()
        {
        }

        public ISyncEntity GetChangedFromServer(DateTime after_sync_date)
        {
            //find entity with minimal sync date which is > after_sync_date
            Entity res = null;
            foreach (var pair in List)
            {
                if (pair.Value.ServerSyncDate > after_sync_date)
                {
                    if (res == null || res.ServerSyncDate > pair.Value.ServerSyncDate)
                        res = pair.Value;
                }
            }
            return res;
        }

        public DateTime SendChangedToServer(ISyncEntity proxy)
        {
            Entity entity = null;
            if (List.ContainsKey(proxy.ID))
            {
                entity = List[proxy.ID];
                if (proxy.IsDeleted)
                    Delete(entity, proxy);
                else
                    Update(entity, proxy);
            }
            else
            {
                entity = Create(proxy);
            }
            return entity.ServerSyncDate;
        }

        private Entity Create(ISyncEntity proxy)
        {
            var entity = new Entity(proxy.ID);
            entity.ClientModifyDate = proxy.ClientModifyDate;
            entity.ServerSyncDate = DateTime.UtcNow;

            entity.Data = ((IEntity)proxy).Data;
            
            List.Add(entity.ID, entity);
            return entity;
        }

        private void Update(Entity entity, ISyncEntity proxy)
        {
            if (!entity.IsDeleted && entity.ClientModifyDate < proxy.ClientModifyDate)
            {
                entity.ClientModifyDate = proxy.ClientModifyDate;
                entity.ServerSyncDate = DateTime.UtcNow;

                entity.Data = ((IEntity)proxy).Data;
            }
        }

        private void Delete(Entity entity, ISyncEntity proxy)
        {
            entity.ClientModifyDate = proxy.ClientModifyDate;
            entity.ServerSyncDate = DateTime.UtcNow;
            entity.IsDeleted = true;
        }

        public void ToConsole()
        {
            Console.WriteLine(" Server:");
            foreach (var e in List)
            {
                Console.WriteLine("  " + e.Value);
            }
        }
    }
}
