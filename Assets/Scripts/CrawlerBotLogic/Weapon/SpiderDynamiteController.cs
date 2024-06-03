using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpiderDynamiteController : ProjectileWeapon {
    [FormerlySerializedAs("rocketPrefab")]
    public GameObject dynamitePrefab;

    private new void Start() {
        base.Start();
    }

    private void Update() {
        if (!isActive) {
            return;
        }

        if (Input.GetKeyDown(activationKey)) {
            AudioManager.Instance.PlaySFX("GranadeShoot");
            isActive = false;
            Trigger();
        }
    }

    public override void Trigger() {
        ProjectileCount = 1; // set beforehand so we don't stop the attack if all spawned projectiles crash before all have been spaweed
        SpawnProjectile(dynamitePrefab, transform.position, Quaternion.identity);
    }
}