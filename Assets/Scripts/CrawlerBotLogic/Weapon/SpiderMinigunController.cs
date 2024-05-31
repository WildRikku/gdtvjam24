using UnityEngine;
using DG.Tweening;

public class SpiderMinigunController : RotatingWeapon
{
    public Transform shotTriggerPoint;
    public GameObject bulletPrefab;

    public ParticleSystem muzzleParticle;
    public float bulletforce = 15;

    public CanvasGroup crosshairCG;
    private int _firedProjectiles;
    private bool _shootingReset; 

    private void Awake()
    {
        crosshairCG.alpha = 0;
        battleField = GameObject.Find("GameManagement").GetComponent<BattleField>();
        rotationTempSpeed = rotationSpeed;
    }

    private void Update()
    {
        if (!isActive) return;

        if (Input.GetKeyDown(activationKey) && _shootingReset == false)
        {
            if (isRotating)
            {
                ProjectileCount = 3;
                InvokeRepeating(nameof(TriggerShot), 0, 0.05f);
            }
            else
            {
                isRotating = true;
                crosshairCG.alpha = 1;
            }
        }

        if (!isRotating) return;
        
        Rotate();
        if (isFadeOutRotation)
            FadeOutRotation();
    }

    private void TriggerShot()
    {
        muzzleParticle.Emit(15);
        GameObject bullet = SpawnProjectile(bulletPrefab, shotTriggerPoint.position, shotTriggerPoint.rotation);
        _firedProjectiles++;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(shotTriggerPoint.up * bulletforce, ForceMode2D.Impulse);
        }

        if (_firedProjectiles == 3)
        {
            // stop firing. attack will end when all projectiles hit something via parent class
            _firedProjectiles = 0;
            isFadeOutRotation = true;
            CancelInvoke(nameof(TriggerShot));
            crosshairCG.DOFade(0, 0.2f);
            _shootingReset = true;
            Invoke(nameof(InvokeShootingReset), 3f);

        }
    }

    private void InvokeShootingReset()
    {
        _shootingReset = false;
    }


}