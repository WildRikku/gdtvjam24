using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingWeapon : Weapon
{
    public Transform weaponRotationPoint;
    
    public float minZRotation = -20f;
    public float maxZRotation = 200f;
    public float rotationSpeed = 10f;
    
    protected bool isRotating;
    private bool rotatingToMax = true;
    protected float rotationTempSpeed;
    protected bool isFadeOutRotation;
    
    
    
    protected void Rotate()
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
    
    protected void FadeOutRotation()
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
