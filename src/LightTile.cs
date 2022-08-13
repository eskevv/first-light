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
            ImageName = tileset.Name ?? "0";
            Width = tileset.TileWidth;
            Height = tileset.TileHeight;
            SourceX = (tileId % tileset.Columns) * Width;
            SourceY = (tileId / tileset.Columns) * Height;
        }

        WorldPositionX = (mapIteration % layerColumns) * tileset.TileWidth;
        WorldPositionY = (mapIteration / layerColumns) * tileset.TileHeight;

        if (tileset.Animations == null)
            return;

        foreach (var animation in tileset.Animations)
        {
            int frames = animation.Frames.Length;
            FrameData = new LightAnimation[frames];

            for (int x = 0; x < frames; x++)
            {
                int srcX = (animation.Frames[x].TiledId % (tileset.Image!.Width / tileset.TileWidth)) * tileset.TileWidth;
                int srcY = (animation.Frames[x].TiledId / (tileset.Image!.Width / tileset.TileWidth)) * tileset.TileHeight;
                int frameDuration = animation.Frames[x].Duration;
                var frame = new LightAnimation(srcX, srcY, frameDuration);
                FrameData[x] = frame;
            }
        }
    }
}
