using System;

namespace Sync
{
    public interface ISyncEntity
    {
        Guid ID { get; }
        DateTime ClientModifyDate { get; }
        DateTime ServerSyncDate { get; }
        bool IsDeleted { get; }
    }

    public interface ISyncServer
    {
        ISyncEntity GetChangedFromServer(DateTime after_sync_date);
        DateTime SendChangedToServer(ISyncEntity entity);
    }

    public interface ISyncClient
    {
        DateTime GetMaxServerSyncDate();
        void ApplyChangeFromServer(ISyncEntity entity);
        ISyncEntity GetChangedOnClient();
        void SetClientEntitySynced(ISyncEntity entity, DateTime server_sync_date);
    }
}
