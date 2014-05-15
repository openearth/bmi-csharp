using System;

namespace BasicModelInterface
{
    /// <summary>
    /// Array allocated in native code (C/C++ conventions).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NativeArray<T> : IArray<T>
    {
        private IntPtr values;

        public NativeArray(IntPtr values, int rank, int[] shape)
        {
            this.values = values;
            Rank = rank;
            Shape = shape;
        }

        public T this[params int[] index]
        {
            get { return GetValue(index); }
            set { SetValue(index, value); }
        }

        public int[] Shape { get; private set; }
        
        public int Rank { get; private set; }

        unsafe private void SetValue(int[] index, T value)
        {
            throw new NotImplementedException();
        }

        unsafe private T GetValue(int[] index) 
        {
            var array = (double*)values;
            return (T)(object)array[index[0]];
        }

        unsafe private double GetDoubleValue(int[] index)
        {
            var array = (double*)values;
            return array[index[0]];
        }
    }
}