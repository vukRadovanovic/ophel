using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, Action.IMoveable {

  public bool isMoving { get; set; }
  public bool affectedByPause { get; set; }

	void Awake() {
    isMoving = true;
    affectedByPause = false;
	}
	
	void Update() { }

   /**
   * Enable movement.
   */
  public void StartMovement() {
    Debug.Log("Starting enemy movement");
    gameObject.GetComponent<AStarAI>().enabled = true;
  }

  /**
   * Freeze and disable movement.
   */
  public void StopMovement() {
    Debug.Log("Stopping enemy movement");
    gameObject.GetComponent<AStarAI>().enabled = false;
    gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
  }
}
