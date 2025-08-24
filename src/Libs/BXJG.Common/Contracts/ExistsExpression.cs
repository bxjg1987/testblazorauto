using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BXJG.Common.Contracts
{
    /// <summary>
    /// 辅助判断重复的表达式容器
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class ExistsExpression<TEntity>
    {
        public ExistsExpression()
        {
        }

        public ExistsExpression(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, string>> displayNameProperty)
        {
            Where = where;
            DisplayNameProperty = displayNameProperty;
        }
        /// <summary>
        /// 判断重复的条件
        /// </summary>
        public Expression<Func<TEntity, bool>> Where { get; set; }
        /// <summary>
        /// 显示具体是哪些重复的显示名称
        /// </summary>
        public Expression<Func<TEntity, string>> DisplayNameProperty { get; set; }
    }
}
