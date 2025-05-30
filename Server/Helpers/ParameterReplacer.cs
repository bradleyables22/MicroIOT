using System.Linq.Expressions;

namespace Server.Helpers
{
	public class ParameterReplacer : ExpressionVisitor
	{
		private readonly ParameterExpression _old;
		private readonly ParameterExpression _new;

		public ParameterReplacer(ParameterExpression oldParam, ParameterExpression newParam)
		{
			_old = oldParam;
			_new = newParam;
		}

		protected override Expression VisitParameter(ParameterExpression node)
		{
			return node == _old ? _new : base.VisitParameter(node);
		}
	}
}
