using UnityEngine;

public class RotatingWeapon : ProjectileWeapon {
    public Transform weaponRotationPoint;

    public float minZRotation = -20f;
    public float maxZRotation = 200f;
    public float rotationSpeed = 10f;

    protected bool isRotating;
    private bool _rotatingToMax = true;
    protected float rotationTempSpeed;
    protected bool isFadeOutRotation;
    private Rigidbody2D botRb;

    protected new void Start() {
        botRb = gameObject.GetComponentInParent<Rigidbody2D>();
        rotationTempSpeed = rotationSpeed;
        base.Start();
    }

    public override void Deactivate() {
        isRotating = false;
        base.Deactivate();
    }

    protected void Rotate() {
        float currentZRotation = weaponRotationPoint.localEulerAngles.z;

        if (_rotatingToMax) {
            currentZRotation = Mathf.MoveTowards(currentZRotation, maxZRotation, rotationTempSpeed * Time.deltaTime);

            if (currentZRotation >= maxZRotation) {
                _rotatingToMax = false;
            }
        }
        else {
            currentZRotation = Mathf.MoveTowards(currentZRotation, minZRotation, rotationTempSpeed * Time.deltaTime);

            if (currentZRotation <= minZRotation) {
                _rotatingToMax = true;
            }
        }

        Vector3 localEulerAngles = weaponRotationPoint.localEulerAngles;
        localEulerAngles = new(localEulerAngles.x,
            localEulerAngles.y, currentZRotation);
        weaponRotationPoint.localEulerAngles = localEulerAngles;
    }

    protected void FadeOutRotation() {
        rotationTempSpeed--;

        if (rotationTempSpeed <= 1) {
            rotationTempSpeed = rotationSpeed;
            isRotating = false;
            isFadeOutRotation = false;
        }
    }
    
    protected virtual void Recoil(float force) {
        if (botRb == null) {
            return;
        }

        Vector2 direction = weaponRotationPoint.right * -1;
        Vector2 impulse = direction * force;

        botRb.AddForce(impulse, ForceMode2D.Impulse);
    }
}