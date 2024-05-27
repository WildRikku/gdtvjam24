namespace DTerrain
{
    public interface ITexturedChunk :IChunk
    {
        int SortingLayerID { get; set; }
        ITextureSource TextureSource { get; }
    }
}

