using ObservableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserGuiLib.ExampleCommon
{
    public interface ITreeNode<T> where T : IComparable
    {
        IObservableProperty<ITreeNode<T>> Left { get; }
        IObservableProperty<ITreeNode<T>> Right { get; }

        T Value { get; }

        bool Insert(T element);
    }

    public interface ITree<T> where T : IComparable
    {
        IObservableProperty<ITreeNode<T>> Root { get; }

        void Insert(T element);
        bool Contains(T element);
    }
}
