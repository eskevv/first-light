namespace FirstLight;

public class Layer
{
   public readonly string Name;
   public readonly int ElementCount;

   public Layer(string name, int elementCount)
   {
      Name = name;
      ElementCount = elementCount;
   }
}