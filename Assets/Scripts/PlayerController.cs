using System;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

public delegate void SimplePlayerEvent(PlayerController playerController);

public class PlayerController : MonoBehaviour
{
    public GameObject mainWeapon;
    public Weapon weapon;
    public Transform weaponSpawnPoint;

    private float _health = 50;
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

    [HideInInspector] public String botName;
    public TMP_Text nameTxt;
    public TMP_Text healthTxt;
    public GameObject damagePopupPrefab;

    void Start()
    {
        GameObject weaponInstance = Instantiate(mainWeapon, weaponSpawnPoint);
        weapon = weaponInstance.GetComponent<Weapon>();
        nameTxt.text = botName;
        healthTxt.text = Health.ToString();
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
        healthTxt.text = Health.ToString();
        GameObject dp = Instantiate(damagePopupPrefab, transform.position, damagePopupPrefab.transform.rotation);
        dp.GetComponent<DamagePopUp>().damageTxt.text = amount.ToString();
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
