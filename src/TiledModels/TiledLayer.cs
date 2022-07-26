namespace FirstLight.TiledModels;

public class TiledLayer : TiledModel
{
   public readonly string Name;
   public readonly int[] Data;
   public readonly int Width;
   public readonly int Height;

   public TiledLayer(TiledProperty[] properties, int[] data)
   {
      Name = FindStringValue("name", properties);
      Width = FindIntValue("width", properties);
      Height = FindIntValue("height", properties);

      Data = data;
   }
}