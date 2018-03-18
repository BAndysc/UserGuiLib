using System;

namespace ObservableObjects
{
    public class ObservableProperty<T> : IObservableProperty<T>
    {
        private T value;

        public event Action<T, T> OnChange;

        public ObservableProperty(T initial)
        {
            value = initial;
        }

        public ObservableProperty()
        {
            value = default(T);
        }

        public void set(T value)
        {
            T old = this.value;
            this.value = value;
            OnChange?.Invoke(old, value);
        }

        public T get()
        {
            return value;
        }
    }
}
