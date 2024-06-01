using UnityEngine;

public class SpiderController : MonoBehaviour {
    public float speed = 1f;
    private Rigidbody2D _rb;
    public bool isActive;
    public ParticleSystem[] boostParticleSystem;
    private bool _canTriggerBoostsound = true;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();

        //TODO
        _rb.AddForce(Vector2.right * 50f);
        Invoke(nameof(TODOSmallForce), 1f);
    }

    private void TODOSmallForce() {
        _rb.AddForce(Vector2.up * 100f);
    }

    private void FixedUpdate() {
        if (!isActive) {
            return;
        }

        if (Mathf.Abs(_rb.velocity.x) < speed) {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
                _rb.AddForce(Vector2.right * 50f);
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
                _rb.AddForce(-Vector2.right * 50f);
            }
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            _rb.AddForce(Vector2.up * 25f);
            boostParticleSystem[0].Emit(1);
            boostParticleSystem[1].Emit(1);

            if (_canTriggerBoostsound == true) {
                Invoke(nameof(ResetTriggerBoostSound), .4f);
                _canTriggerBoostsound = false;
                AudioManager.Instance.PlaySFX("BotBoost");
            }
        }
    }

    private void ResetTriggerBoostSound() {
        _canTriggerBoostsound = true;
    }
}