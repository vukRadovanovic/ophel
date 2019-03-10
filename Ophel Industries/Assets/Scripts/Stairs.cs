//10/03/19
//Stairs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Stairs : MonoBehaviour
{
    public bool isUp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            string sceneName = SceneManager.GetActiveScene().name;

            // Unload the current scene
            SceneManager.UnloadSceneAsync(sceneName);
            string[] sceneWords = sceneName.Split(' ');
            
            // Increments the scene number to the next scene depeending if we going up or down
            sceneWords[sceneWords.Length - 1] = 
                (int.Parse(sceneWords[sceneWords.Length - 1]) + (isUp ? 1:-1)).ToString();

            // Load next scene
            SceneManager.LoadSceneAsync(string.Join(" ", sceneWords));

        }
    }
}
