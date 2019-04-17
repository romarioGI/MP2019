using System;
using System.Collections.Generic;

namespace Lab5.Heap
{
    public class BinaryHeap<TKey, TValue>
    {
        private class Comparator<T> : IComparer<T> 
        {
            public int Compare(T x, T y)
            {
                if (x == null || y == null)
                    throw new NullReferenceException();
                return ((IComparable<T>)x).CompareTo(y);
            }
        }

        private readonly IComparer<TKey> _comparator;

        private KeyValuePair<TKey, TValue>[] _array;

        public int Capacity { get { return _array.Length; } }

        public int Count { get; private set; }

        public BinaryHeap() : this(7) { }

        public BinaryHeap(IComparer<TKey> comparator) : this(7, comparator) { }

        public BinaryHeap(int capacity, IComparer<TKey> comparator = null)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException();

            _array = new KeyValuePair<TKey, TValue>[capacity];
            Count = 0;

            if (comparator == null)
                if(typeof(TKey).GetInterface("IComparable") == typeof(IComparable))
                    _comparator = new Comparator<TKey>();
                else
                    throw new ArgumentException();
            else
                _comparator = comparator;
        }

        private void ExpandArray()
        {
            var oldArray = _array;
            _array = new KeyValuePair<TKey, TValue>[((Capacity + 1) << 1) - 1];

            for (var i = 0; i < oldArray.Length; i++)
                _array[i] = oldArray[i];
        }

        private static void Swap<T>(ref T x, ref T y)
        {
            var temp = x;
            x = y;
            y = temp;
        }

        public void Push(TKey key, TValue value)
        {
            if (Count == Capacity)
                ExpandArray();
            _array[Count] = new KeyValuePair<TKey, TValue>(key,value);
            Count++;

            var current = Count - 1;
            var parent = (current - 1) >> 1;
            while (current > 0 && _comparator.Compare(_array[current].Key, _array[parent].Key) < 0)
            {
                Swap(ref _array[parent], ref _array[current]);

                current = parent;
                parent = (current - 1) >> 1;
            }
        }

        public KeyValuePair<TKey, TValue> GetMin()
        {
            if (Count == 0)
                throw new IndexOutOfRangeException();
            return _array[0];
        }

        public KeyValuePair<TKey, TValue> PopMin()
        {
            if (Count == 0)
                throw new IndexOutOfRangeException();
            var min = _array[0];
            Count--;
            _array[0] = _array[Count];
            _array[Count] = default(KeyValuePair<TKey, TValue>);
            var current = 0;
            while (true)
            {
                var left = 2 * current + 1;
                var right = left + 1;

                var cmpL = left < Count && _comparator.Compare(_array[current].Key, _array[left].Key) > 0;
                var cmpR = right < Count && _comparator.Compare(_array[current].Key, _array[right].Key) > 0;

                if (!cmpL && !cmpR)
                    break;

                int next;

                if (cmpL && cmpR)
                    next = _comparator.Compare(_array[left].Key, _array[right].Key) < 0 ? left : right;
                else
                    next = cmpL ? left : right;


                Swap(ref _array[current], ref _array[next]);
                current = next;
            }

            return min;
        }
    }
}
