using UnityEngine;

public class SpiderGrenadeController : SpiderBazookaController {
    private new void Awake() {
        base.Awake();
    }

    private new void Update() {
        base.Update();
    }

    protected override float CalculateShootingForce() {
        return 2.5f + shootingSpeed * shootingForceFactor * 0.3f;
    }

    protected override void Recoil(float force) {
        // suppress recoil for grenade launcher
        base.Recoil(5);
    }
}