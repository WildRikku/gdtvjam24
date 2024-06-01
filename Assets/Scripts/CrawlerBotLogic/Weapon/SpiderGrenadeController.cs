using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SpiderGrenadeController : RotatingWeapon {
    public Transform shotTriggerPoint;
    public GameObject bombPrefab;

    public float shootingSpeed = 10f;
    private float shootingforce = 5f;

    public ParticleSystem muzzleParticle;
    public CanvasGroup speedBarCG;
    public CanvasGroup crosshairCG;
    public Image speedBar;

    private bool isShooting;
    private bool isShootingState;
    private bool shootigReset = false; //TODO

    private Rigidbody2D botRb;

    private void Awake() {
        battleField = GameObject.Find("GameManagement").GetComponent<BattleField>();
        rotationTempSpeed = rotationSpeed;
    }

    private void Start() {
        botRb = gameObject.GetComponentInParent<Rigidbody2D>();

        battleField = GameObject.Find("GameManagement").GetComponent<BattleField>();
        speedBarCG.alpha = 0;
        crosshairCG.alpha = 0;
    }

    private void Update() {
        if (!isActive) {
            return;
        }

        if (isShootingState && isRotating) {
            ShootingState();
        }

        if (Input.GetKeyDown(activationKey) && isShootingState == false && isShooting == false &&
            shootigReset == false) {
            if (isRotating) {
                AudioManager.Instance.PlaySFX("BazookaLoad");
                isShootingState = true;
                speedBarCG.DOFade(1, 0.2f);
            }
            else {
                AudioManager.Instance.PlaySFX("BazookaReload");
                isRotating = true;
                crosshairCG.alpha = 1;
            }
        }

        if (isRotating && isShootingState == false) {
            Rotate();
        }
    }

    private void ShootingState() {
        float pendulumFrequency = 0.5f;

        shootingforce = Mathf.Lerp(0, 1, (Mathf.Sin(Time.time * pendulumFrequency * Mathf.PI * 2) + 1) / 2);
        speedBar.fillAmount = shootingforce;

        if (Input.GetKeyDown(activationKey)) {
            Invoke(nameof(TriggerShot), 0.1f);
            isShooting = true;
            isShootingState = false;
        }
    }

    private void TriggerShot() {
        AudioManager.Instance.PlaySFX("GranadeShoot");
        ShootImpulse(5);

        ProjectileCount = 1;
        muzzleParticle.Emit(40);
        GameObject bomb = SpawnProjectile(bombPrefab, shotTriggerPoint.position, shotTriggerPoint.rotation);

        Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
        shootingforce = 2.5f + shootingSpeed * shootingforce * 0.3f;

        if (rb != null) {
            rb.AddForce(shotTriggerPoint.up * shootingforce, ForceMode2D.Impulse);
        }

        speedBarCG.DOFade(0, 0.2f);
        crosshairCG.DOFade(0, 0.2f);

        isRotating = false;
        isShooting = false;
        shootigReset = true;
        Invoke(nameof(InvokeShootingReset), 3f);
    }

    private void InvokeShootingReset() {
        shootigReset = false;
    }

    private void ShootImpulse(float force) {
        if (botRb != null) {
            Vector2 direction = weaponRotationPoint.right * -1;
            Vector2 impulse = direction * force;

            botRb.AddForce(impulse, ForceMode2D.Impulse);
        }
    }
}