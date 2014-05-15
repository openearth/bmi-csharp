namespace BasicModelInterface
{
    public interface IArray<T>
    {
        T this[params int[] index] { get; set; }

        int[] Shape { get; }

        int Rank { get; }
    }
}