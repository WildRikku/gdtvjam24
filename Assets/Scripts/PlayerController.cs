using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject mainWeapon;
    private Weapon _weapon;

    void Start()
    {
        GameObject weaponInstance = Instantiate(mainWeapon);
        _weapon = weaponInstance.GetComponent<Weapon>();
    }

    public void Attack()
    {
        _weapon.Trigger();
    }
}
