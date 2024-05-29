using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SpiderBazookaController : MonoBehaviour
{
    public Transform weaponRotationPoint;
    public Transform shotTriggerPoint;
    public GameObject bombPrefab;

    public float minZRotation = -20f;
    public float maxZRotation = 200f;
    public float rotationSpeed = 10f;
    public float shootingSpeed = 10f;
    public float shootingFactor = 10f;
    public KeyCode activationKey = KeyCode.A;

    public ParticleSystem muzzleParticle;
    public CanvasGroup speedBarCG;
    public Image speedBar;
    private BattleField battleField;

    private bool isRotating;
    private bool isShooting;
    private bool isShootingState;
    private bool rotatingToMax = true;


    private void Start()
    {
        battleField = GameObject.Find("GameManagement").GetComponent<BattleField>();
        speedBarCG.alpha = 0;
    }

    private void Update()
    {
        if (isShootingState && isRotating) ShootingState();

        if (Input.GetKeyDown(activationKey) && isShootingState == false && isShooting == false)
        {
            if (isRotating)
            {
                isShootingState = true;
                shootingFactor = 0;
                speedBarCG.DOFade(1, 0.2f);
            }
            else
            {
                isRotating = true;
            }
        }

        if (isRotating && isShootingState == false) RotateState();
    }


    private void ShootingState()
    {
        float pendulumFrequency = 0.5f;

        shootingFactor = Mathf.Lerp(0, 1, (Mathf.Sin(Time.time * pendulumFrequency * Mathf.PI * 2) + 1) / 2);
        speedBar.fillAmount = shootingFactor;

        if (Input.GetKeyDown(activationKey))
        {
            Invoke(nameof(TriggerShot), 0.1f);
            isShooting = true;
            isShootingState = false;
        }
    }

    private void RotateState()
    {
        float currentZRotation = weaponRotationPoint.localEulerAngles.z;

        if (rotatingToMax)
        {
            currentZRotation = Mathf.MoveTowards(currentZRotation, maxZRotation, rotationSpeed * Time.deltaTime);

            if (currentZRotation >= maxZRotation) rotatingToMax = false;
        }
        else
        {
            currentZRotation = Mathf.MoveTowards(currentZRotation, minZRotation, rotationSpeed * Time.deltaTime);

            if (currentZRotation <= minZRotation) rotatingToMax = true;
        }

        Vector3 localEulerAngles = weaponRotationPoint.localEulerAngles;
        localEulerAngles = new Vector3(localEulerAngles.x,
            localEulerAngles.y, currentZRotation);
        weaponRotationPoint.localEulerAngles = localEulerAngles;
    }


    private void TriggerShot()
    {
        muzzleParticle.Emit(40);
        GameObject bomb = Instantiate(bombPrefab, shotTriggerPoint.position, shotTriggerPoint.rotation);
        ExplodeOnImpact eoi = bomb.GetComponent<ExplodeOnImpact>();
        eoi.primaryLayer = battleField.collidableLayer;
        eoi.secondaryLayer = battleField.visibleLayer;
        MinigunBullet mb = bomb.GetComponent<MinigunBullet>();
        mb.speed = 5 + shootingSpeed * shootingFactor * 1.3f;
        speedBarCG.DOFade(0, 0.2f);

        isRotating = false;
        isShooting = false;
    }
}