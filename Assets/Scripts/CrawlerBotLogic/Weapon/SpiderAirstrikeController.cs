using UnityEngine;
using Random = UnityEngine.Random;

public class SpiderAirstrikeController : ProjectileWeapon {
    public GameObject rocketPrefab;

    private new void Start() {
        base.Start();
    }

    private void Update() {
        if (!isActive) {
            return;
        }

        if (Input.GetKeyDown(activationKey)) {
            AudioManager.Instance.PlaySFX("AirstrikeLoad");
            isActive = false;
            Trigger();
        }
    }

    public override void Trigger() {
        ProjectileCount =
            4; // set beforehand so we don't stop the attack if all spawned projectiles crash before all have been spaweed
        for (short i = 1; i < 5; i++) {
            Invoke(nameof(Shoot), 0.2f * i);
        }
    }

    private void Shoot() {
        AudioManager.Instance.PlaySFX("AirstrikeSpawn");
        SpawnProjectile(rocketPrefab, new(Random.Range(0, battleField.width), battleField.height), Quaternion.identity);
    }
}