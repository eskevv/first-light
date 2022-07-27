namespace FirstLight.Utils;

public struct Coordinates
{
   public readonly float X;
   public readonly float Y;

   public Coordinates(float x, float y)
   {
      X = x;
      Y = y;
   }

   public override string ToString() => $"({X}, {Y})";
}