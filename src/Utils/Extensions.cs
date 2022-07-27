namespace FirstLight.Utils;

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
}