using System;

namespace ObservableObjects
{
    public class ObservableProperty<T> : ObservableValue<T>
    {
        private IObservableValue<T> bound;

        public ObservableProperty(T initial) : base(initial)
        {
        }

        public ObservableProperty(T initial, Func<T, T> checker) : base(initial, checker)
        {
        }

        public ObservableProperty() : base()
        {
        }

        private bool inListener = false;
        private bool inThisListener = false;

        public void bind(IObservableValue<T> other)
        {
            other.OnChange += ObservedChanged;
            OnChange += ThisChanged;
            bound = other;
        }

        public void unbind()
        {
            if (bound != null)
            {
                bound.OnChange -= ObservedChanged;
                OnChange -= ThisChanged;
                bound = null;
            }
        }

        private void ObservedChanged(T oldVal, T newVal)
        {
            if (inThisListener)
                return;

            inListener = true;
            set(newVal);
            inListener = false;
        }

        private void ThisChanged(T oldVal, T newVal)
        {
            if (inListener)
                return;

            inThisListener = true;
            bound.set(newVal);
            inThisListener = false;
        }
    }
}
