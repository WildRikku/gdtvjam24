using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum ShootingStates : int {
    Idle,
    WaitingForAngle,
    WaitingForForce,
    Shot
}

public class SpiderBazookaController : RotatingWeapon {
    public Transform shotTriggerPoint;
    public GameObject bombPrefab;

    public float shootingSpeed = 10f;
    public float shootingForceFactor;

    public ParticleSystem muzzleParticle;
    public CanvasGroup speedBarCG;
    public CanvasGroup crosshairCG;
    public Image speedBar;

    [FormerlySerializedAs("effectName")]
    public string shootEffectName;

    protected new void Awake() {
        speedBarCG.alpha = 0;
        crosshairCG.alpha = 0;
        base.Awake();
    }

    protected new void Update() {
        if (!isActive) {
            return;
        }

        base.Update();

        if (shootingState == ShootingStates.WaitingForForce) {
            shootingForceFactor = Mathf.Lerp(0, 1, (Mathf.Sin(Time.time * 0.5f * Mathf.PI * 2) + 1) / 2);
            speedBar.fillAmount = shootingForceFactor;
        }

        if (!isAiControled && Input.GetKeyDown(activationKey)) {
            if (shootingState != ShootingStates.Shot) {
                NextState();
            }
        }
        
    }
    public void NextState() {
        switch (shootingState) {
            case ShootingStates.WaitingForForce:
                // let player define force       
                isActive = false; // deactivate
                Invoke(nameof(TriggerShot), 0.1f);
                break;
            case ShootingStates.WaitingForAngle:
                AudioManager.Instance.PlaySFX("BazookaLoad");
                isRotating = false;
                speedBarCG.DOFade(1, 0.2f);
                break;
            case ShootingStates.Idle:
                AudioManager.Instance.PlaySFX("BazookaReload");
                isRotating = true;
                crosshairCG.alpha = 1;
                break;
        }
        shootingState++;
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

        Invoke(nameof(InvokeShootingReset), 3f);
    }

    protected virtual float CalculateShootingForce() {
        return 5f + shootingSpeed * shootingForceFactor * 1.5f;
    }

    protected void InvokeShootingReset() {
    }
}