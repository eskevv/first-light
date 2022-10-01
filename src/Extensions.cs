namespace NetTools;

public static class Extensions
{
    public static int[] ToIntArray(this string src)
    {
        return src.Split(',').Select(x => int.Parse(x)).ToArray();
    }

    public static string CombineWithPath(this string src, string path)
    {
        int index = path.LastIndexOf('/');
        return path.Substring(0, index + 1) + src;
    }

    public static string FileName(this string src)
    {
        int findIndex = src.LastIndexOf('/');
        int findExtnesion = src.LastIndexOf('.');
        int index = findIndex == -1 ? 0 : findIndex + 1;
        int extension = findExtnesion == -1 ? src.Length : findExtnesion;
        
        return src.Substring(index, extension - index);
    }
}