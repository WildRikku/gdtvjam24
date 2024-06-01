using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SpiderGrenadeController : SpiderBazookaController {
    private void Awake() {
        AwakeActions();
    }

    private void Start() {
        StartActions();
    }

    private void Update() {
        UpdateActions();
    }

    private void TriggerShot() {
        AudioManager.Instance.PlaySFX(shootEffectName);
        ShootImpulse(5);

        ProjectileCount = 1;
        muzzleParticle.Emit(40);
        GameObject bomb = SpawnProjectile(bombPrefab, shotTriggerPoint.position, shotTriggerPoint.rotation);

        Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
        float shootingforce = 2.5f + shootingSpeed * shootingForceFactor * 0.3f;

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
}