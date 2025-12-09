using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace System.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// 将父类谓词表达式转换为子类谓词表达式
        /// </summary>
        public static Expression<Func<TDerived, bool>> ToDerived<TBase, TDerived>(
            this Expression<Func<TBase, bool>> basePredicate)
            where TDerived : TBase
        {
            var parameter = Expression.Parameter(typeof(TDerived), basePredicate.Parameters[0].Name);
            var body = new ReplaceParameterVisitor(basePredicate.Parameters[0], parameter)
                .Visit(basePredicate.Body);

            return Expression.Lambda<Func<TDerived, bool>>(body, parameter);
        }

        /// <summary>
        /// 将父类导航属性表达式转换为子类导航属性表达式（用于 EF Core Include）
        /// </summary>
        public static Expression<Func<TDerived, object>> ToDerived<TBase, TDerived>(
            this Expression<Func<TBase, object>> baseInclude)
            where TDerived : TBase
        {
            var parameter = Expression.Parameter(typeof(TDerived), baseInclude.Parameters[0].Name);
            var body = new ReplaceParameterVisitor(baseInclude.Parameters[0], parameter)
                .Visit(baseInclude.Body);

            return Expression.Lambda<Func<TDerived, object>>(body, parameter);
        }

        /// <summary>
        /// 通用参数替换器
        /// </summary>
        private class ReplaceParameterVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParam;
            private readonly ParameterExpression _newParam;

            public ReplaceParameterVisitor(ParameterExpression oldParam, ParameterExpression newParam)
            {
                _oldParam = oldParam;
                _newParam = newParam;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == _oldParam ? _newParam : base.VisitParameter(node);
            }
        }
    }
}
