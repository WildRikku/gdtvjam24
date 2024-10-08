using System;
using UnityEngine;

public class SpiderController : MonoBehaviour {
    public float speed = 1f;
    protected Rigidbody2D _rb;
    public virtual bool IsActive { get; set; }

    public ParticleSystem[] boostParticleSystem;
    private bool _canTriggerBoostsound = true;

    protected void Awake() {
        _rb = GetComponent<Rigidbody2D>();

        //TODO schön machen
        _rb.AddForce(Vector2.right * 50f);
        Invoke(nameof(TODOSmallForce), 1f);
    }

    private void TODOSmallForce() {
        _rb.AddForce(Vector2.up * 100f);
    }

    protected virtual void FixedUpdate() {
        if (!IsActive) {
            return;
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            MoveSideways();
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            MoveSideways(-1f);
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            MoveUp();
        }
    }

    protected void MoveSideways(float sign = 1f) {
        if (Mathf.Abs(_rb.velocity.x) < speed) {
            _rb.AddForce(sign * Vector2.right * 50f);
        }
    }

    protected void MoveUp() {
        _rb.AddForce(Vector2.up * 25f);
        boostParticleSystem[0].Emit(1);
        boostParticleSystem[1].Emit(1);

        if (_canTriggerBoostsound == true) {
            Invoke(nameof(ResetTriggerBoostSound), .4f);
            _canTriggerBoostsound = false;
            AudioManager.Instance.PlaySFX("BotBoost");
        }
    }

    private void ResetTriggerBoostSound() {
        _canTriggerBoostsound = true;
    }
}