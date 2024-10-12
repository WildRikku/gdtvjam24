using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : SpiderController {
    private GameController _gameController;
    public PlayerController playerController;

    private int movementPhase = 0;
    private int actionState = 0;

    private bool velocityFlag = false;
    private Vector3 _targetVector;
    private bool _isActive;

    private float xDir;
    public override bool IsActive {
        get => _isActive;
        set {
            if (value && value != _isActive) {
                movementPhase = 1;
                actionState = 1;
                Invoke(nameof(EndMovement), 15f);
                // pick a target zone
                Bounds targetZone = _gameController.movementZones[Random.Range(0, _gameController.movementZones.Count)].bounds;
                _targetVector = RandomVector(targetZone.min, targetZone.max);
                _gameController.debugMarker.position = _targetVector;
                // oversteer height because falling
                _targetVector.y *= 1.5f;
            }
            _isActive = value;
        }

    }

    // Start is called before the first frame update
    private new void Awake() {
        base.Awake();
        _gameController = GameObject.Find("GameManagement").GetComponent<GameController>();
    }

    // Update is called once per frame
    private new void FixedUpdate() {
        if (!IsActive) {
            return;
        }

        switch (actionState) {
            case 1:
                HandleMovement();
                break;
            case 2:
                // wait for landing if necessary
                if (Mathf.Abs(_rb.velocity.y) < Mathf.Epsilon)
                    actionState = 3;
                    break;
            case 3:
                // activate dynamite
                // TOOD: chose weapon
                _gameController.ChangeWeapon(1);
                actionState = 4;
                break;
            case 4:
                // fire
                // TODO ineffizient
                float targetAngle = xDir > 0 ? 120f : 240f;
                if (playerController.weapon.GetType() == typeof(SpiderBazookaController) || playerController.weapon.GetType() == typeof(SpiderGrenadeController)) {
                    SpiderBazookaController bazWeapon = ((SpiderBazookaController)playerController.weapon);
                    switch (bazWeapon.shootingState) {
                        case ShootingStates.Idle:
                            bazWeapon.NextState();
                            break;
                        case ShootingStates.WaitingForAngle:
                            float tol = 3f;
                            if(targetAngle - tol < bazWeapon.currentZRotation && targetAngle + tol > bazWeapon.currentZRotation) {
                                bazWeapon.NextState();
                            }
                            break;
                        case ShootingStates.WaitingForForce:
                            float forceTol = .3f;
                            float targetForce = .45f;
                            if (targetForce - forceTol < bazWeapon.shootingForceFactor && targetForce + forceTol > bazWeapon.shootingForceFactor) {
                                bazWeapon.NextState();
                                actionState = 5;
                            }
                            break;
                    }
                }
                else if (playerController.weapon.GetType() == typeof(SpiderMinigunController)) {
                    SpiderMinigunController minigun = ((SpiderMinigunController)playerController.weapon);
                    switch (minigun.shootingState) {
                        case ShootingStates.Idle:
                            minigun.NextState();
                            break;
                        case ShootingStates.WaitingForAngle:
                            float tol = 3f;
                            if (targetAngle - tol < minigun.currentZRotation && targetAngle + tol > minigun.currentZRotation) {
                                minigun.NextState();
                                actionState = 5;
                            }
                            break;
                    }
                }
                else {
                    playerController.weapon.Trigger();
                    actionState = 5;
                }
                break;
            case 5:
                // retreat from dynamite
                MoveSideways(-xDir);
                break;
        }
    }

    private void HandleMovement() {
        Vector3 distance = _targetVector - transform.position;
        xDir = Mathf.Sign(distance.x);

        // Cycle through flying and walking
        if (Mathf.Abs(distance.x) > 0.5f) {
            switch (movementPhase) {
                case 1:
                    // try flying
                    MoveSideways(xDir);

                    if (distance.y > 0.5f) {
                        MoveUp();

                        if (velocityFlag == false && _rb.velocity.x > 0.5f) {
                            // we are moving
                            velocityFlag = true;
                        }

                        if (velocityFlag == true && _rb.velocity.x < 0.3f) {
                            // we crashed into a wall
                            velocityFlag = false;
                            movementPhase = 2;
                        }
                    }
                    break;
                case 2:
                    // fall down
                    if (Mathf.Abs(_rb.velocity.y) < Mathf.Epsilon) {
                        movementPhase = 3;
                    }
                    break;
                case 3:
                    // try walking
                    MoveSideways(xDir);

                    if (velocityFlag == false && _rb.velocity.x > 0.5f) {
                        // we are moving
                        velocityFlag = true;
                    }

                    if (velocityFlag == true && _rb.velocity.x < 0.3f) {
                        // we crashed into a wall
                        velocityFlag = false;
                        movementPhase = 1;
                    }
                    break;
            }
        }
        else {
            CancelInvoke(nameof(EndMovement));
            actionState = 2;
        }
    }

    private void EndMovement() {
        actionState = 2;
    }

    private Vector3 RandomVector(Vector2 min, Vector2 max) {
        return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), 0);
    }
}
