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
        IObservableValue<ITreeNode<T>> Left { get; }
        IObservableValue<ITreeNode<T>> Right { get; }

        T Value { get; }

        bool Insert(T element);
    }

    public interface ITree<T> where T : IComparable
    {
        IObservableValue<ITreeNode<T>> Root { get; }

        void Insert(T element);
        bool Contains(T element);
    }
}
