using UnityEngine;

public class SpiderMinigunController : RotatingWeapon
{
    public Transform shotTriggerPoint;
    public GameObject bulletPrefab;

    public ParticleSystem muzzleParticle;

    private int salve;
    
    private void Awake()
    {
        battleField = GameObject.Find("GameManagement").GetComponent<BattleField>();
        rotationTempSpeed = rotationSpeed;
    }

    private void Update()
    {
        if (Input.GetKeyDown(activationKey))
        {
            if (isRotating)
                InvokeRepeating(nameof(TriggerShot), 0, 0.1f);
            else
                isRotating = true;
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

        if (salve == 3)
        {
            salve = 0;
            isFadeOutRotation = true;
            CancelInvoke(nameof(TriggerShot));
        }
    }

    
}