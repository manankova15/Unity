namespace Utils
{
    public static class ArrayUtils
    {
        public static T[] InitializeArray<T>(int length, T defaultValue)
        {
            var array = new T[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = defaultValue;
            }
            return array;
        }
    }
}