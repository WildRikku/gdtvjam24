using DTerrain;
using UnityEngine;

public class ExplodeOnImpact : MonoBehaviour
{
    [SerializeField] public BasicPaintableLayer primaryLayer;
    [SerializeField] public BasicPaintableLayer secondaryLayer;

    private PaintingParameters _paintingParameters = new PaintingParameters()
    {
        Color = Color.clear,
        Position = new Vector2Int(),
        Shape = Shape.GenerateShapeCircle(50),
        PaintingMode = PaintingMode.REPLACE_COLOR,
        DestructionMode = DestructionMode.DESTROY
    };

    private void OnCollisionEnter2D(Collision2D col)
    {
        Vector2 position = col.GetContact(0).point;
        _paintingParameters.DestructionMode = DestructionMode.DESTROY;
        _paintingParameters.Position.x = (int)(position.x * primaryLayer.PPU) - 50;
        _paintingParameters.Position.y = (int)(position.y * primaryLayer.PPU) - 50;
        primaryLayer.Paint(_paintingParameters
        );

        _paintingParameters.DestructionMode = DestructionMode.NONE;
        secondaryLayer.Paint(_paintingParameters
        );

        Destroy(gameObject);
    }
}