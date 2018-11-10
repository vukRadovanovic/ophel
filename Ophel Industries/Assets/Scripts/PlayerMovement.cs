using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

  public float moveForce;
  public Rigidbody2D rb2D;

  private bool holdingCrouch;
  private bool holdingRun;

  // Use this for initialization
	void Awake () {
		holdingCrouch = false;
    holdingRun = false;
	}
	
	// Update is called once per frame
	void Update () {
    holdingRun = Input.GetKey(KeyCode.LeftShift);
    holdingCrouch = Input.GetKey(KeyCode.LeftControl);
  }

  // handle physics
  void FixedUpdate() {
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
  }

}
