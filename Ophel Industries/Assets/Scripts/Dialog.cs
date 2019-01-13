using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;

/**
 * Shows the separate phrases of dialogue at fixed speeds for each phrase. 
 */
public class Dialog : MonoBehaviour {
  public TextMeshProUGUI textDisplay;
  public string[] phrases;
  private int index;
  public float[] textSpeeds;
  private bool canContinue;

	// Use this for initialization
	void Start () {
	  StartCoroutine(Type());	
    canContinue = false;
	}
 
  void Update () {
    // check if the full text of the current phrase has been shown
    if (textDisplay.text == phrases[index]) {
        canContinue = true;
    }
    
    // show the text for the next phrase
    if (Input.GetKeyDown(KeyCode.Space)) {
        if (canContinue) {
          NextSentence();
        }
    }
  }

  IEnumerator Type() {
    // show the text for the phrase one letter at a time after delaying for
    // a certain amount of time
    foreach (char letter in phrases[index].ToCharArray()) {
      textDisplay.text += letter;
      yield return new WaitForSeconds(textSpeeds[index]);
    }
  }

  public void NextSentence() {
    canContinue = false;

    // start displaying the next phrase
    if (index < phrases.Length - 1) {
      index++;
      textDisplay.text = "";
      StartCoroutine(Type());
    }
  }
}
