using UnityEngine;

public class SpiderMinigunController : MonoBehaviour
{
    public Transform weaponRotationPoint;
    public Transform shotTriggerPoint;
    public GameObject bulletPrefab;
    private BattleField battleField;

    public float minZRotation = -20f;
    public float maxZRotation = 200f;
    public float rotationSpeed = 10f;
    private float rotationTempSpeed = 0;
    public KeyCode activationKey = KeyCode.A;

    private bool isRotating = false;
    private bool rotatingToMax = true;
    private bool isFadeOutRotation = false;

    public ParticleSystem muzzleParticle;
    private int salve;

    private void Awake()
    {
        battleField = GameObject.Find("GameManagement").GetComponent<BattleField>();
        rotationTempSpeed = rotationSpeed;

    }

    void Update()
    {
        if (Input.GetKeyDown(activationKey))
        {
            if (isRotating == true)
            {
                InvokeRepeating(nameof(TriggerShot), 0, 0.1f);
            }
            else
            {
                isRotating = !isRotating;
            }
        }

        if (isRotating)
        {
            Rotate();
            if (isFadeOutRotation == true)
                FadeOutRotation();
        }
    }

    void Rotate()
    {
        float currentZRotation = weaponRotationPoint.localEulerAngles.z;

        if (rotatingToMax)
        {
            currentZRotation = Mathf.MoveTowards(currentZRotation, maxZRotation, rotationTempSpeed * Time.deltaTime);

            if (currentZRotation >= maxZRotation)
            {
                rotatingToMax = false;

            }
        }
        else
        {
            currentZRotation = Mathf.MoveTowards(currentZRotation, minZRotation, rotationTempSpeed * Time.deltaTime);

            if (currentZRotation <= minZRotation)
            {
                rotatingToMax = true;
            }
        }

        weaponRotationPoint.localEulerAngles = new Vector3(weaponRotationPoint.localEulerAngles.x, weaponRotationPoint.localEulerAngles.y, currentZRotation);
    }

    private void TriggerShot()
    {
        muzzleParticle.Emit(15);
        GameObject bullet = Instantiate(bulletPrefab, shotTriggerPoint.position, shotTriggerPoint.rotation);
        MinigunImpact eoi = bullet.GetComponent<MinigunImpact>();
        eoi.primaryLayer = battleField.collidableLayer;
        eoi.secondaryLayer = battleField.visibleLayer;
        salve += 1;

        if (salve == 3)
        {
            salve = 0;
            isFadeOutRotation = true;
            CancelInvoke("TriggerShot");
        }
    }

    private void FadeOutRotation()
    {
        rotationTempSpeed --;

        if (rotationTempSpeed <= 1)
        {
            rotationTempSpeed = rotationSpeed;
            isRotating = false;
            isFadeOutRotation = false;
        }
    }
}
