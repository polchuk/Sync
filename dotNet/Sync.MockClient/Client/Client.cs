using System;
using System.Collections.Generic;

namespace Sync.Client
{
    public class ClientImpl : ISyncClient
    {
        public readonly List<Entity> List = new List<Entity>();

        public ClientImpl()
        {
        }

        public DateTime GetMaxServerSyncDate()
        {
            var res = DateTime.MinValue;
            foreach (var e in List)
            {
                if (e.ServerSyncDate > res)
                    res = e.ServerSyncDate;
            }
            return res;
        }

        public ISyncEntity GetChangedOnClient()
        {
            foreach (var e in List)
            {
                if (e.IsModified)
                    return Clone(e);
            }
            return null;
        }

        public void SetClientEntitySynced(ISyncEntity entity, DateTime server_last_sync_date)
        {
            Entity e = null;
            foreach (var e_tmp in List)
            {
                if (e_tmp.ID == entity.ID)
                {
                    e = e_tmp;
                    break;
                }
            }

            if (e == null)
                return;

            if (e.IsDeleted)
            {
                List.Remove(e);
            }
                //check if entity was not modified localy while sending to server
            else if (e.ClientModifyDate == entity.ClientModifyDate)
            {
                e.ServerSyncDate = server_last_sync_date;
                e.IsModified = false;
            }
        }

        public void ApplyChangeFromServer(ISyncEntity proxy)
        {
            Entity entity = null;
            foreach (var e in List)
            {
                if (e.ID == proxy.ID)
                {
                    entity = e;
                    break;
                }
            }

            if (entity != null)
            {
                if (proxy.IsDeleted)
                {
                    Delete(entity);
                }
                else
                {
                    Update(proxy, entity);
                }
            }
            else
            {
                Create(proxy);
            }
        }

        private void Create(ISyncEntity proxy)
        {
            var entity = new Entity(proxy.ID);
            entity.ClientModifyDate = proxy.ClientModifyDate;
            entity.ServerSyncDate = proxy.ServerSyncDate;

            entity.Data = ((IEntity)proxy).Data;

            List.Add(entity);
        }

        private void Update(ISyncEntity proxy, Entity entity)
        {
            if (entity.ClientModifyDate < proxy.ClientModifyDate)
            {
                entity.ClientModifyDate = proxy.ClientModifyDate;
                entity.ServerSyncDate = proxy.ServerSyncDate;

                entity.Data = ((IEntity)proxy).Data;
            }
        }

        private void Delete(Entity entity)
        {
            List.Remove(entity);
        }

        private Entity Clone(Entity entity)
        {
            var clone = new Entity();
            clone.ID = entity.ID;
            clone.ClientModifyDate = entity.ClientModifyDate;
            clone.ServerSyncDate = entity.ServerSyncDate;
            clone.IsDeleted = entity.IsDeleted;
            clone.IsModified = entity.IsModified;
            clone.Data = entity.Data;
            return clone;
        }

        public void ToConsole(string name)
        {
            Console.WriteLine(" " + name + ":");
            foreach (var e in List)
            {
                Console.WriteLine("  " + e);
            }
        }
    }
}
