using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosivObjects : MonoBehaviour
{
    private float health = 5;
    public GameObject explosionPrefab;
    private bool _isExplode = false;
    public Collider2D collider_;
    public Rigidbody2D rb;

    public void TakeDamage(float damage)
    {
/*        Debug.Log("Christian hilf mir bitte :(");
        Debug.Log("Kannst du bitte machen, dass das fass explodiert und Schaden macht?");
        Debug.Log("ich bin zu dumm f�r das ExplodeOnImpact script");
        Debug.Log("ich versteh nicht, woher ExplodeOnImpact primaryLayer bekommt. Ich wollte �ber das Fass ein ExplodeObject erzeugen. Bekomm dann aber ne NullReferenz auf primaryLayer");
  */       
    }
}
