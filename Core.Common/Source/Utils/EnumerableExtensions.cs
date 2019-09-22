using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Common
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (action == null)
                return;
            foreach (T item in enumerable)
            {
                action.Invoke(item);
            }
        }

        public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> func)
        {
            foreach (T item in enumerable)
            {
                if (func != null)
                    await func.Invoke(item);
            }
        }

        public static void SafeAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if(!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
        }
    }
}