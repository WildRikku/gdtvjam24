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

    private bool _impacted;

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

    private void Update()
    {
        if (transform.position.y < -1)
        {
            Impact?.Invoke(0);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!(col.collider.CompareTag("DestructibleTerrain") || col.collider.CompareTag("Player"))) return;
        
        if (_impacted) return;
        _impacted = true;
        
        Vector2 position = col.GetContact(0).point;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, (float)radius/primaryLayer.PPU, Physics2D.AllLayers);
        bool terrainHit = false;
        foreach (Collider2D c in hitColliders)
        {
            if (c.CompareTag("Player"))
            {
                // Players only have one collider, send them damage
                c.SendMessage(nameof(PlayerController.TakeDamage), damage);
            }
            else if(c.CompareTag("DestructibleTerrain"))
            {
                // Terrain has many colliders, save that we hit it and act later
                terrainHit = true;
            }
        }
        
        if (terrainHit)
        {
            _paintingParameters.DestructionMode = DestructionMode.DESTROY;
            _paintingParameters.Position.x = (int)(position.x * primaryLayer.PPU) - radius;
            _paintingParameters.Position.y = (int)(position.y * primaryLayer.PPU) - radius;
            primaryLayer.Paint(_paintingParameters);
            _paintingParameters.DestructionMode = DestructionMode.NONE;
            secondaryLayer.Paint(_paintingParameters);
        }

        Impact?.Invoke(damage); // TODO: damage is currently meaningless
        Destroy(gameObject);
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