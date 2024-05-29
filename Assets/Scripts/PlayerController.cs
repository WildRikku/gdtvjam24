using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject mainWeapon;
    private Weapon _weapon;
    public Transform weaponSpawnPoint;
    public event EventHandler AttackFinished;
    public SpiderController spiderController;

    void Start()
    {
        GameObject weaponInstance = Instantiate(mainWeapon, weaponSpawnPoint);
        _weapon = weaponInstance.GetComponent<Weapon>();
    }

    public void Attack()
    {
        _weapon.isActive = true;
        spiderController.isActive = true;
        _weapon.AttackFinished += WeaponOnAttackFinished;
    }

    private void WeaponOnAttackFinished(object sender, EventArgs e)
    {
        _weapon.isActive = false;
        spiderController.isActive = false;
        AttackFinished?.Invoke(this, EventArgs.Empty);
    }
}
