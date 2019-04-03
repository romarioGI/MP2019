using System;

namespace Lab4.SkipList
{
    internal class Node<TKey, TValue>
    {
        public TKey Key { get; }
        public TValue Value { get; set; }

        public Node<TKey, TValue> 
            Right,
            Up,
            Down;

        public Node()
        {
        }

        public Node(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Right = null;
            Up = null;
            Down = null;
        }
    }

    public class SkipList<TKey, TValue> where TKey : IComparable<TKey>
    {
        private readonly int _maxLevel;
        private int _curLevel;
        private readonly double _probability;
        private readonly Random _rd;

        private readonly Node<TKey, TValue>[] _head;
        private readonly Node<TKey, TValue> _tail;

        public int Count { get; private set; }

        public SkipList(int maxLevels = 17, double p = 0.5)
        {
            _maxLevel = maxLevels;
            _probability = p;
            _curLevel = 0;
            _rd = new Random(DateTime.Now.Millisecond);

            _head = new Node<TKey, TValue>[_maxLevel];
            _tail = new Node<TKey, TValue>();

            _head[0] = new Node<TKey, TValue> { Right = _tail };
            for (var i = 1; i < _maxLevel; i++)
            {
                _head[i] = new Node<TKey, TValue> {Right = _tail, Down = _head[i - 1]};
                _head[i - 1].Up = _head[i];
            }
        }

        private Node<TKey, TValue>[] FindPreviousItems(TKey key)
        {
            var previousItems = new Node<TKey, TValue>[_maxLevel];
            var current = _head[_curLevel];
            for (var i = _curLevel; i >= 0; i--)
            {
                while (current.Right != _tail && current.Right.Key.CompareTo(key) < 0)
                    current = current.Right;

                previousItems[i] = current;
                current = current.Down;
            }

            return previousItems;
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException();

            var previousItems = FindPreviousItems(key);
            if (previousItems[0].Right != _tail && previousItems[0].Right.Key.CompareTo(key) == 0)
                throw new ArgumentException("Key must be unique");

            var height = 0;
            while (_rd.NextDouble() < _probability && height < _maxLevel - 1)
                height++;

            if (height > _curLevel)
            {
                for (var i = _curLevel + 1; i <= height; i++)
                    previousItems[i] = _head[i];

                _curLevel = height;
            }

            var newItem = new Node<TKey, TValue>(key, value) {Right = previousItems[0].Right};
            previousItems[0].Right = newItem;
            for (var i = 1; i <= height; i++)
            {
                newItem = new Node<TKey, TValue>(key, value) {Right = previousItems[i].Right};
                previousItems[i].Right = newItem;
                newItem.Down = previousItems[i - 1].Right;
                previousItems[i - 1].Right.Up = newItem;
            }

            Count++;
        }

        private Node<TKey, TValue> FindItem(TKey key)
        {
            var current = _head[_curLevel];
            for (var i = _curLevel; i >= 0; i--)
            {
                while (current.Right != _tail && current.Right.Key.CompareTo(key) < 0)
                    current = current.Right;

                if (current.Right != _tail && current.Right.Key.CompareTo(key) == 0)
                    return current.Right;

                current = current.Down;
            }

            return null;
        }

        public bool ContainsKey(TKey key)
        {
            var previousItem = FindItem(key);
            return previousItem != null && previousItem.Key.CompareTo(key) == 0;
        }

        public bool Remove(TKey key)
        {
            var previousItems = FindPreviousItems(key);
            if (previousItems[0].Right == _tail || previousItems[0].Right.Key.CompareTo(key) != 0)
                return false;

            foreach (var item in previousItems)
            {
                if (item == null || item.Right == _tail || item.Right.Key.CompareTo(key) != 0)
                    break;
                item.Right = item.Right.Right;
            }

            while (_curLevel > 0 && _head[_curLevel].Right == _tail)
                _curLevel--;

            Count--;

            return true;
        }

        public TValue this[TKey key]
        {
            get
            {
                var item = FindItem(key);
                if (item == null || item.Key.CompareTo(key) != 0)
                    throw new ArgumentException();

                return item.Value;
            }
            set
            {
                var item = FindItem(key);
                if (item == null || item.Key.CompareTo(key) != 0)
                    throw new ArgumentException();

                item.Value = value;
            }
        }

        public void Print()
        {
            for (var i = _curLevel; i >= 0; i--)
            {
                var current = _head[i].Right;
                while (current != _tail)
                {
                    Console.Write(current.Key + " ");
                    current = current.Right;
                }
                Console.WriteLine();
            }
        }
    }
}
