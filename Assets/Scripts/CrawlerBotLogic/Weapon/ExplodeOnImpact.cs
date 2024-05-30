using DTerrain;
using UnityEngine;

public class ExplodeOnImpact : MonoBehaviour
{
    [SerializeField] public BasicPaintableLayer primaryLayer;
    [SerializeField] public BasicPaintableLayer secondaryLayer;
    public GameObject explosionPrefab;

    public int radius = 60;

    private PaintingParameters _paintingParameters = new()
    {
        Color = Color.clear,
        Position = new Vector2Int(),
        PaintingMode = PaintingMode.REPLACE_COLOR,
        DestructionMode = DestructionMode.DESTROY
    };

    private void Start()
    {
        _paintingParameters.Shape = Shape.GenerateShapeCircle(radius);
    }

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

    private void OnDestroy()
    {
        if (explosionPrefab != null)
        {
            Transform transform1 = transform;
            Instantiate(explosionPrefab, transform1.position, transform1.rotation);
        }
    }
}