using UnityEngine;

public class SpiderController : MonoBehaviour
{
    public float speed = 1f;
    private Rigidbody2D rb;
    public bool isActive;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!isActive) return;
        
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
            if (Input.GetKey(KeyCode.Space))
            {
                rb.AddForce(Vector2.up * 25f);
            }

        }
    }
}

