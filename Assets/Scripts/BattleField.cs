using DTerrain;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class BattleField : MonoBehaviour
{
    [FormerlySerializedAs("BackgroundSprite")] public Sprite backgroundSprite;

    [DoNotSerialize, HideInInspector]
    public float width;
    [DoNotSerialize, HideInInspector]
    public float height;
    public BasicPaintableLayer collidableLayer;
    public BasicPaintableLayer visibleLayer;
    
    // Start is called before the first frame update
    void Start()
    {
        if (backgroundSprite != null)
        {
            width = backgroundSprite.texture.width / backgroundSprite.pixelsPerUnit;
            height = backgroundSprite.texture.height / backgroundSprite.pixelsPerUnit;
        }
    }
}
