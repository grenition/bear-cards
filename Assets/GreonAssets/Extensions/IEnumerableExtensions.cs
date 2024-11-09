using System;
using System.Collections.Generic;
using System.Linq;

namespace GreonAssets.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> objects, Action<T> action)
        {
            foreach (var o in objects) action.Invoke(o);
        }

        public static void Set<T>(this List<T> list, T value)
        {
            if (!list.Contains(value))
            {
                list.Add(value);
            } 
        }
        
        public static bool TryGetFirst<T>(this IEnumerable<T> objects, out T value)
        {
            value = objects.FirstOrDefault();
            return value != null;
        }
        
        public static bool TryGetFirst<T>(this IEnumerable<T> objects, Func<T, bool> predicate, out T value)
        {
            value = objects.FirstOrDefault(predicate);
            return value != null;
        }

        
        public static bool TryAdd<T>(this List<T> list, T value)
        {
            if (!list.Contains(value))
            {
                list.Add(value);
                return true;
            }

            return false;
        }
        
        public static bool TryRemove<T>(this List<T> list, T value)
        {
            if (list.Contains(value))
            {
                return list.Remove(value);
            }

            return false;
        }

        public static IEnumerable<T> AppendIf<T>(this IEnumerable<T> enumerable, T obj, Func<T, bool> condition)
        {
            if (condition == null) return enumerable;
            if (!condition(obj)) return enumerable;

            return enumerable.Append(obj);
        }
    }

    public static class DictionaryExtensions
    {
        public static void Set<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
                return;
            } 
            
            dictionary[key] = value;
        }

        public static void Delete<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            if (!dictionary.ContainsKey(key))
                return;

            dictionary.Remove(key);
        }
        
        public static void DeleteIdenticals<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key) && dictionary[key].Equals(value))
                return;

            dictionary.Remove(key);
        }
        
        public static TValue Get<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
        {
            if(key == null || !dictionary.ContainsKey(key))
                return default;

            return dictionary[key];
        }
    }
}
