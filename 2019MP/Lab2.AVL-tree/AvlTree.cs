using System;

namespace Lab2.AVL_tree
{
    internal class Node<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public Node<TKey, TValue> Right, Left, Parent;

        public int Height { get; set; }

        public Node(TKey key, TValue value, Node<TKey,TValue> parent = null)
        {
            Key = key;
            Value = value;
            Parent = parent;
        }
    }

    public class AvlTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private Node<TKey, TValue> _root;

        public AvlTree()
        {
            _root = null;
        }

        public int Count { get; private set; }

        private static void Add(Node<TKey, TValue> root, Node<TKey, TValue> newNode)
        {
            while (true)
            {
                var cmp = root.Key.CompareTo(newNode.Key);

                if (cmp == 0)
                    throw new ArgumentException();

                if (cmp == -1)
                {
                    if (root.Right is null)
                    {
                        root.Right = newNode;
                        newNode.Parent = root;
                        return;
                    }

                    root = root.Right;
                }
                else
                {
                    if (root.Left is null)
                    {
                        root.Left = newNode;
                        newNode.Parent = root;
                        return;
                    }

                    root = root.Left;
                }
            }
        }

        private static void UpdateHeight(Node<TKey, TValue> node)
        {
            int leftH, rightH;

            while (node is null == false)
            {
                if (node.Left is null)
                    leftH = 0;
                else
                    leftH = node.Left.Height + 1;

                if (node.Right is null)
                    rightH = 0;
                else
                    rightH = node.Right.Height + 1;

                node.Height = leftH > rightH ? leftH : rightH;

                node = node.Parent;
            }
        }

        private static void ReplaceParent(Node<TKey, TValue> node, Node<TKey, TValue> parent)
        {
            if(node is null)
                return;
            node.Parent = parent;
        }

        private void RightRotation(Node<TKey, TValue> node)
        {
            var nodeL = node.Left;
            ReplaceParent(nodeL, node.Parent);

            ReplaceChild(node.Parent, node, nodeL);

            node.Left = nodeL.Right;
            ReplaceParent(node.Left, node);

            nodeL.Right = node;
            ReplaceParent(node, nodeL);
        }

        private void LeftRotation(Node<TKey, TValue> node)
        {
            var nodeR = node.Right;
            ReplaceParent(nodeR, node.Parent);

            ReplaceChild(node.Parent, node, nodeR);

            node.Right = nodeR.Left;
            ReplaceParent(node.Right, node);

            nodeR.Left = node;
            ReplaceParent(node, nodeR);
        }

        private void BalanceNode(Node<TKey, TValue> node)
        {
            int lh, rh, llh, lrh, rlh, rrh;

            lh = node.Left?.Height + 1 ?? 0;
            rh = node.Right?.Height + 1 ?? 0;

            if (lh - rh == 2)
            {
                llh = node.Left.Left?.Height+1 ?? 0;
                rlh = node.Left.Right?.Height+1 ?? 0;

                if (llh > rlh)
                {
                    RightRotation(node);
                    UpdateHeight(node);
                }
                else
                {
                    var nodeL = node.Left;
                    LeftRotation(nodeL);
                    UpdateHeight(nodeL);
                    RightRotation(node);
                    UpdateHeight(node);
                }

                return;
            }

            if (rh - lh == 2)
            {
                lrh = node.Right.Left?.Height+1 ?? 0;
                rrh = node.Right.Right?.Height+1 ?? 0;

                if (rrh > lrh)
                {
                    LeftRotation(node);
                    UpdateHeight(node);
                }
                else
                {
                    var nodeR = node.Right;
                    RightRotation(nodeR);
                    UpdateHeight(nodeR);
                    LeftRotation(node);
                    UpdateHeight(node);
                }
            }
        }

        private void BalanceTree(Node<TKey, TValue> node)
        {
            while (node.Equals(_root) == false)
            {
                BalanceNode(node);
                node = node.Parent;
            }  
            
            BalanceNode(_root);
            if (_root.Parent is null == false)
                _root = _root.Parent;
        }

        public void Add(TKey key, TValue value)
        {
            var newNode = new Node<TKey, TValue>(key, value);
            Count++;

            if (_root is null)
            {
                _root = newNode;
                return;
            }
            
            Add(_root, newNode);
            UpdateHeight(newNode);
            BalanceTree(newNode);
        }

        private Node<TKey, TValue> LowerBound(TKey key)
        {
            var cur = _root;
            var ret = cur;

            while (true)
            {
                if (cur.Key.CompareTo(key) == -1)
                {
                    if (cur.Right is null)
                        break;
                    cur = cur.Right;
                }
                else
                {
                    ret = cur;
                    if (cur.Left is null)
                        break;
                    cur = cur.Left;
                }
            }

            return ret;
        }

        private void ReplaceChild(Node<TKey, TValue> node, Node<TKey, TValue> child, Node<TKey, TValue> replacement)
        {
            if (node is null == false)
                if (child.Equals(node.Left))
                    node.Left = replacement;
                else
                    node.Right = replacement;
            else
                _root = replacement;
            
            ReplaceParent(replacement,node);
        }

        private static Node<TKey, TValue> FindReplacement(Node<TKey, TValue> node)
        {
            node = node.Right;

            while (node.Left is null == false)
                node = node.Left;

            return node;
        }

        private void Remove(Node<TKey, TValue> node)
        {
            var parent = node.Parent;
            var left = node.Left;
            var right = node.Right;
            if ((left is null || right is null) == false)
            {
                var repl = FindReplacement(node);
                Remove(repl);
                parent = node.Parent;
                left = node.Left;
                right = node.Right;
                ReplaceChild(parent, node, repl);
                repl.Height = node.Height;
                repl.Left = left;
                ReplaceParent(repl.Left,repl);
                repl.Right = right;
                ReplaceParent(repl.Right, repl);
            }
            else
            {
                var child = left ?? right;

                ReplaceChild(parent, node, child);
                if (parent is null == false)
                {
                    UpdateHeight(parent);
                    BalanceTree(parent);
                }
            }
        }

        public bool Remove(TKey key)
        {
            if (_root is null)
                return false;

            var node = LowerBound(key);

            if (node.Key.CompareTo(key) != 0)
                return false;

            Remove(node);
            Count--;
            return true;
        }

        public bool ContainsKey(TKey key)
        {
            if (_root is null)
                return false;
            return LowerBound(key).Key.CompareTo(key) == 0;
        }

        public TValue this[TKey key]
        {
            get
            {
                var node = LowerBound(key);
                if(node.Key.CompareTo(key)!=0)
                    throw new ArgumentException();

                return node.Value;
            }
            set
            {
                var node = LowerBound(key);
                if (node.Key.CompareTo(key) != 0)
                    throw new ArgumentException();

                node.Value = value;
            }
        }

        private string Print(Node<TKey, TValue> node)
        {
            var res = "";
            if (node.Left is null == false)
                res = Print(node.Left);
            res += String.Format("{0} {1}\n", node.Key, node.Value);
            if (node.Right is null == false)
                res += Print(node.Right);

            return res;
        }

        public string Print()
        {
            if(_root is null == false)
                return Print(_root);
            return "";
        }
    }
}
