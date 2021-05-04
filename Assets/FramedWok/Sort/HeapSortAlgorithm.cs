using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FramedWok.Sorting
{
    public static class HeapSortAlgorithm
    {
        private static int marker = 0;

        /// <summary>
        /// Sorts a list using the heap sort algorithm
        /// </summary>
        /// <param name="list">The list to sort</param>
        /// <returns>The sorted list</returns>
        public static List<T> HeapSort<T>(List<T> list) where T : IComparable<T>
        {
            BuildMaxHeap(list);
            for (int i = marker; i > 0; --i)
            {
                Swap(list, i - 1, 0);
                marker -= 1;
                Heapify(list, 0);
            }
            return list;
        }

        /// <summary>
        /// Forms the whole list into a heap
        /// </summary>
        private static void BuildMaxHeap<T>(List<T> list) where T : IComparable<T>
        {
            marker = list.Count;
            for (int i = marker / 2; i > 0; --i)
            {
                Heapify(list, i - 1);
            }
        }

        /// <summary>
        /// Ensures that an item in the list satisfies the heap property
        /// Any item in the list is either the biggest item, or has a parent that is bigger
        /// Each item can have up to two children.
        /// </summary>
        private static void Heapify<T>(List<T> list, int index) where T : IComparable<T>
        {
            int left = (2 * (index + 1)) - 1;
            int right = left + 1;

            int max = index;

            if (left < marker)
            {
                if (list[left].CompareTo(list[index]) == 1)
                    max = left;
            }

            if (right < marker)
            {
                if (list[right].CompareTo(list[max]) == 1)
                    max = right;
            }

            if (max != index)
            {
                Swap(list, index, max);
                Heapify(list, max);
            }
        }

        /// <summary>
        /// Swap two items in a list
        /// </summary>
        private static void Swap<T>(List<T> list, int a, int b) where T : IComparable<T>
        {
            T temp = list[a];
            list[a] = list[b];
            list[b] = temp;
        }
    }
}