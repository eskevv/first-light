using FirstLight.TiledModels;
namespace FirstLight;

public class LightTile
{
   public readonly float WorldPositionX;
   public readonly float WorldPositionY;
   public readonly int SourceX;
   public readonly int SourceY;
   public readonly int Width;
   public readonly int Height;
   public readonly string ImageName;
   public readonly LightAnimation[]? FrameData;

   public LightTile(int tileId, TiledTileset tileset, int mapIteration, int layerColumns)
   {
      Width = tileset.TileWidth;
      Height = tileset.TileHeight;
      WorldPositionX = (mapIteration % layerColumns) * tileset.TileWidth;
      WorldPositionY = (mapIteration / layerColumns) * tileset.TileHeight;
      SourceX = (tileId % tileset.Columns) * tileset.TileWidth;
      SourceY = (tileId / tileset.Columns) * tileset.TileHeight;
      ImageName = tileset.Name ?? "0";

      if (tileset.Animations == null || tileset.Image == null) return;
      foreach (var animation in tileset.Animations)
      {
         if (tileId != animation.Id || animation.Frames == null) continue;
         int frames = animation.Frames.Length;
         FrameData = new LightAnimation[frames];

         for (int x = 0; x < frames; x++)
         {
            int srcX = (animation.Frames[x].TiledId % (tileset.Image.Width / tileset.TileWidth)) * tileset.TileWidth;
            int srcY = (animation.Frames[x].TiledId / (tileset.Image.Width / tileset.TileWidth)) * tileset.TileHeight;
            int frameDuration = animation.Frames[x].Duration;
            var frame = new LightAnimation(srcX, srcY, frameDuration);
            FrameData[x] = frame;
         }
      }
   }
}
