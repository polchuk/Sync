using System;

namespace Sync.Server
{
    public interface IEntity
    {
        string Data { get; set; }
    }

    class Entity : ISyncEntity, IEntity
    {
        public Guid ID { get; set; }
        public DateTime ClientModifyDate { get; set; }
        public DateTime ServerSyncDate { get; set; }
        public bool IsDeleted { get; set; }

        public string Data { get; set; }

        public Entity(Guid id)
        {
            ID = id;
        }

        public override string ToString()
        {
            return string.Format("Entity ID:{0}, Mod(UTC):{1} Sync(UTC):{2} Deleted:{3} ({4})",
                ID.Format(), ClientModifyDate.Format(), ServerSyncDate.Format(), IsDeleted.Format(), Data);
        }
    }
}
