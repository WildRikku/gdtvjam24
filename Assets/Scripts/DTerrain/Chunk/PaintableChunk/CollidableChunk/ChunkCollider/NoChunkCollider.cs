using System.Collections.Generic;
using UnityEngine;

namespace DTerrain
{
    public class NoChunkCollider : MonoBehaviour, IChunkCollider
    {

        public void UpdateColliders(List<Column> pixelData, ITextureSource textureSource)
        {
            //Doesnt do anything.
        }

    }
}
