using UnityEngine;

public class SpiderMinigunController : MonoBehaviour
{
    public Transform weaponRotationPoint;
    public Transform shotTriggerPoint;
    public GameObject bulletPrefab;

    public float minZRotation = -20f;
    public float maxZRotation = 200f;
    public float rotationSpeed = 10f;
    public KeyCode activationKey = KeyCode.A;

    public ParticleSystem muzzleParticle;
    private BattleField battleField;
    private bool isFadeOutRotation;

    private bool isRotating;
    private bool rotatingToMax = true;
    private float rotationTempSpeed;
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

    private void Rotate()
    {
        float currentZRotation = weaponRotationPoint.localEulerAngles.z;

        if (rotatingToMax)
        {
            currentZRotation = Mathf.MoveTowards(currentZRotation, maxZRotation, rotationTempSpeed * Time.deltaTime);

            if (currentZRotation >= maxZRotation) rotatingToMax = false;
        }
        else
        {
            currentZRotation = Mathf.MoveTowards(currentZRotation, minZRotation, rotationTempSpeed * Time.deltaTime);

            if (currentZRotation <= minZRotation) rotatingToMax = true;
        }

        Vector3 localEulerAngles = weaponRotationPoint.localEulerAngles;
        localEulerAngles = new Vector3(localEulerAngles.x,
            localEulerAngles.y, currentZRotation);
        weaponRotationPoint.localEulerAngles = localEulerAngles;
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
            CancelInvoke(nameof(TriggerShot));
        }
    }

    private void FadeOutRotation()
    {
        rotationTempSpeed--;

        if (rotationTempSpeed <= 1)
        {
            rotationTempSpeed = rotationSpeed;
            isRotating = false;
            isFadeOutRotation = false;
        }
    }
}