using System;
using System.Collections.Generic;
using System.Text;

namespace Helper
{
    public static class Extension
    {
        public static string ToString(this byte[] data, Encoding encoding)
        {
            return encoding.GetString(data);
        }

        public static string ToString(this byte[] data, string encoding)
        {
            return Encoding.GetEncoding(encoding).GetString(data);
        }
        public static IEnumerable<T> TakeSegment<T>(this IEnumerable<ArraySegment<T>> data, int length)
        {
            int pos = 0;
            foreach (ArraySegment<T> seg in data)
            {
                var excute = seg.GetEnumerator();
                while (excute.MoveNext() && pos < length)
                {
                    pos++;
                    yield return excute.Current;
                }
            }
        }
    }
}
