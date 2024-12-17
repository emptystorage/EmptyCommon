using System;

namespace EmptyCommon
{
    public sealed class ReactValue<T>
    {
        private T _value;

        public ReactValue(T value = default)
        {
            _value = value;
            ValueChanged += _ => { };
        }

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                ValueChanged.Invoke(_value);
            }
        }

        public event Action<T> OnValueChanged
        {
            add
            {
                ValueChanged += value;
                ValueChanged.Invoke(_value);
            }
            remove
            {
                ValueChanged -= value;
            }
        }

        private event Action<T> ValueChanged;
    }
}
