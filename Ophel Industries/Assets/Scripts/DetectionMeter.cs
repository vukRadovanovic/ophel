using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectionMeter : MonoBehaviour {

    public int detectionRate;
    public Text detectionText;
    float detectedTime;
    GameObject attachedObj;

    public DetectionMeter(Text text, GameObject obj) {
        detectionText = text;
        detectionRate = 0;
        detectedTime = 0.0f;
        attachedObj = obj;
    }

	// Use this for initialization
	void Start () {
        detectionRate = 0;
        detectedTime = 0.0f;
	}
	
	// Update is called once per frame
	public void Update () {
        detectionText.transform.position = attachedObj.transform.position + Vector3.up;
		if(detectionRate < 25) {
            detectedTime = -1;
            detectionText.text = string.Empty;
        }
        else if(detectionRate < 50) {
            detectedTime = -1;
            detectionText.text = "?";
        }
        else if (detectionRate < 75) {
            detectedTime = -1;
            detectionText.text = "??";
        }
        else if (detectionRate < 100) {
            detectedTime = -1;
            detectionText.text = "???";
        }
        else { // Detected
            if(detectedTime == -1) {
                detectedTime = Time.time;
            }
            if (Time.time - detectedTime < 1) {
                detectionText.text = "!";
            }
            else {
                detectionText.text = string.Empty;
            }
        }
    }
}
