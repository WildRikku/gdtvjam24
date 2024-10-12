using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpiderDynamiteController : ProjectileWeapon {
    [FormerlySerializedAs("rocketPrefab")]
    public GameObject dynamitePrefab;

    private new void Awake() {
        base.Awake();
    }

    private void Update() {
        if (!isActive) {
            return;
        }

        if (!isAiControled && Input.GetKeyDown(activationKey)) {
            Trigger();
        }
    }

    public override void Trigger() {
        AudioManager.Instance.PlaySFX("GranadeShoot");
        isActive = false;
        ProjectileCount = 1; // set beforehand so we don't stop the attack if all spawned projectiles crash before all have been spaweed
        SpawnProjectile(dynamitePrefab, transform.position, Quaternion.identity);
    }
}