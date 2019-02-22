using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

  private float speakerCheckDist = 0.2f;

	void Awake() {
		
	}
	
	void Update() {
    // talk to interactable person or object after pressing space
	  if (Input.GetKeyDown(KeyCode.Space) && 
        !Dialogue.Controller.Instance.isTalking) {
      Speaker speaker = SpeakerPresent();

      if (speaker != null) {
        // let speaker start dialogue
        speaker.Speak();
      }
    }  
    else if (Input.GetKeyDown(KeyCode.Space) &&
        Dialogue.Controller.Instance.isTalking) {
      Dialogue.Controller.Instance.HandleInput();
    }

    if (Dialogue.Controller.Instance.isTalking) {
        // freeze the player
        gameObject.GetComponent<PlayerMovement>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
    else {
        gameObject.GetComponent<PlayerMovement>().enabled = true;
    }
	}

  Speaker SpeakerPresent() {
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
}
