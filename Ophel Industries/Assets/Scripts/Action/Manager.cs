using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Action {
  public class Manager : MonoBehaviour {
    private static Action.Manager _instance;
    public static Action.Manager Instance { get { return _instance; } }

    public bool paused { get; set; }

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
      if (Input.GetKeyDown(KeyCode.P)) {
        HandlePauseInput(); 
      } 
    }

    /**
     * Stop the movement of all characters in the scene.
     */
    void Pause() {
      GameObject characters = GameObject.Find("Characters");

      // find all moveable children
      foreach (Transform child in characters.transform) {
        Action.IMoveable moveableChild = 
            child.gameObject.GetComponent<Action.IMoveable>();

        // stop the child's movement if it is moveable
        if (moveableChild != null && moveableChild.isMoving) {
          Debug.Log(child + " has been paused");
          moveableChild.StopMovement();
          moveableChild.isMoving = false;
          moveableChild.affectedByPause = true;
        }
      }

      paused = true;
      Menu.Manager.Instance.ShowPauseMenu();
    }

    /**
     * Resume the movement of all characters previously paused in the scene.
     */
    void Resume() {
      GameObject characters = GameObject.Find("Characters");

      // find all moveable children
      foreach (Transform child in characters.transform) {
        Action.IMoveable moveableChild =
            child.gameObject.GetComponent<Action.IMoveable>();

        // start the child's movement if it was affected by pause
        if (moveableChild != null && moveableChild.affectedByPause) {
          Debug.Log(child + " has been resumed");
          moveableChild.StartMovement();
          moveableChild.isMoving = true;
          moveableChild.affectedByPause = false;
        }
      }

      paused = false;
      Menu.Manager.Instance.HidePauseMenu();
    }

    /**
     * Pause or unpause the game depending on the current state.
     */
    void HandlePauseInput() {
      if (!paused) {
        Pause();
      }
      else {
        Resume();
      }
    }
  }
}
