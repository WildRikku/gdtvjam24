using DTerrain;
using Unity.VisualScripting;
using UnityEngine;

public class BattleField : MonoBehaviour
{
    public Sprite BackgroundSprite;

    [DoNotSerialize, HideInInspector]
    public float width;
    [DoNotSerialize, HideInInspector]
    public float height;
    public BasicPaintableLayer collidableLayer;
    public BasicPaintableLayer visibleLayer;
    
    // Start is called before the first frame update
    void Start()
    {
        if (BackgroundSprite != null)
        {
            width = BackgroundSprite.texture.width / BackgroundSprite.pixelsPerUnit;
            height = BackgroundSprite.texture.height / BackgroundSprite.pixelsPerUnit;
        }
    }
}
