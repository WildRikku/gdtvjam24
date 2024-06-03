using System;
using UnityEngine;
using UnityEngine.Serialization;

public delegate void ProjectileFired(GameObject projectile);

public class Weapon : MonoBehaviour {
    public BattleField battleField;
    public KeyCode activationKey = KeyCode.A;

    public bool isActive;

    public event EventHandler AttackFinished;

    [FormerlySerializedAs("name")]
    [Header("Configuration")]
    public string displayName;
    [TextArea(3,5)] public string description;
    public Sprite buttonSprite;
    public int ammo;
    public int index;

    protected void Start() {
        battleField = GameObject.Find("GameManagement").GetComponent<BattleField>();
    }

    public virtual void Trigger() {
        Debug.Log("Default Weapon base class Boom");
    }

    public virtual void Deactivate() {
        isActive = false;
    }

    protected void FinishAttack() {
        AttackFinished?.Invoke(this, EventArgs.Empty);
    }
}

public class ProjectileWeapon : Weapon {
    protected int ProjectileCount;
    public event ProjectileFired ProjectileFired;

    protected new void Start() {
        base.Start();
    }
    
    private void OnProjectileImpact(float damage) {
        ProjectileCount--;
        if (ProjectileCount == 0) {
            FinishAttack();
        }
    }

    protected GameObject SpawnProjectile(GameObject prefab, Vector3 translation, Quaternion rotation) {
        GameObject projectile = Instantiate(prefab, translation, rotation);
        ExplodeOnImpact eoi = projectile.GetComponent<ExplodeOnImpact>();
        eoi.primaryLayer = battleField.collidableLayer;
        eoi.secondaryLayer = battleField.visibleLayer;
        eoi.Impact += OnProjectileImpact;
        ProjectileFired?.Invoke(projectile);
        return projectile;
    }
}