using System;
using System.Linq;

namespace BasicModelInterface
{
    public static class BasicModelInterfaceExtensions
    {
        public static T GetValue<T>(this IBasicModelInterface model, string variable)
        {
            var values = model.GetValues(variable);
            return (T)values.GetValue(0);
        }

        public static T GetValue<T>(this IBasicModelInterface model, string variable, int index)
        {
            var values = model.GetValues(variable);
            return (T)values.GetValue(index);
        }

        public static void SetValue<T>(this IBasicModelInterface model, string variable, T value)
        {
            var length = BasicModelInterfaceLibrary.GetTotalLength(model.GetShape(variable));
            var values = Enumerable.Repeat(value, length).ToArray();

            model.SetValues(variable, values);
        }

        public static void SetValue<T>(this IBasicModelInterface model, string variable, int index, T value)
        {
            var shape = model.GetShape(variable);

            if (shape.Length != 1)
            {
                throw new NotSupportedException("use SetValue(variable, start, count, values)");
            }

            //model.SetValues(variable, new int[] { index }, new int[] { 1 }, new T[] { value });
            model.SetValues(variable, new int[] { index },  new T[] { value });
        }
    }
}