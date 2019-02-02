using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue {
  // TODO: use singleton pattern
  public class Controller : MonoBehaviour {
    public Text nameDisplay;
    public Text phraseDisplay;
    public GameObject boxDisplay;

    private Dialogue.Manager manager;
    private string name;
    private string phrase;
    private float typeSpeed;
    private bool finishedDisplay;
    private bool isOpen;

    /**
     * Find the dialogue manager.
     */
    void Awake() {
      manager = GameObject.Find("GameManager").GetComponent<Dialogue.Manager>();
      isOpen = false;
      finishedDisplay = true;

      if (manager == null) {
        Debug.Log("Dialogue manager not found");
      }
    }
    
	  /**
     * Prepare display for the first message of the conversation.
     */
	  void Start() {
      CloseMessageDisplay();
	  }
	
	  /**
     * Check if the current message has finished displaying and if the user
     * wants to move on to the next message.
     */
	  void Update() {
      if (phraseDisplay.text == phrase) {
        HandleMessageFinished();
      }
		  if (Input.GetKeyDown(KeyCode.Space) && finishedDisplay) {
        if (isOpen) {
          HandleContinue();
        }
        else if (!manager.IsFinished()) {
          OpenMessageDisplay();
          DisplayCurrentMessage();
        }
      }
	  }

    /**
     * Open the message display.
     */
    void OpenMessageDisplay() {
      nameDisplay.gameObject.SetActive(true);
      phraseDisplay.gameObject.SetActive(true);
      boxDisplay.gameObject.SetActive(true);
      isOpen = true;
      // Time.timeScale = 0;
    }

    /**
     * Hide the message display.
     */
    void CloseMessageDisplay() {
      nameDisplay.gameObject.SetActive(false);
      phraseDisplay.gameObject.SetActive(false);
      boxDisplay.gameObject.SetActive(false);
      isOpen = false;
      // Time.timeScale = 1;
    }

    /**
     * Update instance variable(s) given that the message has finished
     * displaying. 
     */
    void HandleMessageFinished() {
        finishedDisplay = true;
    }

    /**
     * Move the user to the next message in the conversation or close the
     * box if no messages are left given that the user wants to continue.
     */
    void HandleContinue() {
        manager.MoveNext();

        if (manager.IsFinished()) {
          CloseMessageDisplay();
        }
        else {
          DisplayCurrentMessage();
        }
    }
    
    /**
     * Display the current message after fetching the current message and
     * resetting the display.
     */
    void DisplayCurrentMessage() {
        UpdateMessageFields();
        ResetMessageDisplay();
        StartCoroutine(UpdateMessageDisplay());
    }

    /**
     * Update the instance variables for the name, phrase, and type speed of
     * the current message.
     */
    void UpdateMessageFields() {
        Message currMessage = manager.CurrentMessage();
        name = currMessage.name;
        phrase = currMessage.phrase;
        typeSpeed = currMessage.typeSpeed;
    }

    /**
     * Clear the message display.
     */
    void ResetMessageDisplay() {
        nameDisplay.text = "";
        phraseDisplay.text = "";
        finishedDisplay = false;
    }

    /**
     * Display the message with a typing effect for the phrase.
     */
    IEnumerator UpdateMessageDisplay() {
      nameDisplay.text = name;

      foreach (char letter in phrase.ToCharArray()) {
        phraseDisplay.text += letter;
        yield return new WaitForSeconds(typeSpeed);
      }
    }
  }
}
