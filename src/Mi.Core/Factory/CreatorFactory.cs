using Mi.Core.GlobalUser;
using Mi.Core.Toolkit.Helper;
using Mi.Entity.BASE;

namespace Mi.Core.Factory
{
    public class CreatorFactory
    {
        private readonly IMiUser _miUser;

        public CreatorFactory(IMiUser miUser)
        {
            _miUser = miUser;
        }

        public T NewEntity<T>() where T : EntityBase, new()
        {
            var entity = new T();
            entity.Id = IdHelper.SnowflakeId();
            entity.CreatedBy = _miUser.UserId;
            entity.CreatedOn = TimeHelper.LocalTime();
            return entity;
        }
    }
}