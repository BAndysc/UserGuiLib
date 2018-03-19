using ObservableObjects;
using System;

namespace UserGuiLib.ExampleCommon
{
    public class TreeNode<T> : ITreeNode<T> where T : IComparable
    {
        public T Value { get; }
        
        public IObservableProperty<ITreeNode<T>> Left { get; }
        public IObservableProperty<ITreeNode<T>> Right { get; }

        public TreeNode(T value)
        {
            Value = value;
            Left = new ObservableProperty<ITreeNode<T>>(null);
            Right = new ObservableProperty<ITreeNode<T>>(null);
        }

        public bool Insert(T element)
        {
            if (Value.CompareTo(element) == 0)
                return false;

            if (Value.CompareTo(element) > 0)
            {
                if (Left.get() == null)
                    Left.set(new TreeNode<T>(element));
                else
                    Left.get().Insert(element);
            }
            else
            {
                if (Right.get() == null)
                    Right.set(new TreeNode<T>(element));
                else
                    Right.get().Insert(element);
            }

            return true;
        }
    }

    public class BSTTree<T> : ITree<T> where T : IComparable
    {
        public IObservableProperty<ITreeNode<T>> Root { get; private set; }

        public BSTTree()
        {
            Root = new ObservableProperty<ITreeNode<T>>(null);
        }

        public void Insert(T element)
        {
            if (Root.get() == null)
                Root.set(new TreeNode<T>(element));
            else
                Root.get().Insert(element);
        }

        public bool Contains(T element)
        {
            throw new NotImplementedException();
        }

        private void ApplyInfixNode(Action<T> function, ITreeNode<T> element)
        {
            if (element.Left.get() != null)
                ApplyInfixNode(function, element.Left.get());

            function(element.Value);

            if (element.Right.get() != null)
                ApplyInfixNode(function, element.Right.get());
        }

        public void ApplyInfix(Action<T> function)
        {
            ApplyInfixNode(function, Root.get());
        }
    }
}
