using System;

namespace ObservableObjects
{
    public class ObservableProperty<T> : IObservableProperty<T>
    {
        private T value;
        private Func<T, T> checker = null;

        public event Action<T, T> OnChange;

        public ObservableProperty(T initial)
        {
            value = initial;
        }

        public ObservableProperty(T initial, Func<T, T> checker)
        {
            value = initial;
            this.checker = checker;
        }

        public ObservableProperty()
        {
            value = default(T);
        }

        public void set(T value)
        {
            T old = this.value;
            if (checker != null)
                value = checker(value);
            this.value = value;
            OnChange?.Invoke(old, value);
        }

        public T get()
        {
            return value;
        }
    }
}
