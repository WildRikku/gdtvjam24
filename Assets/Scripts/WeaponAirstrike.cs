using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponAirstrike : Weapon
{
    public GameObject rocketPrefab;
    public BattleField battleField;

    private void Start()
    {
        battleField = GameObject.Find("GameManagement").GetComponent<BattleField>();
    }

    public override void Trigger()
    {
        for (short i = 1; i < 5; i++)
        {
            GameObject rocket = Instantiate(rocketPrefab, new Vector3(Random.Range(0, battleField._width), battleField._height), Quaternion.identity);
            ExplodeOnImpact eoi = rocket.GetComponent<ExplodeOnImpact>();
            eoi.primaryLayer = battleField.collidableLayer;
            eoi.secondaryLayer = battleField.visibleLayer;
        }
    }
}
