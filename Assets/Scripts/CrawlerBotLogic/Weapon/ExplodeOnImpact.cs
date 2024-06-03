using CrawlerBotLogic.Weapon;
using DTerrain;
using UnityEngine;
using UnityEngine.Serialization;

public delegate void Impact(float damage);

public class ExplodeOnImpact : MonoBehaviour {
    [SerializeField]
    public BasicPaintableLayer primaryLayer;
    [SerializeField]
    public BasicPaintableLayer secondaryLayer;
    public GameObject explosionFXPrefab;

    public int radius = 60;
    public float damage;

    public event Impact Impact;

    private bool _impacted;

    private bool _canExplode = true;
    public float destroyTimer;

    public string[] impactSounds;
    public string timerAlertSound;
    private int _timerAlertSoundCount;
    private readonly string[] _bounceSounds = { "MouseHover1", "MouseHover2", "MouseHover3" };
    private int _boundsSoundCount;
    private bool _canTriggerBounceSound = true;
    private bool _hasExploded;

    private PaintingParameters _paintingParameters = new() {
        Color = Color.clear,
        Position = new(),
        PaintingMode = PaintingMode.REPLACE_COLOR,
        DestructionMode = DestructionMode.DESTROY
    };

    protected void Start() {
        _paintingParameters.Shape = Shape.GenerateShapeCircle(radius);

        if (destroyTimer > 0) {
            _canExplode = false;
            Invoke(nameof(ExplodeAfterTimer), destroyTimer);
        }

        if (timerAlertSound != "") {
            InvokeRepeating(nameof(TriggerTimerAlertSond), 1f, 1f);
        }
    }

    private void Update() {
        if (transform.position.y < -1) {
            Impact?.Invoke(0);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("WaterBorder")) {
            AudioManager.Instance.PlaySFX("Splash");
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D col) {
        TriggerBounceSound();
        if (_canExplode == false) {
            return;
        }

        if (!(col.collider.CompareTag("DestructibleTerrain") || col.collider.CompareTag("DestructibleObjects") ||
              col.collider.CompareTag("Player"))) {
            return;
        }

        Vector2 position = col.GetContact(0).point;
        Explode(position);
        
        _impacted = true;
    }

    private void SpawnExplosion() {
        if (_hasExploded == false) {
            if (explosionFXPrefab != null) {
                Transform transform1 = transform;
                Instantiate(explosionFXPrefab, transform1.position, transform1.rotation);
            }

            _hasExploded = true;
        }
    }

    private void TriggerBounceSound() {
        if (impactSounds.Length > 0) {
            if (_boundsSoundCount == 0) {
                int ran = Random.Range(0, impactSounds.Length);
                AudioManager.Instance.PlaySFX(impactSounds[ran]);
                _boundsSoundCount = 1;
            }
            else if (_boundsSoundCount <= 6 && _canTriggerBounceSound == true) {
                int ran = Random.Range(0, _bounceSounds.Length);
                AudioManager.Instance.PlaySFX(_bounceSounds[ran]);
                _canTriggerBounceSound = false;
                _boundsSoundCount++;
            }
        }
        else if (_boundsSoundCount <= 6 && _canTriggerBounceSound == true) {
            int ran = Random.Range(0, _bounceSounds.Length);
            AudioManager.Instance.PlaySFX(_bounceSounds[ran]);
            _canTriggerBounceSound = false;
            _boundsSoundCount++;
        }

        Invoke(nameof(ResetBounceSound), 0.1f);
    }

    private void ResetBounceSound() {
        _canTriggerBounceSound = true;
    }

    private void TriggerTimerAlertSond() {
        _timerAlertSoundCount++;
        if (_timerAlertSoundCount < destroyTimer) {
            AudioManager.Instance.PlaySFX(timerAlertSound);
        }
    }

    private void ExplodeAfterTimer() {
        Explode(transform.position);
    }

    protected void Explode(Vector3 pos) {
        Collider2D[] hitColliders =
            Physics2D.OverlapCircleAll(pos, (float)radius / primaryLayer.PPU, Physics2D.AllLayers);
        bool terrainHit = false;
        
        foreach (Collider2D c in hitColliders) {
            if (c.CompareTag("Player")) {
                // Players only have one collider, send them damage
                c.SendMessage(nameof(PlayerController.TakeDamage), damage);
            }
            else if (c.CompareTag("DestructibleTerrain")) {
                // Terrain has many colliders, save that we hit it and act later
                terrainHit = true;
            }

            if (c.CompareTag("DestructibleObjects")) {
                // Objects only have one collider, send them damage
                c.SendMessage(nameof(IDestructibleObject.TakeDamage), damage);
            }
        }

        if (terrainHit) {
            _paintingParameters.DestructionMode = DestructionMode.DESTROY;
            _paintingParameters.Position.x = (int)(transform.position.x * primaryLayer.PPU) - radius;
            _paintingParameters.Position.y = (int)(transform.position.y * primaryLayer.PPU) - radius;
            primaryLayer.Paint(_paintingParameters);
            _paintingParameters.DestructionMode = DestructionMode.NONE;
            secondaryLayer.Paint(_paintingParameters);
        }

        if (!_impacted) {
            Impact?.Invoke(damage); // TODO: damage is currently meaningless
        }

        SpawnExplosion();
        Destroy(gameObject);
    }
}