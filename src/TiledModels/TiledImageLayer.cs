namespace FirstLight;

public class TiledImageLayer
{
    public int Id;
    public float OffsetX;
    public float OffsetY;
    public float ParallaxX;
    public float ParallaxY;
    public bool RepeatX;
    public bool RepeatY;

    public string Name = default!;

    public string? Class;
    public TiledProperty[]? CustomProperties;
}