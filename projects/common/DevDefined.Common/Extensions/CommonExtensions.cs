using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevDefined.Common.Extensions
{
    public static class CommonExtensions
    {
        public static Action<Action<int>> LoopTo(this int start, int end)
        {
            return new Action<Action<int>>(action => To(start, end).ForEach(i => action(i)));
        }

        public static IEnumerable<int> To(this int start, int end)
        {
            if (end < start)
                for (int i = start; i > end - 1; i--)
                    yield return i;
            else
                for (int i = start; i < end + 1; i++)
                    yield return i;   
        }

        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (T item in sequence) action(item);
        }

        public static void PrintLine(this object o)
        {
            Console.WriteLine(o);
        }

        public static IDictionary<KeyType,IList<ProjectedType>> ToProjectedDictionaryOfLists<ItemType, KeyType, ProjectedType>
        (
            this IEnumerable<ItemType> that,
            Func<ItemType, KeyType> keyFunc,
            Func<ItemType, ProjectedType> projectedItemFunc)
        {
            var dictionaryOfLists = new Dictionary<KeyType, IList<ProjectedType>>();
            foreach (var item in that)
            {
                var key = keyFunc(item);
                var projectedItem = projectedItemFunc(item);
                IList<ProjectedType> list;
                if (dictionaryOfLists.TryGetValue(key, out list))
                {
                    list.Add(projectedItem);
                }
                else
                {
                    list = new List<ProjectedType>();
                    list.Add(projectedItem);
                    dictionaryOfLists.Add(key, list);
                }
            }

            return dictionaryOfLists;
        }    
    }
}
