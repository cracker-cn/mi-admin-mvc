using Force.DeepCloner;

namespace Mi.Toolkit.Helper
{
	public class CloneHelper
	{
		public static IList<TSource> Clone<TSource>(IList<TSource> sources)
		{
			return sources.DeepClone();
		}
	}
}