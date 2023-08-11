using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Extensions
{
    public class Algorithms
    {
        public static int BinarySearch(List<int> collection, int value)
        {
            int left = 0, right = collection.Count;
            int mid;
            while (left < right - 1)
            {
                mid = (left + right) >> 1;
                if (collection[mid] > value) right = mid;
                else left = mid;
            }

            return left;
        }
    }
}
