
using UnityEngine;
using DG.Tweening;

public class SpiderMinigunController : RotatingWeapon
{
    public Transform shotTriggerPoint;
    public GameObject bulletPrefab;

    public ParticleSystem muzzleParticle;
    public float bulletforce = 15;

    public CanvasGroup crosshairCG;
    private int salve;
    
    private void Awake()
    {
        crosshairCG.alpha = 0;
        battleField = GameObject.Find("GameManagement").GetComponent<BattleField>();
        rotationTempSpeed = rotationSpeed;
    }

    private void Update()
    {
        if (!isActive) return;
        
        if (Input.GetKeyDown(activationKey))
        {
            if (isRotating)
                InvokeRepeating(nameof(TriggerShot), 0, 0.1f);
            else
            {
                isRotating = true;
                crosshairCG.alpha = 1;

            }
        }

        if (isRotating)
        {
            Rotate();
            if (isFadeOutRotation)
                FadeOutRotation();
        }
    }
    
    private void TriggerShot()
    {
        muzzleParticle.Emit(15);
        GameObject bullet = Instantiate(bulletPrefab, shotTriggerPoint.position, shotTriggerPoint.rotation);
        ExplodeOnImpact eoi = bullet.GetComponent<ExplodeOnImpact>();
        eoi.primaryLayer = battleField.collidableLayer;
        eoi.secondaryLayer = battleField.visibleLayer;
        salve += 1;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(shotTriggerPoint.up * bulletforce, ForceMode2D.Impulse);
        }

        if (salve == 3)
        {
            salve = 0;
            isFadeOutRotation = true;
            CancelInvoke(nameof(TriggerShot));
            OnAttackFinished();
            crosshairCG.DOFade(0, 0.2f);
        }

    }

    
}