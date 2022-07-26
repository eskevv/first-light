namespace FirstLight.TiledModels;

public class TiledAnimation : TiledModel
{
   public readonly int TileId;
   public readonly List<TiledFrame> Frames;

   public TiledAnimation(TiledProperty[] properties, List<TiledFrame> frames)
   {
      TileId = FindIntValue("id", properties);
      Frames = frames;
   }
}