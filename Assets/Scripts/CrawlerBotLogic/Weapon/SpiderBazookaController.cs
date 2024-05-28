using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SpiderBazookaController : MonoBehaviour
{
    public Transform weaponRotationPoint;
    public Transform shotTriggerPoint;
    public GameObject bombPrefab;
    private BattleField battleField;

    public float minZRotation = -20f;
    public float maxZRotation = 200f;
    public float rotationSpeed = 10f;
    public float shootingSpeed = 10f;
    public float shootingFactor = 10f;
    public KeyCode activationKey = KeyCode.A;

    private bool isRotating = false;
    private bool rotatingToMax = true;
    private bool isShootingState = false;

    public ParticleSystem muzzleParticle;
    public CanvasGroup speedBarCG;
    public Image speedBar;


    private void Start()
    {
        battleField = GameObject.Find("GameManagement").GetComponent<BattleField>();
        speedBarCG.alpha = 0;
    }

    void Update()
    {
        if (isShootingState == true && isRotating == true)
        {
            ShootingState();
        }

        if (Input.GetKeyDown(activationKey) && isShootingState == false)
        {
            if (isRotating == true)
            {
                isShootingState = true;
                speedBarCG.DOFade(1, 0.2f);
            }
            else
            {
                isRotating = true;
            }
        }

        if (isRotating == true && isShootingState == false)
        {
            RotateState();
        }
    }


    void ShootingState()
    {
        float pendulumFrequency = 0.5f;

        shootingFactor = Mathf.Lerp(0, 1, (Mathf.Sin(Time.time * pendulumFrequency * Mathf.PI * 2) + 1) / 2);
        speedBar.fillAmount = shootingFactor;

        if (Input.GetKeyDown(activationKey))
        {
            Invoke(nameof(TriggerShot), 0.1f);
            isShootingState = false;
            
        }
    }

    void RotateState()
    {
        float currentZRotation = weaponRotationPoint.localEulerAngles.z;

        if (rotatingToMax)
        {
            currentZRotation = Mathf.MoveTowards(currentZRotation, maxZRotation, rotationSpeed * Time.deltaTime);

            if (currentZRotation >= maxZRotation)
            {
                rotatingToMax = false;

            }
        }
        else
        {
            currentZRotation = Mathf.MoveTowards(currentZRotation, minZRotation, rotationSpeed * Time.deltaTime);

            if (currentZRotation <= minZRotation)
            {
                rotatingToMax = true;
            }
        }

        weaponRotationPoint.localEulerAngles = new Vector3(weaponRotationPoint.localEulerAngles.x, weaponRotationPoint.localEulerAngles.y, currentZRotation);
    }



    private void TriggerShot()
    {
        muzzleParticle.Emit(40);
        GameObject bomb = Instantiate(bombPrefab, shotTriggerPoint.position, shotTriggerPoint.rotation);
        BazookaImpact eoi = bomb.GetComponent<BazookaImpact>();
        eoi.primaryLayer = battleField.collidableLayer;
        eoi.secondaryLayer = battleField.visibleLayer;
        MinigunBullet mb = bomb.GetComponent<MinigunBullet>();
        mb.speed = 5 + (shootingSpeed * shootingFactor * 1.5f);
        speedBarCG.DOFade(0, 0.2f);

        isRotating = false;
    }


}
