using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour {
 
  private string speakerTag;
  private int dialogueIndex;

	void Awake() {
    dialogueIndex = 0;
    speakerTag = gameObject.name;
	}
	
	void Update() {
	}

  public void Speak() {
    PrepareDialogue();
    Dialogue.Controller.Instance.HandleInput();
    // TODO: how to handle data with more than one conversation 
    dialogueIndex++;
  }

  private void PrepareDialogue() {
    Dialogue.Manager.Instance.LoadConversation(speakerTag, dialogueIndex);
  }
}
