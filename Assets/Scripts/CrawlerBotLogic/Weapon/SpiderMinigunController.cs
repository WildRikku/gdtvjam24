using UnityEngine;
using DG.Tweening;

public class SpiderMinigunController : RotatingWeapon {
    public Transform shotTriggerPoint;
    public GameObject bulletPrefab;

    public ParticleSystem muzzleParticle;
    public float bulletforce = 15;

    public CanvasGroup crosshairCG;
    private int _firedProjectiles;

    private new void Awake() {
        crosshairCG.alpha = 0;
        base.Awake();
    }

    private new void Update() {
        if (!isActive) {
            return;
        }
        
        base.Update();

        if (!isAiControled && Input.GetKeyDown(activationKey)) {
            NextState();
        }
    }

    public void NextState() {
        switch (shootingState) {
            case ShootingStates.WaitingForAngle:
                isActive = false; // deactivates listening for keys but does not deactivate rotation
                ProjectileCount = 3;
                InvokeRepeating(nameof(TriggerShot), 0, 0.05f);
                break;
            case ShootingStates.Idle:
                AudioManager.Instance.PlaySFX("MinigunReload");
                isRotating = true;
                crosshairCG.alpha = 1;
                break;
        }
        shootingState++;
    }

    private void TriggerShot() {
        Recoil(2f);
        muzzleParticle.Emit(15);
        GameObject bullet = SpawnProjectile(bulletPrefab, shotTriggerPoint.position, shotTriggerPoint.rotation);
        _firedProjectiles++;

        AudioManager.Instance.PlaySFX(_firedProjectiles == 2 ? "MinigunShoot2" : "MinigunShoot1");

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
        }
    }
}