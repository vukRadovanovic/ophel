using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue {
  public class Controller : MonoBehaviour {
    private static Controller _instance;
    public static Controller Instance { get { return _instance; } }

    public Text nameDisplay;
    public Text phraseDisplay;
    public GameObject boxDisplay;
    public bool isTalking;

    private string name;
    private string phrase;
    private float typeSpeed;
    private bool finishedDisplay;
    private bool isOpen;

    /**
     * Enforce singleton pattern and initialize instance variables. 
     */
    void Awake() {
      // check if instance has been initialized
      if (_instance == null) {
        _instance = this;
      }
      // destroy any other instances
      else if (_instance != this) {
        Destroy(gameObject);
      }

      // allow this gameobject to persist across scenes
      DontDestroyOnLoad(gameObject);

      isOpen = false;
      finishedDisplay = true;
      isTalking = false;
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
      /*
      if (phraseDisplay.text == phrase) {
        HandleMessageFinished();
      }

      if (Input.GetKeyDown(KeyCode.Space) && isOpen && finishedDisplay) {
        HandleContinue();
      }
      else if (Dialogue.Manager.Instance.openDialogue) {
          Debug.Log("Want to open dialogue");
          Dialogue.Manager.Instance.openDialogue = false;
          OpenMessageDisplay();
          DisplayCurrentMessage();
      }
      */
	  }

    public void HandleInput() {
      if (phraseDisplay.text == phrase) {
        HandleMessageFinished();
      }

      if (isOpen && finishedDisplay) {
        HandleContinue();
      }
      else if (!isOpen) {
        OpenMessageDisplay();
        DisplayCurrentMessage();
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
      isTalking = true;
    }

    /**
     * Hide the message display.
     */
    void CloseMessageDisplay() {
      nameDisplay.gameObject.SetActive(false);
      phraseDisplay.gameObject.SetActive(false);
      boxDisplay.gameObject.SetActive(false);
      isOpen = false;
      isTalking = false;
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
        Dialogue.Manager.Instance.MoveNext();

        if (Dialogue.Manager.Instance.IsFinished()) {
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
        Message currMessage = Dialogue.Manager.Instance.CurrentMessage();
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
