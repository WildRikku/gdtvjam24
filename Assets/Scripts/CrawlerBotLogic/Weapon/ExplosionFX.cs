using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFX : MonoBehaviour
{
    private Animator expAnimator;

    void Start()
    {
        expAnimator = GetComponent<Animator>();
        expAnimator.SetTrigger("TrExplode");

        Invoke(nameof(DestroyFX), 5f);
    }

    private void DestroyFX()
    {
        Destroy(gameObject);
    }

   
}