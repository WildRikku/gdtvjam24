using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public delegate void SimplePlayerEvent(PlayerController playerController);

public class PlayerController : MonoBehaviour {
    public GameObject mainWeapon;
    public Weapon weapon;
    public Transform weaponSpawnPoint;
    public GameObject dieExplosion;
    private GameMenuController _gameMenuController;
    public string[] dieMessages;
    public string[] playerTurnMesseges;

    private float _health = 50;

    private string[] impactSounds = new string[3] { "Impact1", "Impact2", "Impact3" };

    public float Health {
        get => _health;
        private set {
            _health = value;
            HealthUpdated?.Invoke(this);
            if (_health <= 0) {
                Instantiate(dieExplosion, transform.position, transform.rotation);
                //AudioManager.Instance.PlaySFX("BotDieSound");
                Die();
            }
            else {
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

    [HideInInspector]
    public string botName;
    
    [HideInInspector]
    public int teamColor;
    public TMP_Text nameTxt;
    public TMP_Text healthTxt;
    public GameObject damagePopupPrefab;

    private void Start() {
        _gameMenuController = GameObject.Find("Canvas").GetComponent<GameMenuController>();
        GameObject weaponInstance = Instantiate(mainWeapon, weaponSpawnPoint);
        weapon = weaponInstance.GetComponent<Weapon>();
        nameTxt.text = botName;
        healthTxt.text = Health.ToString();
    }

    private void Update() {
        if (transform.position.y < -0.7f) {
            Health = 0;
        }
    }

    public void StartTurn() {
        _myTurn = true;
        weapon.isActive = true;
        spiderController.isActive = true;
        weapon.AttackFinished += OnWeaponAttackFinished;


        string message = playerTurnMesseges[UnityEngine.Random.Range(0, playerTurnMesseges.Length)];
        message = "Referee: " + message.Replace("name", botName);
        _gameMenuController.TriggerMessage(message,4) ;
    }

    private void OnWeaponAttackFinished(object sender, EventArgs e) {
        weapon.AttackFinished -= OnWeaponAttackFinished;
        EndTurn();
        //Debug.Log("finishing player turn because weapon");
        TurnFinished?.Invoke(this);
    }

    public void EndTurn(bool force = false) {
        if (force) {
            weapon.Deactivate();
        }
        spiderController.isActive = false;
        _myTurn = false;
    }

    public void TakeDamage(float amount) {
        Health -= amount;
        healthTxt.text = Health.ToString();
        GameObject dp = Instantiate(damagePopupPrefab, transform.position, damagePopupPrefab.transform.rotation);
        dp.GetComponent<DamagePopUp>().damageTxt.text = amount.ToString();
    }

    private void Die() {
//        if (_myTurn) {
//            Debug.Log("finishing player turn because dead");
//        }

        // Trigger the Message system
        string dM = botName + " " + dieMessages[UnityEngine.Random.Range(0, dieMessages.Length)];
        _gameMenuController.TriggerMessage(dM, teamColor);

        Destroy(gameObject);
    }
}