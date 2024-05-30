using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpiderAirstrikeController : ProjectileWeapon
{
    public GameObject rocketPrefab;
    
    private void Start()
    {
        battleField = GameObject.Find("GameManagement").GetComponent<BattleField>();
    }

    private void Update()
    {
        if (!isActive) return;
        
        if (Input.GetKeyDown(activationKey))
        {
            isActive = false;
            Trigger();
        }
    }

    public override void Trigger()
    {
        ProjectileCount = 5; // set beforehand so we don't stop the attack if all spawned projectiles crash before all have been spaweed
        for (short i = 1; i < 5; i++)
        {
            GameObject rocket = Instantiate(rocketPrefab, new Vector3(Random.Range(0, battleField.width), battleField.height), Quaternion.identity, transform);
            ExplodeOnImpact eoi = rocket.GetComponent<ExplodeOnImpact>();
            eoi.primaryLayer = battleField.collidableLayer;
            eoi.secondaryLayer = battleField.visibleLayer;
            eoi.Impact += OnProjectileImpact;
        }
    }
}
