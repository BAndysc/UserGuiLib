using System;

namespace ObservableObjects
{
    public interface IObservableValue<T>
    {
        T get();
        void set(T val);

        event Action<T, T> OnChange;
    }
}
