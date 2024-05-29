using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderChangeWeaponColor : MonoBehaviour
{
    public int teamColor;

    public SpriteRenderer weaponplatform;
    public SpriteRenderer minigun, bazooka, airstike;


    public List<Sprite> weaponplatforms, miniguns, bazookas, airstikes;

    private void Start()
    {
        teamColor = gameObject.GetComponentInParent<SpiderChangeTeamColor>().teamColor;

        if (weaponplatform != null) weaponplatform.sprite = weaponplatforms[teamColor];
        if (minigun != null) minigun.sprite = miniguns[teamColor];
        if (bazooka != null) bazooka.sprite = bazookas[teamColor];
        if (airstike != null) airstike.sprite = airstikes[teamColor];


    }

}
