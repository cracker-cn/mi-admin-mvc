using Mi.Core.GlobalUser;
using Mi.Entity.BASE;
using Mi.Core.Toolkit.Helper;

namespace Mi.Core.Factory
{
    public class CreatorFactory
    {
        private readonly IMiUser _miUser;

        public CreatorFactory(IMiUser miUser)
        {
            _miUser = miUser;
        }

        public T New<T>() where T : EntityBase, new()
        {
            var entity = new T();
            entity.Id = IdHelper.SnowflakeId();
            entity.CreatedBy = _miUser.UserId;
            entity.CreatedOn = TimeHelper.LocalTime();
            return entity;
        }
    }
}