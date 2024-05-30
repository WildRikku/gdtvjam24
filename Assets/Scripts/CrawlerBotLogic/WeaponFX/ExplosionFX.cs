using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFX : MonoBehaviour
{
    private Animator expAnimator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        expAnimator = GetComponent<Animator>();
        expAnimator.SetTrigger("TrExplode");
        spriteRenderer = GetComponent<SpriteRenderer>();

        Invoke(nameof(DestroyFX), 5f);
    }

    private void Update()
    {
        var animStateInfo = expAnimator.GetCurrentAnimatorStateInfo(0);
        var NTime = animStateInfo.normalizedTime;
        if (NTime > 0.95) spriteRenderer.enabled = false;
    }

    private void DestroyFX()
    {
        Destroy(gameObject);
    }

   
}