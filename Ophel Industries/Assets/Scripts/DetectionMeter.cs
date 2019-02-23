using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectionMeter : MonoBehaviour {

    public int detectionRate;
    public Text detectionText;
    float detected;
    GameObject attachedObj;

    public DetectionMeter(Text text, GameObject obj) {
        detectionText = text;
        detectionRate = 0;
        detected = 0.0f;
        attachedObj = obj;
    }

	// Use this for initialization
	void Start () {
        detectionRate = 0;
        detected = 0.0f;
	}
	
	// Update is called once per frame
	public void Update () {
        detectionText.transform.position = attachedObj.transform.position + Vector3.up;
		if(detectionRate < 25) {
            detectionText.text = string.Empty;
        }
        else if(detectionRate < 50) {
            detectionText.text = "?";
        }
        else if (detectionRate < 75) {
            detectionText.text = "??";
        }
        else if (detectionRate < 100) {
            detectionText.text = "???";
        }
        else { // Detected
            detectionText.text = "!";
        }
    }
}
