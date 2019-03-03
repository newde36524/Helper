using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Helper
{
    public class ObjectCloneHelper
    {
        private static Dictionary<string, object> _Dic = new Dictionary<string, object>();
        public static TOut TransExp<TIn, TOut>(TIn tIn)
        {
            string key = $"funckey_{typeof(TIn).FullName}_{typeof(TOut).FullName}";
            if (!_Dic.Keys.Contains(key))
            {
                ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
                List<MemberBinding> memberBindingList = new List<MemberBinding>();
                foreach (var item in typeof(TOut).GetProperties())
                {
                    PropertyInfo propertyInfo = typeof(TIn).GetProperty(item.Name);//找到相同的属性
                    if (propertyInfo == null) { continue; }
                    MemberExpression property = Expression.Property(parameterExpression, propertyInfo);
                    memberBindingList.Add(Expression.Bind(item, property));
                }
                foreach (var item in typeof(TOut).GetFields())
                {
                    FieldInfo fieldInfo = typeof(TIn).GetField(item.Name);
                    if (fieldInfo == null) { continue; }
                    MemberExpression property = Expression.Field(parameterExpression, fieldInfo);
                    memberBindingList.Add(Expression.Bind(item, property));
                }
                Expression<Func<TIn, TOut>> expression = Expression.Lambda<Func<TIn, TOut>>(Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList), new ParameterExpression[]
                {
                    parameterExpression
                });
                Func<TIn, TOut> func = expression.Compile();
                _Dic.Add(key, func);
            }
            return ((Func<TIn, TOut>)_Dic[key])(tIn);
        }
    }
}
