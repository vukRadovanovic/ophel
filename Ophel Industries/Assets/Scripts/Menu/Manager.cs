using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu {
  public class Manager : MonoBehaviour {
    private static Menu.Manager _instance;
    public static Menu.Manager Instance { get { return _instance; } }

    /**
     * Enforce singleton pattern.
     */
	  void Awake () {
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
    }
	
	  void Update () {
      // quit to main menu if escape key is pressed
      if (Input.GetKeyDown(KeyCode.Escape)) {
        Debug.Log("Quitting to main menu");
        Application.LoadLevel("StartMenuScene");
      }
    }

    public void ShowNextScene(string sceneName) {
      Application.LoadLevel(sceneName);
    }

    public void ShowPauseMenu() {
      Transform pauseMenu = gameObject.transform.Find("PauseMenuCanvas");
      pauseMenu.GetComponent<Canvas>().enabled = true;
    }

    public void HidePauseMenu() {
      Transform pauseMenu = gameObject.transform.Find("PauseMenuCanvas");
      pauseMenu.GetComponent<Canvas>().enabled = false;
    }

    public void QuitApplication() {
      Application.Quit();
    }
  }
}
