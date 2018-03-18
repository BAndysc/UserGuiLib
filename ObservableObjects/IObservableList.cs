using System;
using System.Collections.Generic;

namespace ObservableObjects
{
    public interface IObservableList<T> : IList<T>
    {
        event Action<T> OnRemoveItem;
        event Action<T, int> OnInsertItem;
    }
}
