using System;

namespace Sync.Client
{
    public class ServerEntity : ISyncEntity, Sync.Server.IEntity
    {
        public Guid ID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime ClientModifyDate { get; set; }
        public DateTime ServerSyncDate { get; set; }
        public string Data { get; set; }
    }

    public class ServerProxy : ISyncServer
    {
        public ISyncEntity GetChangedFromServer(DateTime after_sync_date)
        {
            var entity = Sync.Server.ServerImpl.Instance.GetChangedFromServer(after_sync_date);
            if (entity == null)
                return null;

            // Create client entity from server result
            return new Entity()
                {
                    ID = entity.ID,
                    ClientModifyDate = entity.ClientModifyDate,
                    ServerSyncDate = entity.ServerSyncDate,
                    IsDeleted = entity.IsDeleted,
                    Data = ((Server.IEntity)entity).Data
                };
        }

        public DateTime SendChangedToServer(ISyncEntity entity)
        {
            // Create server proxy from client entity
            var proxy = new ServerEntity()
                {
                    ID = entity.ID,
                    ClientModifyDate = entity.ClientModifyDate,
                    IsDeleted = entity.IsDeleted,
                    Data = ((Client.IEntity)entity).Data
                };

            return Server.ServerImpl.Instance.SendChangedToServer(proxy);
        }

        public void ToConsole()
        {
            ((IToConsole)Server.ServerImpl.Instance).ToConsole();
        }

        //Singleton
        private static ServerProxy instance = null;

        public static ServerProxy Instance
        {
            get
            {
                if (instance == null)
                    instance = new ServerProxy();
                return instance;
            }
        }
    }
}
