using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public BattleField battleField;
    public KeyCode activationKey = KeyCode.A;

    public bool isActive;

    public event EventHandler AttackFinished;

    public virtual void Trigger()
    {
        Debug.Log("Default Weapon base class Boom");
    }

    protected void OnAttackFinished()
    {
        AttackFinished?.Invoke(this, EventArgs.Empty);
    }
}

public class ProjectileWeapon : Weapon
{
    protected int ProjectileCount;

    protected void OnProjectileImpact(float damage)
    {
        ProjectileCount--;
        if (ProjectileCount == 0)
        {
            OnAttackFinished();
        }
    }

    protected GameObject SpawnProjectile(GameObject prefab, Vector3 translation, Quaternion rotation)
    {
        GameObject projectile = Instantiate(prefab, translation, rotation, transform);
        ExplodeOnImpact eoi = projectile.GetComponent<ExplodeOnImpact>();
        eoi.primaryLayer = battleField.collidableLayer;
        eoi.secondaryLayer = battleField.visibleLayer;
        eoi.Impact += OnProjectileImpact;
        return projectile;
    }
}