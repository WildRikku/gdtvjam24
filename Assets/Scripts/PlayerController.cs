using System;
using UnityEngine;
using UnityEngine.Serialization;

public delegate void SimplePlayerEvent(PlayerController playerController);

public class PlayerController : MonoBehaviour
{
    public GameObject mainWeapon;
    [FormerlySerializedAs("_weapon")] public Weapon weapon;
    public Transform weaponSpawnPoint;

    private float _health;
    public float Health
    {
        get => _health;
        private set
        {
            _health = value;
            HealthUpdated?.Invoke(this);
            if (_health <= 0)
            {
                Die();
            }
        }
    }

    public short index;

    public event SimplePlayerEvent TurnFinished;
    public event SimplePlayerEvent HealthUpdated;
    
    public SpiderController spiderController;

    void Start()
    {
        GameObject weaponInstance = Instantiate(mainWeapon, weaponSpawnPoint);
        weapon = weaponInstance.GetComponent<Weapon>();
    }

    private void FixedUpdate()
    {
        if (transform.position.y < -0.7f)
        {
            Health = 0;
        }
    }

    public void Attack()
    {
        weapon.isActive = true;
        spiderController.isActive = true;
        weapon.AttackFinished += WeaponOnAttackFinished;
    }

    private void WeaponOnAttackFinished(object sender, EventArgs e)
    {
        weapon.AttackFinished -= WeaponOnAttackFinished;
        weapon.isActive = false;
        spiderController.isActive = false;
        TurnFinished?.Invoke(this);
    }

    public void TakeDamage(float amount)
    {
        Health -= amount;
    }

    private void Die()
    {
        if (weapon.isActive)
        {
            weapon.AttackFinished -= WeaponOnAttackFinished;
        }

        TurnFinished?.Invoke(this); // we can call this always because Team will only listen to the active player
        Destroy(gameObject);
    }
}
