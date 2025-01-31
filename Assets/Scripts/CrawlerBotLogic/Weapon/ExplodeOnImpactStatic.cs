using CrawlerBotLogic.Weapon;
using UnityEngine;

public class ExplodeOnImpactStatic : ExplodeOnImpact {
    [Tooltip("Distance needed to fall before it explodes (will always explode when hit by a projectile)")]
    public float fallDistance;

    private float _lastPosition;
    private float _fallenDistance;
    [field: SerializeField]
    public float health { get; set; }
    
    protected new void Awake()
    {
        BattleField battleField = GameObject.Find("GameManagement").GetComponent<BattleField>();
        primaryLayer = battleField.collidableLayer;
        secondaryLayer = battleField.visibleLayer;
        _lastPosition = transform.position.y;
        
        base.Awake();
    }

    private void FixedUpdate() {
        float y = transform.position.y;
        if (y < _lastPosition) {
            _fallenDistance += _lastPosition - y;
        }
        else {
            _fallenDistance = 0;
        }
        _lastPosition = y;
    }
    
    protected override void OnCollisionEnter2D(Collision2D col) {
        if (_fallenDistance > fallDistance) {
            base.OnCollisionEnter2D(col);
        }
    }
    
    public void Die() {
        Explode(transform.position);
    }

    public void TakeDamage(float amount) {
        if (health == 0) return; // this prevents calling Die() multiple times
        
        health -= amount;
        if (health <= 0) {
            health = 0;
            Die();
        }
    }
}
