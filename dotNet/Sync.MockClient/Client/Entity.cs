using System;

namespace Sync.Client
{
    public interface IEntity
    {
        string Data { get; set; }
    }

    public class Entity : ISyncEntity, IEntity
    {
        public Guid ID { get; set; }
        public DateTime ClientModifyDate { get; set; }
        public DateTime ServerSyncDate { get; set; }
        public bool IsDeleted { get; set; }

        public string Data { get; set; }
        public bool IsModified { get; set; }

        public void SetModified()
        {
            ClientModifyDate = DateTime.UtcNow;
            IsModified = true;
        }

        public Entity()
        {
            ID = Guid.NewGuid();
            SetModified();
        }

        public Entity(Guid id)
            : this()
        {
            ID = id;
        }

        public override string ToString()
        {
            return string.Format("Entity ID:{0}, Modified:{1} Deleted:{2} ({3})",
                ID.Format(), IsModified.Format(), IsDeleted.Format(), Data);
        }
    }
}
