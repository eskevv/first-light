namespace FirstLight;

public static class Helpers
{
    public static T[] CombineArrays<T>(T[] first, T[] second)
    {
        var final = new T[first.Length + second.Length];
        first.CopyTo(final, 0);
        second.CopyTo(final, first.Length);
        return final;
    }
}