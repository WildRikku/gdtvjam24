namespace DTerrain {
    public interface IChildChunk : IChunk {
        IChunk Parent { get; set; }
    }
}