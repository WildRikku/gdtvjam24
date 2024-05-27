using DTerrain;
using UnityEngine;
using UnityEngine.Serialization;

public class BattleField : MonoBehaviour
{
    [FormerlySerializedAs("OriginalTexture")] public Sprite BackgroundSprite;

    public float _width;
    public float _height;
    public BasicPaintableLayer collidableLayer;
    public BasicPaintableLayer visibleLayer;
    
    // Start is called before the first frame update
    void Start()
    {
        if (BackgroundSprite != null)
        {
            _width = BackgroundSprite.texture.width / BackgroundSprite.pixelsPerUnit;
            _height = BackgroundSprite.texture.height / BackgroundSprite.pixelsPerUnit;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
