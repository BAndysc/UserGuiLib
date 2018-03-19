using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableObjects
{
    public class ObservableValue<T> : IObservableValue<T>
    {
        private T value;
        private Func<T, T> checker = null;

        public event Action<T, T> OnChange;
        
        public ObservableValue()
        {
            value = default(T);
        }

        public ObservableValue(T initial)
        {
            value = initial;
        }

        public ObservableValue(T initial, Func<T, T> checker)
        {
            value = initial;
            this.checker = checker;
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
