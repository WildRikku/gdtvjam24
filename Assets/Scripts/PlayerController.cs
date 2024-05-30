using System;
using UnityEngine;

public delegate void SimplePlayerEvent(PlayerController playerController);

public class PlayerController : MonoBehaviour
{
    public GameObject mainWeapon;
    private Weapon _weapon;
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
        _weapon = weaponInstance.GetComponent<Weapon>();
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
        _weapon.isActive = true;
        spiderController.isActive = true;
        _weapon.AttackFinished += WeaponOnAttackFinished;
    }

    private void WeaponOnAttackFinished(object sender, EventArgs e)
    {
        _weapon.AttackFinished -= WeaponOnAttackFinished;
        _weapon.isActive = false;
        spiderController.isActive = false;
        TurnFinished?.Invoke(this);
    }

    public void TakeDamage(float amount)
    {
        Health -= amount;
    }

    private void Die()
    {
        if (_weapon.isActive)
        {
            _weapon.AttackFinished -= WeaponOnAttackFinished;
        }

        TurnFinished?.Invoke(this); // we can call this always because Team will only listen to the active player
        Destroy(gameObject);
    }
}
