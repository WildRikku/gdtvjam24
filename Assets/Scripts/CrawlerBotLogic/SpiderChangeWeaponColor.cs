using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderChangeWeaponColor : MonoBehaviour {
    [HideInInspector]
    public int teamColor;

    public SpriteRenderer weaponplatform;
    public SpriteRenderer weapon;

    public List<Sprite> weaponplatforms, weapons;

    private void Start() {
        teamColor = gameObject.GetComponentInParent<SpiderChangeTeamColor>().teamColor;

        if (weaponplatform != null) {
            weaponplatform.sprite = weaponplatforms[teamColor];
        }

        if (weapon != null) {
            weapon.sprite = weapons[teamColor];
        }
    }
}