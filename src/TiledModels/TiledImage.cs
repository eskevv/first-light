namespace FirstLight.TiledModels;

public class TiledImage : TiledModel
{
   public readonly int Width;
   public readonly int Height;
   public readonly string Source;

   public TiledImage(TiledProperty[] properties)
   {
      Width = FindIntValue("width", properties);
      Height = FindIntValue("height", properties);
      Source = FindStringValue("source", properties);
   }
}