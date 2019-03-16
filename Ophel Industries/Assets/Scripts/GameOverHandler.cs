using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour {

    public Text gameOver;

	// Use this for initialization
	void Start () {
        gameOver.text = string.Empty;
        
    }
	
	// Update is called once per frame
	void Update () {
        if (AStarAI.caughtPlayer) {
            GameOver(gameOver);
        }
        if(PlayerMovement.movementEnabled == false) {
            if (Input.GetKey(KeyCode.Space)) {
                //SceneManager.UnloadSceneAsync("StartScene");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
	}

    static void GameOver(Text gameOver) {
        gameOver.text = "Game Over!";
        PlayerMovement.movementEnabled = false;
    }
}
