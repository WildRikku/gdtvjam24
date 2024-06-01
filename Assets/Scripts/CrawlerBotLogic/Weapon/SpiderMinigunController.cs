using UnityEngine;
using DG.Tweening;

public class SpiderMinigunController : RotatingWeapon {
    public Transform shotTriggerPoint;
    public GameObject bulletPrefab;

    public ParticleSystem muzzleParticle;
    public float bulletforce = 15;

    public CanvasGroup crosshairCG;
    private int _firedProjectiles;
    private bool _shootingReset;

    private Rigidbody2D botRb;

    private void Awake() {
        botRb = gameObject.GetComponentInParent<Rigidbody2D>();

        crosshairCG.alpha = 0;
        battleField = GameObject.Find("GameManagement").GetComponent<BattleField>();
        rotationTempSpeed = rotationSpeed;
    }

    private void Update() {
        if (!isActive) {
            return;
        }

        if (Input.GetKeyDown(activationKey) && _shootingReset == false) {
            if (isRotating) {
                ProjectileCount = 3;
                InvokeRepeating(nameof(TriggerShot), 0, 0.05f);
            }
            else {
                AudioManager.Instance.PlaySFX("MinigunRealod");
                isRotating = true;
                crosshairCG.alpha = 1;
            }
        }

        if (!isRotating) {
            return;
        }

        Rotate();
        if (isFadeOutRotation) {
            FadeOutRotation();
        }
    }

    private void TriggerShot() {
        ShootImpulse(2f);
        muzzleParticle.Emit(15);
        GameObject bullet = SpawnProjectile(bulletPrefab, shotTriggerPoint.position, shotTriggerPoint.rotation);
        _firedProjectiles++;

        if (_firedProjectiles == 1 || _firedProjectiles == 3) {
            AudioManager.Instance.PlaySFX("MinigunShoot1");
        }

        if (_firedProjectiles == 2) {
            AudioManager.Instance.PlaySFX("MinigunShoot2");
        }

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null) {
            rb.AddForce(shotTriggerPoint.up * bulletforce, ForceMode2D.Impulse);
        }

        if (_firedProjectiles == 3) {
            // stop firing. attack will end when all projectiles hit something via parent class
            _firedProjectiles = 0;
            isFadeOutRotation = true;
            CancelInvoke(nameof(TriggerShot));
            crosshairCG.DOFade(0, 0.2f);
            _shootingReset = true;
            Invoke(nameof(InvokeShootingReset), 3f);
        }
    }

    private void InvokeShootingReset() {
        _shootingReset = false;
    }

    private void ShootImpulse(float force) {
        if (botRb != null) {
            Vector2 direction = weaponRotationPoint.right * -1;
            Vector2 impulse = direction * force;

            botRb.AddForce(impulse, ForceMode2D.Impulse);
        }
    }
}