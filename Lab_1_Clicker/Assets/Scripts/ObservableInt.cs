using System;

namespace Scripts
{
    public class ObservableInt
    {
        private int value;
        public Action<int> OnValueChanged;

        public int Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnValueChanged?.Invoke(value);
            }
        }

        public ObservableInt(int initialValue)
        {
            value = initialValue;
        }
    }
}
