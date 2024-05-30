using DTerrain;
using UnityEngine;
using UnityEngine.Serialization;

public delegate void Impact(float damage);

public class ExplodeOnImpact : MonoBehaviour
{
    [SerializeField] public BasicPaintableLayer primaryLayer;
    [SerializeField] public BasicPaintableLayer secondaryLayer;
    [FormerlySerializedAs("explosionPrefab")] public GameObject explosionFXPrefab;

    public int radius = 60;
    public float damage;

    public Collider2D projectileCollider;
    public event Impact Impact;

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
        if (col.collider.CompareTag("DestructibleTerrain"))
        {
            Vector2 position = col.GetContact(0).point;
            _paintingParameters.DestructionMode = DestructionMode.DESTROY;
            _paintingParameters.Position.x = (int)(position.x * primaryLayer.PPU) - radius;
            _paintingParameters.Position.y = (int)(position.y * primaryLayer.PPU) - radius;
            primaryLayer.Paint(_paintingParameters);

            _paintingParameters.DestructionMode = DestructionMode.NONE;
            secondaryLayer.Paint(_paintingParameters);
            Impact?.Invoke(0);
            Destroy(gameObject);
        }
        else if (col.collider.CompareTag("Player"))
        {
            PlayerController hitPlayer = col.collider.gameObject.GetComponent<PlayerController>();
            hitPlayer.TakeDamage(damage);
            Impact?.Invoke(damage);
            Destroy(gameObject);
        }

        // TODO: Handle falling out of the map
    }

    private void OnDestroy()
    {
        if (explosionFXPrefab != null)
        {
            Transform transform1 = transform;
            Instantiate(explosionFXPrefab, transform1.position, transform1.rotation);
        }
    }
}