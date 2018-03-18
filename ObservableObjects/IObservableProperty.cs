using System;

namespace ObservableObjects
{
    public interface IObservableProperty<T>
    {
        T get();
        void set(T val);

        event Action<T, T> OnChange;
    }
}
