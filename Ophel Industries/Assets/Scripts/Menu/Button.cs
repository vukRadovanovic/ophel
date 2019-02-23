using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Menu {
  public class Button : MonoBehaviour {

    public UnityEngine.UI.Button button;
    public string sceneToLoad;

	  void Awake() {}

    void Start() {
      button.onClick.AddListener(LoadScene);
    }

	  void Update() { }

    public void LoadScene() {
      Menu.Manager.Instance.ShowNextScene(sceneToLoad);
    }
  }
}
