using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpiderAirstrikeController : Weapon
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
        for (short i = 1; i < 5; i++)
        {
            GameObject rocket = Instantiate(rocketPrefab, new Vector3(Random.Range(0, battleField.width), battleField.height), Quaternion.identity, transform);
            ExplodeOnImpact eoi = rocket.GetComponent<ExplodeOnImpact>();
            eoi.primaryLayer = battleField.collidableLayer;
            eoi.secondaryLayer = battleField.visibleLayer;
        }
        OnAttackFinished();
    }
}
