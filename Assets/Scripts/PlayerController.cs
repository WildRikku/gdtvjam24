using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject mainWeapon;
    private Weapon _weapon;

    void Start()
    {
        _weapon = mainWeapon.GetComponent<Weapon>();
    }

    public void Attack()
    {
        _weapon.Trigger();
    }
}
