using DTerrain;
using UnityEngine;

public class ExplodeOnImpact : MonoBehaviour
{
    [SerializeField] public BasicPaintableLayer primaryLayer;
    [SerializeField] public BasicPaintableLayer secondaryLayer;

    [SerializeField] public static int radius = 50;

    private PaintingParameters _paintingParameters = new PaintingParameters()
    {
        Color = Color.clear,
        Position = new Vector2Int(),
        Shape = Shape.GenerateShapeCircle(radius),
        PaintingMode = PaintingMode.REPLACE_COLOR,
        DestructionMode = DestructionMode.DESTROY
    };

    private void OnCollisionEnter2D(Collision2D col)
    {
        Vector2 position = col.GetContact(0).point;
        _paintingParameters.DestructionMode = DestructionMode.DESTROY;
        _paintingParameters.Position.x = (int)(position.x * primaryLayer.PPU) - radius;
        _paintingParameters.Position.y = (int)(position.y * primaryLayer.PPU) - radius;
        primaryLayer.Paint(_paintingParameters
        );

        _paintingParameters.DestructionMode = DestructionMode.NONE;
        secondaryLayer.Paint(_paintingParameters
        );

        Destroy(gameObject);
    }
}