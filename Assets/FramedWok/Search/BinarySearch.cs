using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FramedWok.Searching
{
    public static class BinarySearch
    {
        /// <summary>
        /// Perform a binary search to look for an item in a list.
        /// This algorithm only works on sorted lists
        /// </summary>
        /// <param name="list">The lsit being searched through</param>
        /// <param name="itemToLookFor">The item we're looking for</param>
        /// <returns>The item from the list, or the default value if it isn't there</returns>
        public static T Search<T>(List<T> list, T itemToLookFor) where T : IComparable<T>
        {
            if (list.Count == 0)
                return default;
            if(list.Count == 1)
                return list[0].CompareTo(itemToLookFor) == 0 ? list[0] : default;

            int itemIndex = list.Count / 2;
            T itemToCheck = list[itemIndex];
            switch (itemToCheck.CompareTo(itemToLookFor))
            {
                case 0:
                    return itemToCheck;
                case 1:
                    //The item we're looking for is smaller
                    return Search(list.GetRange(0, itemIndex), itemToLookFor);
                case -1:
                    //The item we're looking for is larger
                    itemIndex++;
                    return Search(list.GetRange(itemIndex, list.Count - itemIndex), itemToLookFor);
                default:
                    //Something weird happened
                    return default;
            }
        }
    }
}