using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Helper
{
    public static class ListHelper
    {
        /// <summary>
        /// DataTable转List(表列的顺序需要和泛型对象一致且需要全部存在)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            List<T> list = new List<T>();
            if (dt != null)
            {
                List<PropertyInfo> infos = new List<PropertyInfo>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    T t = Activator.CreateInstance<T>();
                    foreach (var p in t.GetType().GetProperties())
                    {
                        p.SetValue(t, dt.Rows[i][p.Name]);
                    }
                    list.Add(t);
                }
            }
            return list;
        }

        /// <summary>
        /// 对数据源进行分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IEnumerable<T> QueryPage<T>(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            return source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
      
        /// <summary>
        /// 对数据源进行筛选分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="source"></param>
        /// <param name="funcWhere"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="funcOrderby"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public static IEnumerable<T> QueryPage<T, S>(IEnumerable<T> source, Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, Expression<Func<T, S>> funcOrderby, bool isAsc = true) where T : class
        {
            var list = source;
            if (funcWhere != null)
            {
                list = list.Where(funcWhere.Compile());
            }
            list = isAsc ? list.OrderBy(funcOrderby.Compile()) : list.OrderByDescending(funcOrderby.Compile());
            return list;
        }
    }
}
