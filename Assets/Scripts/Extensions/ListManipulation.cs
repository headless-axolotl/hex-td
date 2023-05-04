using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Extensions
{
    public static class ListManipulation
    {
        public static void ShiftNonNull<T>(this List<T> list)
            where T : class
        {
            int delta = 0;
            for(int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                    delta++;
                else if(delta != 0)
                {
                    list[i - delta] = list[i];
                    list[i] = null;
                }
            }
        }

        public static void ShiftNonNull(this List<GameObject> list) // Unity is weird.
        {
            int delta = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                    delta++;
                else if (delta != 0)
                {
                    list[i - delta] = list[i];
                    list[i] = null;
                }
            }
        }

        public static void ResetDuplicates<T>(this List<T> list)
        {
            HashSet<T> containedItems = new();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                    continue;
                if (containedItems.Contains(list[i]))
                    list[i] = default;
                else containedItems.Add(list[i]);
            }
        }
    }
}
