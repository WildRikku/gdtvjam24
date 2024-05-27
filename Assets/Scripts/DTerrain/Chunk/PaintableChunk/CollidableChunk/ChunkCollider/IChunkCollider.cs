using System.Collections.Generic;

namespace DTerrain
{
    public interface IChunkCollider
    {
        void UpdateColliders(List<Column> pixelData, ITextureSource textureSource);
    }
}
