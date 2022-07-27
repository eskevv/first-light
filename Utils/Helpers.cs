namespace FirstLight.Utils;

public static class Helpers
{
   public static int EnsureInteger(string? value)
   {
      if (value == null) throw new FirstLightException($"The given value was null.");
      return int.Parse(value);
   }

   public static string EnsureString(string? value)
   {
      if (value == null) throw new FirstLightException($"The given value was null.");
      return value;
   }

   public static T[] CombineArrays<T>(T[] first, T[] second)
   {
      var final = new T[first.Length + second.Length];
      first.CopyTo(final, 0);
      second.CopyTo(final, first.Length);
      return final;
   }
}