using DTerrain;
using UnityEngine;

public class ExplodeOnImpact : MonoBehaviour
{
    [SerializeField]
    protected BasicPaintableLayer primaryLayer;
    [SerializeField]
    protected BasicPaintableLayer secondaryLayer;

    private void OnCollisionEnter2D(Collision2D col) 
    {
        Debug.Log("BdOOM");
        int circleSize = 50;
        Shape destroyCircle = Shape.GenerateShapeCircle(circleSize);

        Vector2 position = col.GetContact(0).point;
        primaryLayer?.Paint(new PaintingParameters() 
        {
            Color = Color.clear, 
            Position = new Vector2Int((int)(position.x * primaryLayer.PPU) - circleSize, (int)(position.y * primaryLayer.PPU) - circleSize), 
            Shape = destroyCircle, 
            PaintingMode=PaintingMode.REPLACE_COLOR,
            DestructionMode = DestructionMode.DESTROY
        });

        secondaryLayer?.Paint(new PaintingParameters() 
        {
            Color = Color.clear,
            Position = new Vector2Int((int)(position.x * secondaryLayer.PPU) - circleSize, (int)(position.y * secondaryLayer.PPU) - circleSize), 
            Shape = destroyCircle, 
            PaintingMode=PaintingMode.REPLACE_COLOR,
            DestructionMode = DestructionMode.NONE
        });
        
        Destroy(gameObject);
    }
}
