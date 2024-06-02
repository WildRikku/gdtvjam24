using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SpiderBazookaController : RotatingWeapon {
    public Transform shotTriggerPoint;
    public GameObject bombPrefab;

    public float shootingSpeed = 10f;
    protected float shootingForceFactor;

    public ParticleSystem muzzleParticle;
    public CanvasGroup speedBarCG;
    public CanvasGroup crosshairCG;
    public Image speedBar;

    protected bool isShooting;
    private bool waitingForShot;
    protected bool shootingReset = false; //TODO

    protected Rigidbody2D botRb;
    [FormerlySerializedAs("effectName")]
    public string shootEffectName;

    protected void Awake() {
        battleField = GameObject.Find("GameManagement").GetComponent<BattleField>();
        rotationTempSpeed = rotationSpeed;
    }

    protected void Start() {
        botRb = gameObject.GetComponentInParent<Rigidbody2D>();
        battleField = GameObject.Find("GameManagement").GetComponent<BattleField>();
        speedBarCG.alpha = 0;
        crosshairCG.alpha = 0;
    }

    protected void Update() {
        if (!isActive) {
            return;
        }

        if (waitingForShot && isRotating) {
            ShootingState();
        }

        if (Input.GetKeyDown(activationKey) && waitingForShot == false && isShooting == false &&
            shootingReset == false) {
            if (isRotating) {
                AudioManager.Instance.PlaySFX("BazookaLoad");
                waitingForShot = true;
                speedBarCG.DOFade(1, 0.2f);
            }
            else {
                AudioManager.Instance.PlaySFX("BazookaReload");
                isRotating = true;
                crosshairCG.alpha = 1;
            }
        }

        if (isRotating && waitingForShot == false) {
            Rotate();
        }
    }

    private void ShootingState() {
        // let player define force       
        shootingForceFactor = Mathf.Lerp(0, 1, (Mathf.Sin(Time.time * 0.5f * Mathf.PI * 2) + 1) / 2);
        speedBar.fillAmount = shootingForceFactor;

        if (Input.GetKeyDown(activationKey)) {
            Invoke(nameof(TriggerShot), 0.1f);
            isShooting = true;
            waitingForShot = false;
            isActive = false;
        }
    }

    protected void TriggerShot() {
        AudioManager.Instance.PlaySFX(shootEffectName);
        Recoil(10f * Mathf.Max(0.5f, shootingForceFactor));
        
        ProjectileCount = 1;
        muzzleParticle.Emit(40);
        GameObject bomb = SpawnProjectile(bombPrefab, shotTriggerPoint.position, shotTriggerPoint.rotation);

        Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
        float shootingforce = CalculateShootingForce();

        if (rb != null) {
            rb.AddForce(shotTriggerPoint.up * shootingforce, ForceMode2D.Impulse);
        }

        speedBarCG.DOFade(0, 0.2f);
        crosshairCG.DOFade(0, 0.2f);

        isRotating = false;
        isShooting = false;
        shootingReset = true;
        Invoke(nameof(InvokeShootingReset), 3f);
    }

    protected virtual float CalculateShootingForce() {
        return 5f + shootingSpeed * shootingForceFactor * 1.5f;
    }

    protected void InvokeShootingReset() {
        shootingReset = false;
    }

    protected virtual void Recoil(float force) {
        if (botRb == null) {
            return;
        }

        Vector2 direction = weaponRotationPoint.right * -1;
        Vector2 impulse = direction * force;

        botRb.AddForce(impulse, ForceMode2D.Impulse);
    }
}