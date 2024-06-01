using UnityEngine;

public class ExplosionFX : MonoBehaviour {
    private Animator expAnimator;
    private SpriteRenderer spriteRenderer;

    public string explosionSound;

    private void Start() {
        AudioManager.Instance.PlaySFX(explosionSound);

        expAnimator = GetComponent<Animator>();
        expAnimator.SetTrigger("TrExplode");
        spriteRenderer = GetComponent<SpriteRenderer>();

        Invoke(nameof(DestroyFX), 5f);
    }

    private void Update() {
        AnimatorStateInfo animStateInfo = expAnimator.GetCurrentAnimatorStateInfo(0);
        float NTime = animStateInfo.normalizedTime;
        if (NTime > 0.95) {
            spriteRenderer.enabled = false;
        }
    }

    private void DestroyFX() {
        Destroy(gameObject);
    }
}