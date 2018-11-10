using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

  public float moveForce;
  public Rigidbody2D rb2D;

  // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {}

  // handle physics
  void FixedUpdate() {
    HandleHorizontal();
    HandleVertical();
  }

  // handle horizontal movement
  void HandleHorizontal() {
    float horizMoveDir = Input.GetAxis("Horizontal");
    
    // Time.deltaTime makes our player move at the same rate regardless of
    // the computer's frame rate
    // ForceMode2D.Impulse will make the player move without taking mass into
    // consideration - i.e. snappy movement
    rb2D.AddForce(Vector2.right * horizMoveDir * moveForce * Time.deltaTime,
        ForceMode2D.Impulse);
  }

  // handle vertical movement
  void HandleVertical() {
    float vertMoveDir = Input.GetAxis("Vertical"); 

    rb2D.AddForce(Vector2.up * vertMoveDir * moveForce * Time.deltaTime,
        ForceMode2D.Impulse);
  }
}
