using System.Linq.Expressions;

using Mi.Entity.BASE;
using Mi.Toolkit.Helper;

namespace Mi.Core.DB
{
    public class Updater<T> : IDisposable where T : EntityBase, new()
    {
        private T? instance;
        private IList<string>? keys;

        public Updater()
        {
            instance = Activator.CreateInstance<T>();
            keys = new List<string>(16);
        }

        public Updater<T> Set<TProp>(Expression<Func<T, TProp>> propSelect, TProp value)
        {
            var expr = (propSelect.Body as MemberExpression);
            if (expr == null) throw new Exception("update prop not exist");

            var value_expr = Expression.Parameter(typeof(TProp), "value");
            var assign_expr = Expression.Assign(expr, value_expr);
            var model = propSelect.Parameters[0];
            var lambda = Expression.Lambda<Action<T, TProp>>(assign_expr, model, value_expr).Compile();
            lambda(instance!, value);
            keys!.Add(expr.Member.Name);
            return this;
        }

        public Updater<T> SetTime(Expression<Func<T, DateTime?>> propSelect) => Set(propSelect, TimeHelper.LocalTime());

        public Updater<T> SetUser(Expression<Func<T, long?>> propSelect, long userId) => Set(propSelect, userId);

        public Updater<T> ModifiedTime() => SetTime(x => x.ModifiedOn);

        public Updater<T> ModifiedUser(long userId) => SetUser(x => x.ModifiedBy, userId);

        public Updater<T> MarkDeleted() => Set(x => x.IsDeleted, 1);

        public KeyValuePair<IList<string>, T> Explain() => new KeyValuePair<IList<string>, T>(keys!, instance!);

        public void Dispose()
        {
            instance = default(T);
            keys = null;
        }
    }
}