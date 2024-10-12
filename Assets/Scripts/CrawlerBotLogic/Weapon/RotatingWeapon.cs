using UnityEngine;

public class RotatingWeapon : ProjectileWeapon {
    public Transform weaponRotationPoint;

    public float minZRotation = -20f;
    public float maxZRotation = 200f;
    public float rotationSpeed = 10f;

    private bool _isActive;
    public override bool isActive {
        get => _isActive;
        set {
            _isActive = value;
            _shootingState = ShootingStates.Idle;
        }
    }

    protected ShootingStates _shootingState = ShootingStates.Idle;
    protected bool isRotating; // not a state but an effect
    private bool _rotatingToMax = true;
    private float _rotationTempSpeed;
    protected bool isFadeOutRotation;
    private Rigidbody2D botRb;

    protected new void Awake() {
        botRb = gameObject.GetComponentInParent<Rigidbody2D>();
        _rotationTempSpeed = rotationSpeed;
        base.Awake();
    }

    protected void Update() {
        if (!isRotating) {
            return;
        }

        Rotate();
        if (isFadeOutRotation) {
            FadeOutRotation();
        }
    }

    public override void Deactivate() {
        ResetRotation();
        base.Deactivate();
    }

    private void Rotate() {
        float currentZRotation = weaponRotationPoint.localEulerAngles.z;

        if (_rotatingToMax) {
            currentZRotation = Mathf.MoveTowards(currentZRotation, maxZRotation, _rotationTempSpeed * Time.deltaTime);

            if (currentZRotation >= maxZRotation) {
                _rotatingToMax = false;
            }
        }
        else {
            currentZRotation = Mathf.MoveTowards(currentZRotation, minZRotation, _rotationTempSpeed * Time.deltaTime);

            if (currentZRotation <= minZRotation) {
                _rotatingToMax = true;
            }
        }

        Vector3 localEulerAngles = weaponRotationPoint.localEulerAngles;
        localEulerAngles = new(localEulerAngles.x,
            localEulerAngles.y, currentZRotation);
        weaponRotationPoint.localEulerAngles = localEulerAngles;
    }

    private void FadeOutRotation() {
        _rotationTempSpeed--;

        if (_rotationTempSpeed <= 1) {
            ResetRotation();
        }
    }

    private void ResetRotation() {
        _rotationTempSpeed = rotationSpeed;
        isRotating = false;
        isFadeOutRotation = false;
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