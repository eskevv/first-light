namespace FirstLight;

public struct LightAnimation
{
   public int SourceX;
   public int SourceY;
   public int Duration;

   public LightAnimation(int sourceX, int sourceY, int duration)
   {
      SourceX = sourceX;
      SourceY = sourceY;
      Duration = duration;
   }
}
