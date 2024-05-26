using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    public float speed = 1f;
    private Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(Mathf.Abs(rb.velocity.x) < speed)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.AddForce(Vector2.right * 50f);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.AddForce(-Vector2.right * 50f);
            }
        }
    }
}

