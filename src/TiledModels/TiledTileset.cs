namespace FirstLight;

public class TiledTileset
{
    public int TileWidth;
    public int TileHeight;
    public int TileCount;
    public int Columns;

    public string Name = default!;
    public string Version = default!;
    public string TiledVersion = default!;

    public string? Class;
    public TiledImage[]? ImageCollection;
    public TiledImage? Image;
    public TiledAnimation[]? Animations;
    public TiledProperty[]? CustomProperties;
}