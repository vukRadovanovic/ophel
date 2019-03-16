using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float moveForce;
    public Rigidbody2D rb2D;
    public static bool movementEnabled;
    public Animator animator;
    private bool holdingCrouch;
    private bool holdingRun;

    // Use this for initialization
    void Awake() {
        holdingCrouch = false;
        holdingRun = false;
        movementEnabled = true;
    }

    // Update is called once per frame
    void Update() {
        if(movementEnabled == false) {
            return;
        }
        holdingRun = Input.GetKey(KeyCode.LeftShift);
        holdingCrouch = Input.GetKey(KeyCode.LeftControl);
    }

    // handle physics
    void FixedUpdate() {
        if (movementEnabled == false) {
            return;
        }
        HandleMovement();
    }

    void HandleMovement() {
        float modMoveForce = moveForce;

        if (holdingCrouch) {
            modMoveForce = moveForce * 0.5f;
        }
        else if (holdingRun) {
            modMoveForce = moveForce * 1.5f;
        }

        rb2D.velocity = new Vector2(Input.GetAxis("Horizontal") * modMoveForce,
            Input.GetAxis("Vertical") * modMoveForce);

        animator.SetFloat("Speed.X", rb2D.velocity.x);
        animator.SetFloat("Speed.Y", rb2D.velocity.y);
        animator.SetBool("YGreaterThanX", 
            Mathf.Abs(rb2D.velocity.y) > Mathf.Abs(rb2D.velocity.x) ? true : false);

    }

}
