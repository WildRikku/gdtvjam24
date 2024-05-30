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
        set
        {
            _health = value;
            HealthUpdated?.Invoke(this);
        }
    }

    public short index;

    public event SimplePlayerEvent AttackFinished;
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
            Die();
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
        _weapon.isActive = false;
        spiderController.isActive = false;
        AttackFinished?.Invoke(this);
    }

    private void Die()
    {
        Health = 0; // do this first as it triggers the event
        AttackFinished?.Invoke(this); // we can call this always because Team will only listen to the active player
        Destroy(gameObject);
    }
}
