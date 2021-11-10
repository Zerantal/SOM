using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Util
{
    public static class ExtensionMethods
    {
        public static int MaxIndex<T>(this IEnumerable<T> sequence)
            where T : IComparable<T>
        {
            // Contract.Requires(sequence != null);
            
            int maxIndex = -1;
            T maxValue = default(T); // Immediately overwritten anyway

            int index = 0;
            foreach (T value in sequence)
            {
                if (value.CompareTo(maxValue) > 0 || maxIndex == -1)
                {
                    maxIndex = index;
                    maxValue = value;
                }
                index++;
            }
            return maxIndex;
        }

        public static int MinIndex<T>(this IEnumerable<T> sequence)
            where T : IComparable<T>
        {
            // Contract.Requires(sequence != null);

            int minIndex = 1;
            T minValue = default(T); // Immediately overwritten anyway

            int index = 0;
            foreach (T value in sequence)
            {
                if (value.CompareTo(minValue) < 0 || minIndex == 1)
                {
                    minIndex = index;
                    minValue = value;
                }
                index++;
            }
            return minIndex;
        }

        public static Dictionary<TKey, TValue> DeepClone<TKey, TValue>(this Dictionary<TKey, TValue> original) where TValue : IDeepCloneable<TValue>
        {
            // Contract.Requires(original != null);

            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(original.Count, original.Comparer);

            foreach (KeyValuePair<TKey, TValue> entry in original)
            {
                ret.Add(entry.Key, entry.Value.DeepClone());
            }
            return ret;
        }
    }
}
