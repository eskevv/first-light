using FirstLight.TiledModels;
using FirstLight.Utils;
namespace FirstLight;

public class MapTile : MapElement
{
   public readonly int Width;
   public readonly int Height;
   
   public readonly string ImageName;

   /// <summary>Returns true if this tile is animated.</summary>
   public bool IsAnimated => AnimationPoints != null;
   /// <summary>
   /// <br>The y coordinate of the image this tile should render from.</br>
   /// <br>Caution! Getting this field will automatically advance the tile frame.</br>
   ///</summary>
   public int ImageSourceX => RetrieveImageSourceX();
   /// <summary>
   /// <br>The y coordinate of the image this tile should render from.</br>
   /// <br>Caution! Getting this field will automatically advance the tile frame.</br>
   ///</summary>
   public int ImageSourceY => RetrieveImageSourceY();

   private int _currentFrame;
   private readonly List<Coordinate>? AnimationPoints;
   private readonly int OriginalSourceX;
   private readonly int OriginalSourceY;

   private int RetrieveImageSourceX()
   {
      if (AnimationPoints == null) return OriginalSourceX;
      _currentFrame %= AnimationPoints.Count;
      return (int)AnimationPoints[_currentFrame++].X;
   }

   private int RetrieveImageSourceY()
   {
      if (AnimationPoints == null) return OriginalSourceY;
      _currentFrame %= AnimationPoints.Count;
      return (int)AnimationPoints[_currentFrame++].Y;
   }

   public MapTile(string layerName, TiledTileset tileSet, int tileGid, int iteration, int columns) : base(layerName)
   {
      int srcId = tileGid - tileSet.FirstGid;

      Width = tileSet.TileWidth;
      Height = tileSet.TileHeight;
      WorldPositionX = (iteration % columns) * tileSet.TileWidth;
      WorldPositionY = (iteration / columns) * tileSet.TileHeight;
      OriginalSourceX = (srcId % (tileSet.Image.Width / tileSet.TileWidth)) * tileSet.TileWidth;
      OriginalSourceY = (srcId / (tileSet.Image.Width / tileSet.TileWidth)) * tileSet.TileHeight;
      ImageName = tileSet.Name;
      ParentLayer = layerName;

      foreach (var animation in tileSet.Animations)
      {
         if (srcId != animation.TileId) continue;
         AnimationPoints = new List<Coordinate>();
         foreach (var frame in animation.Frames)
         {
            int srcX = (frame.TiledId % (tileSet.Image.Width / tileSet.TileWidth)) * tileSet.TileWidth;
            int srcY = (frame.TiledId / (tileSet.Image.Width / tileSet.TileWidth)) * tileSet.TileHeight;
            AnimationPoints.Add(new Coordinate(srcX, srcY));
         }
      }
   }
}