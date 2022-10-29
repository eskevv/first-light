using NetTools;

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
    public readonly string ImageSource;
    public readonly LightAnimation[]? FrameData;

    public LightTile(int tileId, TiledTileset tileset, int mapIteration, TiledTileLayer layer)
    {
        if (tileset.Image == null && tileId >= 0)
        {
            var tiledImage = tileset.ImageCollection!.First(x => x.Id == tileId);
            ImageName = tiledImage.Source.FileName();
            Width = tiledImage.Width;
            Height = tiledImage.Height;
            SourceX = 0;
            SourceY = 0;
        }
        else
        {
            ImageName = tileset.Name;
            Width = tileset.TileWidth;
            Height = tileset.TileHeight;
            SourceX = (tileId % tileset.Columns) * Width;
            SourceY = (tileId / tileset.Columns) * Height;
        }

        WorldPositionX = (mapIteration % layer.Width) * layer.TileWidth;
        WorldPositionY = (mapIteration / layer.Height) * layer.TileHeight;


        if (tileset.Columns == 0)
        {
            WorldPositionY += layer.TileHeight - Height;
            ImageSource = tileset.ImageCollection![tileId].Source;
        }
        else
        {
            ImageSource = tileset.Image!.Source;
        }

        if (tileset.Animations == null)
            return;

        foreach (var animation in tileset.Animations)
        {
            int frames = animation.Frames.Length;
            FrameData = new LightAnimation[frames];

            for (int x = 0; x < frames; x++)
            {
                Console.WriteLine(tileset.Image!.Source!);
                int srcX = (animation.Frames[x].TiledId % (tileset.Image!.Width / tileset.TileWidth)) * tileset.TileWidth;
                int srcY = (animation.Frames[x].TiledId / (tileset.Image!.Width / tileset.TileWidth)) * tileset.TileHeight;
                int frameDuration = animation.Frames[x].Duration;
                var frame = new LightAnimation(srcX, srcY, frameDuration);
                FrameData[x] = frame;
            }
        }
    }
}
