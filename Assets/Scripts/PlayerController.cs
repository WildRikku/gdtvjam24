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
    public GameObject dieExplosion;
    private GameMenuController _gameMenuController;
    public string[] dieMessages;

    private float _health = 50;

    private string[] impactSounds = new string[3] { "Impact1", "Impact2", "Impact3" };
    public float Health
    {
        get => _health;
        private set
        {
            _health = value;
            HealthUpdated?.Invoke(this);
            if (_health <= 0)
            {
                Instantiate(dieExplosion, transform.position, transform.rotation);
                //AudioManager.Instance.PlaySFX("BotDieSound");
                Die();
            }
            else
            {
                // trigger impact sound
                int ran = UnityEngine.Random.Range(0, 3);
                AudioManager.Instance.PlaySFX(impactSounds[ran]);
            }
        }
    }

    public short index;
    private bool _myTurn;

    public event SimplePlayerEvent TurnFinished;
    public event SimplePlayerEvent HealthUpdated;
    
    public SpiderController spiderController;

    [HideInInspector] public string botName;
    [HideInInspector] public int teamColor;
    public TMP_Text nameTxt;
    public TMP_Text healthTxt;
    public GameObject damagePopupPrefab;

    void Start()
    {
        _gameMenuController = GameObject.Find("Canvas").GetComponent<GameMenuController>();
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
        _myTurn = true;
        weapon.isActive = true;
        spiderController.isActive = true;
        weapon.AttackFinished += WeaponOnAttackFinished;
    }

    private void WeaponOnAttackFinished(object sender, EventArgs e)
    {
        weapon.AttackFinished -= WeaponOnAttackFinished;
        weapon.isActive = false;
        spiderController.isActive = false;
        _myTurn = false;
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

        if (_myTurn)
        {
            TurnFinished?.Invoke(this);
        }

        // Trigger the Message system
        string dM = botName + " " + dieMessages[UnityEngine.Random.Range(0, dieMessages.Length) ];
        _gameMenuController.TriggerMessage(dM, teamColor);

        Destroy(gameObject);
    }


}
