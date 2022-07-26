using FirstLight.TiledModels;
using FirstLight.Utils;
namespace FirstLight;

public class MapTile : MapElement
{
   /// <summary>The width in pixels for this tile.</summary>
   public readonly int Width;
   /// <summary>The height in pixels for this tile.</summary>
   public readonly int Height;
   /// <summary>The name of the image corresponding to this tile's tileset.</summary>
   public readonly string ImageName;
   /// <summary>Is true if the tile is animated.</summary>
   public readonly bool IsAnimated;
   /// 
   /// <summary>The x coordinate of the image in pixels this tile is rendered from.</summary>
   public int SourceX => GetImageSourceX();
   /// <summary>The y coordinate of the image in pixels this tile is rendered from.</summary>
   public int SourceY => GetImageSourceY();

   private int _currentFrame;
   private float _frameTime;

   private readonly int _originalSourceX;
   private readonly int _originalSourceY;
   private readonly GameMap _world;
   private readonly Coordinates[]? AnimationPoints;
   private readonly int[]? FrameDurations;

   public MapTile(string layerName, TiledTileset tileSet, int tileGid, int iteration, int columns, GameMap world) : base(layerName)
   {
      int srcId = tileGid - tileSet.FirstGid;
      _world = world;

      Width = tileSet.TileWidth;
      Height = tileSet.TileHeight;
      WorldPositionX = (iteration % columns) * tileSet.TileWidth;
      WorldPositionY = (iteration / columns) * tileSet.TileHeight;
      _originalSourceX = (srcId % (tileSet.Image.Width / tileSet.TileWidth)) * tileSet.TileWidth;
      _originalSourceY = (srcId / (tileSet.Image.Width / tileSet.TileWidth)) * tileSet.TileHeight;
      ImageName = tileSet.Name;
      ParentLayer = layerName;

      foreach (var animation in tileSet.Animations)
      {
         if (srcId != animation.TileId) continue;
         IsAnimated = true;
         int frames = animation.Frames.Count;
         AnimationPoints = new Coordinates[frames];
         FrameDurations = new int[frames];
         for (int x = 0; x < frames; x++)
         {
            int srcX = (animation.Frames[x].TiledId % (tileSet.Image.Width / tileSet.TileWidth)) * tileSet.TileWidth;
            int srcY = (animation.Frames[x].TiledId / (tileSet.Image.Width / tileSet.TileWidth)) * tileSet.TileHeight;
            AnimationPoints[x] = new Coordinates(srcX, srcY);
            FrameDurations[x] = animation.Frames[x].Duration;
         }
      }
   }

   private int GetImageSourceX()
   {
      if (AnimationPoints == null || FrameDurations == null) return _originalSourceX;
      return (int)AnimationPoints[_currentFrame].X;
   }

   private int GetImageSourceY()
   {
      if (AnimationPoints == null || FrameDurations == null) return _originalSourceY;
      return (int)AnimationPoints[_currentFrame].Y;
   }


   /// <summary>The update method called by GameMap. You should not have to call this yourself.</summary>
   public void Update()
   {
      if (AnimationPoints == null || FrameDurations == null) return;
      _frameTime += _world.TimePassed * 1000;

      if (_frameTime < FrameDurations[_currentFrame]) return;
      _currentFrame++;
      _currentFrame %= AnimationPoints.Length;
      _frameTime = 0f;
   }
}