using System.Linq.Expressions;

namespace Mi.Core.DB
{
	public class Updater<T> : IDisposable
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

		public KeyValuePair<IList<string>, T> Explain() => new KeyValuePair<IList<string>, T>(keys!, instance!);

		public void Dispose()
		{
			instance = default(T);
			keys = null;
		}
	}
}