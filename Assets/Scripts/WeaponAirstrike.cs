using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponAirstrike : Weapon
{
    public GameObject rocketPrefab;
    public BattleField battleField;

    private void Awake()
    {
        battleField = GameObject.Find("GameManagement").GetComponent<BattleField>();
    }

    public override void Trigger()
    {
        for (short i = 1; i < 5; i++)
        {
            Instantiate(rocketPrefab,
                new Vector3(Random.Range(battleField._width / -2f, battleField._width / 2f), battleField._height / 2f), quaternion.identity);
        }
    }
}
