using UnityEngine;
using Random = UnityEngine.Random;

public class SpiderAirstrikeController : ProjectileWeapon {
    public GameObject rocketPrefab;

    private short _firedProjectiles;
    private float _randomX;
    private const short _projectileCount = 4;

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
            _projectileCount; // set beforehand so we don't stop the attack if all spawned projectiles crash before all have been spaweed
        _randomX = Random.Range(0, battleField.width);
        for (short i = 1; i < 5; i++) {
            Invoke(nameof(Shoot), 0.2f * i);
        }
    }

    private void Shoot() {
        AudioManager.Instance.PlaySFX("AirstrikeSpawn");
        SpawnProjectile(rocketPrefab, new(_randomX + 0.25f * _firedProjectiles, battleField.height), Quaternion.identity);
        _firedProjectiles++;
        if (_firedProjectiles == _projectileCount) {
            _firedProjectiles = 0;
        }
    }
}