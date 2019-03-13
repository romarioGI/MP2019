using System;
using System.Collections;
using System.Collections.Generic;

namespace Lab3.HashTable
{
    internal class Node<TKey, TValue>
    {
        public TKey Key { get; private set; }
        public TValue Value { get; set; }
        public int KeyHashCode { get; private set; }
        public int Next { get; set; }

        public Node(TKey key, TValue value, int keyHashCode)
        {
            Key = key;
            Value = value;
            KeyHashCode = keyHashCode;
            Next = -1;
        }
    }

    internal class HashTableEnumerator<TKey, TValue> : IEnumerator<KeyValuePair<TKey, TValue>>
    {
        private int _pos = -1;
        private readonly Node<TKey, TValue>[] _table;

        public HashTableEnumerator(Node<TKey, TValue>[] table)
        {
            _table = table;
        }

        public void Dispose()
        {

        }

        private int FindPos(int pos)
        {
            while (pos < _table.Length && _table[pos] == null)
                pos++;

            return pos;
        }

        public bool MoveNext()
        {
            _pos = FindPos(_pos + 1);

            if (_pos == _table.Length)
            {
                Reset();
                return false;
            }

            return true;
        }

        public void Reset()
        {
            _pos = -1;
        }

        public KeyValuePair<TKey, TValue> Current
        {
            get
            {
                if (_pos <= -1 || _pos >= _table.Length)
                    throw new InvalidOperationException();
                return new KeyValuePair<TKey, TValue>(_table[_pos].Key, _table[_pos].Value);
            }
        }

        object IEnumerator.Current => Current;
    }

    internal static class HashCoder
    {
        public static int GetHash(int firstHash, int mod)
        {
            var ret = firstHash ^ 357913941;
            ret %= mod - 1;
            if (ret < 0)
                ret += mod - 1;
            return ret + 1;
        }
    }

    internal static class CapacityGenerator
    {
        private const int MaxCapacity = int.MaxValue / 2; //иначе индексы будут вычисляться с ошибками изза переполнения int

        private static readonly List<int> PrimeNumbers;

        static CapacityGenerator()
        {
            PrimeNumbers = new List<int>();
            var nums = new bool[33000];
            for (var i = 2; i < 182; i++)
                if (nums[i] == false)
                    for (var j = i * i; j < nums.Length; j += i)
                        nums[j] = true;
            for (var i = 2; i < nums.Length; i++)
                if (nums[i] == false)
                    PrimeNumbers.Add(i);
        }

        public static int GetNextCapacity(int capacity)
        {
            var next = capacity * 2;
            next++;
            if (next < 0 || next > MaxCapacity)
                throw new ArgumentOutOfRangeException();
            var ok = true;
            while (ok)
            {
                ok = false;
                foreach (var p in PrimeNumbers)
                {
                    if(next <= p)
                        break;

                    if (next % p == 0)
                    {
                        next+=2;
                        ok = true;
                        break;
                    }
                }
            }

            return next;
        }
    }

    /// <summary>
    /// Используется двойное хэширование:
    /// ind(key, round) = h(key) + round * h`(key);
    /// </summary>
    public class HashTable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private Node<TKey, TValue>[] _table;

        public int Count { get; private set; }

        public int Capacity
        {
            get { return _table.Length; }
        }

        public HashTable()
        {
            _table = new Node<TKey, TValue>[5];
        }

        private int GetNewCapacity()
        {
            return CapacityGenerator.GetNextCapacity(Capacity);
        }

        private int GetHeadIndex(int hashCode)
        {
            var ret = hashCode % Capacity;
            if (ret < 0)
                ret += Capacity;

            return ret;
        }

        private int GetNextIndex(int curIndex, int secondHashCode)
        {
            curIndex += secondHashCode;
            return GetHeadIndex(curIndex);
        }

        private void Add(Node<TKey,TValue> node)
        {
            var keyHashCode = node.KeyHashCode;
            var curIndex = GetHeadIndex(keyHashCode);
            if (_table[curIndex] == null)
            {
                _table[curIndex] = node;
                return;
            }

            if (GetHeadIndex(_table[curIndex].KeyHashCode) != curIndex)
            {
                var curNode = _table[curIndex];
                Remove(curNode.Key, curNode.KeyHashCode);
                _table[curIndex] = node;

                curNode.Next = -1;
                curIndex = GetHeadIndex(curNode.KeyHashCode);
                node = curNode;
                Count++;
            }

            while (_table[curIndex].Next != -1)
            {
                if (_table[curIndex].Key.Equals(node.Key))
                    throw new ArgumentException();

                curIndex = _table[curIndex].Next;
            }

            var lastNode = _table[curIndex];
            var secondHashCode = HashCoder.GetHash(keyHashCode, Capacity);
            while (_table[curIndex] != null)
                curIndex = GetNextIndex(curIndex, secondHashCode);
            lastNode.Next = curIndex;
            _table[curIndex] = node;
        }

        private void ExpandTable()
        {
            var oldTable = _table;
            _table = new Node<TKey, TValue>[GetNewCapacity()];

            foreach (var node in oldTable)
            {
                if (node == null)
                    continue;

                node.Next = -1;
                Add(node);
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException();

            Count++;
            if (Count > Capacity - 1)
                ExpandTable();

            Add(new Node<TKey, TValue>(key, value, key.GetHashCode()));
        }

        private int FindIndex(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException();

            var keyHashCode = key.GetHashCode();
            var curIndex = GetHeadIndex(keyHashCode);

            if (_table[curIndex] == null)
                return -1;

            if (GetHeadIndex(_table[curIndex].KeyHashCode) != curIndex)
                return -1;

            while (curIndex != -1)
            {
                if (_table[curIndex].Key.Equals(key))
                    return curIndex;
                curIndex = _table[curIndex].Next;
            }

            return -1;
        }

        public bool Contains(TKey key)
        {
            var index = FindIndex(key);
            return index != -1;
        }

        private void Remove(TKey key, int keyHashCode)
        {
            var curIndex = GetHeadIndex(keyHashCode);
            if (_table[curIndex] == null)
                return;
            if (GetHeadIndex(_table[curIndex].KeyHashCode) != curIndex)
                return;

            if (_table[curIndex].Key.Equals(key))
            {
                Count--;

                if (_table[curIndex].Next == -1)
                {
                    _table[curIndex] = null;
                    return;
                }

                var curNode = _table[curIndex];
                _table[curIndex] = _table[curNode.Next];
                _table[curNode.Next] = null;
                return;
            }

            var previousIndex = curIndex;
            curIndex = _table[previousIndex].Next;
            while (curIndex != -1)
            {
                if (_table[curIndex].Key.Equals(key))
                {
                    _table[previousIndex].Next = _table[curIndex].Next;
                    _table[curIndex] = null;

                    Count--;
                    return;
                }

                previousIndex = curIndex;
                curIndex = _table[curIndex].Next;
            }
        }

        public void Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException();

            var keyHashCode = key.GetHashCode();
            Remove(key, keyHashCode);
        }

        public TValue this[TKey key]
        {
            get
            {
                var index = FindIndex(key);
                if (index == -1)
                    throw new ArgumentException();

                return _table[index].Value;
            }
            set
            {
                var index = FindIndex(key);
                if (index == -1)
                    throw new ArgumentException();

                _table[index].Value = value;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new HashTableEnumerator<TKey, TValue>(_table);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
