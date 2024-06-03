using UnityEngine;

public class ExplodeOnImpactStatic : ExplodeOnImpact {
    [Tooltip("Distance needed to fall before it explodes (will always explode when hit by a projectile)")]
    public float fallDistance;

    private float _lastPosition;
    private float _fallenDistance;
    
    
    protected new void Start()
    {
        BattleField battleField = GameObject.Find("GameManagement").GetComponent<BattleField>();
        primaryLayer = battleField.collidableLayer;
        secondaryLayer = battleField.visibleLayer;
        _lastPosition = transform.position.y;
        
        base.Start();
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
}
