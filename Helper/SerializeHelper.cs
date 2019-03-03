using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Helper
{
    /// <summary>
    /// 二进制序列化帮助类，被序列化类和其中的属性类需要加上[Serializable]特性
    /// </summary>
    public class SerializeHelper
    {
        public static string Serializeable(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, obj);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public static T Derializeable<T>(string obj)
        {
            var objByteArr = Convert.FromBase64String(obj);
            using (MemoryStream ms = new MemoryStream(objByteArr))
            {
                return (T)new BinaryFormatter().Deserialize(ms);
            }
        }

        /// <summary>
        /// 深克隆
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T DeepClone<T>(T t)
        {
            return Derializeable<T>(Serializeable(t));
        }
    }
}
