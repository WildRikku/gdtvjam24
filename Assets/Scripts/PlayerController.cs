using System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public delegate void SimplePlayerEvent(PlayerController playerController);

public class PlayerController : MonoBehaviour {
    [FormerlySerializedAs("mainWeapon")]
    public GameObject weaponPrefab;
    private GameObject _weaponObject;
    public Weapon weapon;
    public Transform weaponSpawnPoint;
    public GameObject dieExplosionPrefab;
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
                _health = 0;
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
    private bool _drowned;
    private bool _hasFired;

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
        _gameMenuController = GameObject.Find("UI").GetComponent<GameMenuController>();
        nameTxt.text = botName;
        healthTxt.text = Health.ToString();
    }

    private void Update() {
        if (transform.position.y < -0.7f) {
            _drowned = true;
            EndTurn();
            if (Health > 0) {
                Health = 0;
                if (!_hasFired) {
                    TurnFinished?.Invoke(this);
                }
            }
        }
    }

    public void CreateWeapon(int weaponIndex) {
        _weaponObject = Instantiate(weaponPrefab, weaponSpawnPoint);
        weapon = _weaponObject.GetComponent<Weapon>();
        weapon.index = weaponIndex;
    }

    public void StartTurn() {
        _myTurn = true;
        spiderController.isActive = true;
        ActivateWeapon();
        string message = playerTurnMesseges[UnityEngine.Random.Range(0, playerTurnMesseges.Length)];
        message = "Referee: " + message.Replace("name", botName);
        _gameMenuController.TriggerMessage(message,4) ;
    }

    private void ActivateWeapon() {
        weapon.isActive = true;
        weapon.AttackFinished += OnWeaponAttackFinished;
        ProjectileWeapon pw = weapon as ProjectileWeapon;
        if (pw != null) {
            pw.ProjectileFired += OnProjectileFired;
        }
    }

    private void OnProjectileFired(GameObject projectile) {
        _hasFired = true;
    }

    public void ChangeWeapon(GameObject newWeaponPrefab, int weaponIndex) {
        weaponPrefab = newWeaponPrefab;
        Destroy(_weaponObject);
        CreateWeapon(weaponIndex);
        ActivateWeapon();
    }

    private void OnWeaponAttackFinished(object sender, EventArgs e) {
        weapon.AttackFinished -= OnWeaponAttackFinished;
        _hasFired = false;
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
        if (_health == 0) return;
        amount = Mathf.RoundToInt(amount);
        Health -= amount;
        healthTxt.text = Health.ToString();
        GameObject dp = Instantiate(damagePopupPrefab, transform.position, damagePopupPrefab.transform.rotation);
        dp.GetComponent<DamagePopUp>().damageTxt.text = amount.ToString();
    }

    private void Die() {
        if (!_drowned) {
            SpawnDieExplosion();
            //AudioManager.Instance.PlaySFX("BotDieSound");
        }

        // Trigger the Message system
        string dM = botName + " " + dieMessages[UnityEngine.Random.Range(0, dieMessages.Length)];
        _gameMenuController.TriggerMessage(dM, teamColor);

        Destroy(gameObject);
    }

    private void SpawnDieExplosion() {
        BattleField battleField = GameObject.Find("GameManagement").GetComponent<BattleField>();

        GameObject projectile = Instantiate(dieExplosionPrefab, transform.position, transform.rotation);
        ExplodeOnImpact eoi = projectile.GetComponent<ExplodeOnImpact>();
        eoi.primaryLayer = battleField.collidableLayer;
        eoi.secondaryLayer = battleField.visibleLayer;
    }
}