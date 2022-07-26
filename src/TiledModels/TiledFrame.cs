namespace FirstLight.TiledModels;

public class TiledFrame : TiledModel
{
   public readonly int TiledId;
   public readonly int Duration;

   public TiledFrame(TiledProperty[] properties)
   {
      TiledId = FindIntValue("tileid", properties);
      Duration = FindIntValue("duration", properties);
   }
}