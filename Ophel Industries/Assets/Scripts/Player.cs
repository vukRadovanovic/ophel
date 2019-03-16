using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, Action.IMoveable {

  public bool isMoving { get; set; }
  public bool affectedByPause { get; set; }
  private float speakerCheckDist = 0.2f;

	void Awake() {
    isMoving = true;
    affectedByPause = false;
	}
	
	void Update() {
    // talk to interactable person or object after pressing space
	  if (Input.GetKeyDown(KeyCode.Space) && 
        !Dialogue.Controller.Instance.isTalking) {
      Speaker speaker = DetectSpeaker();

      if (speaker != null) {
        // let speaker start dialogue
        speaker.Speak();
      }
    }  
    else if (Input.GetKeyDown(KeyCode.Space) &&
        Dialogue.Controller.Instance.isTalking) {
      Dialogue.Controller.Instance.HandleInput();
    }

    // stop movement while the player is in a conversation
    if (Dialogue.Controller.Instance.isTalking && isMoving) {
        isMoving = false;
        StopMovement();
    }
    else if (!Dialogue.Controller.Instance.isTalking && !affectedByPause &&
             !isMoving) {
        // FIXME: this is dangerous since the player will always be able to move
        //        if they are not in a conversation and frozen by some other
        //        method
        isMoving = true;
        StartMovement();
    }
	}

  Speaker DetectSpeaker() {
    Collider2D playerCollider = this.GetComponent<Collider2D>();
    Vector2 frontOfPlayer = (Vector2)transform.position + Vector2.up *
        (playerCollider.bounds.size.magnitude / 2.0f);

    // send raycast out in front of the player to check if there is a speaker
    RaycastHit2D hit = Physics2D.Raycast(frontOfPlayer, Vector2.up,
                                         speakerCheckDist);

    // check for raycast collision in front of player
    if (hit.collider != null) {
      Speaker collidedSpeaker = hit.collider.GetComponent<Speaker>();
      return collidedSpeaker; 
    }

    return null;
  }

  /**
   * Enable movement.
   */
  public void StartMovement() {
    Debug.Log("Starting player movement");
    gameObject.GetComponent<PlayerMovement>().enabled = true;
  }

  /**
   * Freeze and disable movement.
   */
  public void StopMovement() {
    Debug.Log("Stopping player movement");
    gameObject.GetComponent<PlayerMovement>().enabled = false;
    gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
  }
}
