﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dafujian.Data.Base
{
    /// <summary>
    /// EF辅助类
    /// </summary>
    public static class EfUtils
    {

        public static Expression<Func<T, bool>> True<T>()
        {
            return (T f) => true;
        }

        public static Expression<Func<T, bool>> False<T>()
        {
            return (T f) => false;
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> exp_left, Expression<Func<T, bool>> exp_right)
        {
            var parameterExpression = Expression.Parameter(typeof(T), "candidate");
            var expr = new ParameterReplacer(parameterExpression);
            var left = expr.Replace(exp_left.Body);
            var right = expr.Replace(exp_right.Body);
            return Expression.Lambda<Func<T, bool>>(Expression.Or(left, right), new ParameterExpression[]
            {
                parameterExpression
            });
        }

        public static Expression<Func<T, bool>> AndOr<T>(string[] keys, string orKey, T t)
        {
            if (keys == null)
            {
                return null;
            }
            var expression = EfUtils.And<T>(keys, t);
            if (!string.IsNullOrEmpty(orKey))
            {
                Expression<Func<T, bool>> exp_right = EfUtils.And<T>(new string[]
                {
                    orKey
                }, t);
                expression = expression.Or(exp_right);
            }
            return expression;
        }


        public static Expression<Func<T, bool>> And<T>(string[] keys, T t)
        {
            if (keys == null)
            {
                return null;
            }
            var expression = EfUtils.True<T>();
            for (int i = 0; i < keys.Length; i++)
            {
                var text = keys[i];
                var expr = t.GetType().GetProperty(text);
                var val = expr.GetValue(t, null).ToString();
                var type = expr.ToString();
                if (text.ToLower().Equals("datetime") || text.ToLower().Equals("clientdatetime"))
                {
                    expression = expression.And(EfUtils.AndIndexOf<T>(type, text, val));
                }
                else
                {
                    expression = expression.And(EfUtils.And<T>(type, text, val));
                }
            }
            return expression;
        }

        public static Expression<Func<T, bool>> AndByGuid<T>(string key, Guid val)
        {
            if (string.IsNullOrEmpty(key) || val == null)
                return null;
            var expression = EfUtils.True<T>();
            expression = expression.And(EfUtils.AndByGuid<T>("Guid", key, val));
            return expression;
        }

        public static Expression<Func<T, bool>> AndByGuid<T>(string[] keys, Guid val)
        {
            if (keys == null || val == null)
            {
                return null;
            }
            var expression = EfUtils.True<T>();
            for (int i = 0; i < keys.Length; i++)
            {
                var text = keys[i];
                expression = expression.And(EfUtils.AndByGuid<T>("Guid", text, val));
            }
            return expression;
        }


        public static Expression<Func<T, bool>> AndByGuid<T>(string type, string key, Guid val)
        {
            var parameterExpression = Expression.Parameter(typeof(T), "candidate");
            var arg = Expression.Property(parameterExpression, typeof(T).GetProperty(key));
            var right = Expression.Constant(val);
            return Expression.Lambda<Func<T, bool>>(Expression.Equal(arg, right), new ParameterExpression[]
            {
                parameterExpression
            });
        }

        public static Expression<Func<T, bool>> And<T>(string type, string key, string val)
        {
            var parameterExpression = Expression.Parameter(typeof(T), "candidate");
            var arg = Expression.Property(parameterExpression, typeof(T).GetProperty(key));
            Expression right;
            if (type.ToLower().IndexOf("system.nullable`1[system.int32]") >= 0)
            {
                right = Expression.Constant(Convert.ToInt32(val), typeof(int?));
            }
            if (type.ToLower().IndexOf("int32") >= 0)
            {
                right = Expression.Constant(Convert.ToInt32(val), typeof(int));
            }
            else if (type.ToLower().IndexOf("system.guid") >= 0)
            {
                right = Expression.Constant(Guid.Parse(val), typeof(Guid));
            }
            else if (type.ToLower().IndexOf("system.datetime") >= 0)
            {
                right = Expression.Constant(DateTime.Parse(val), typeof(DateTime));
            }
            else
            {
                right = Expression.Constant(val);
            }
            return Expression.Lambda<Func<T, bool>>(Expression.Equal(arg, right), new ParameterExpression[]
            {
                parameterExpression
            });
        }



        public static Expression<Func<T, bool>> AndIndexOf<T>(string type, string key, string val)
        {
            var parameterExpression = Expression.Parameter(typeof(T), "candidate");
            var arg = Expression.Property(parameterExpression, typeof(T).GetProperty(key));
            var method = typeof(string).GetMethod("Contains");
            var constantExpression = Expression.Constant(val);
            return Expression.Lambda<Func<T, bool>>(Expression.Call(arg, method, new Expression[]
            {
                constantExpression
            }), new ParameterExpression[]
            {
                parameterExpression
            });
        }
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> exp_left, Expression<Func<T, bool>> exp_right)
        {
            var parameterExpression = Expression.Parameter(typeof(T), "candidate");
            var expr = new ParameterReplacer(parameterExpression);
            var left = expr.Replace(exp_left.Body);
            var right = expr.Replace(exp_right.Body);
            return Expression.Lambda<Func<T, bool>>(Expression.And(left, right), new ParameterExpression[]
            {
                parameterExpression
            });
        }
    }

    internal class ParameterReplacer : ExpressionVisitor
    {
        public ParameterExpression ParameterExpression
        {
            get;
            private set;
        }
        public ParameterReplacer(ParameterExpression paramExpr)
        {
            this.ParameterExpression = paramExpr;
        }
        public Expression Replace(Expression expr)
        {
            return this.Visit(expr);
        }
        protected override Expression VisitParameter(ParameterExpression p)
        {
            return this.ParameterExpression;
        }
    }
}
