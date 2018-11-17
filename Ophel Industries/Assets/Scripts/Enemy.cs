using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public string name;
    private Vector2 position;
    public Vector2 direction = new Vector2(-1.0f, 0.0f);
    public float coneAngle = Mathf.PI*(15/180);
    public float radius = 2.0f;
    private bool playerDetected = false;
    private float speed = 0.05f;

    // Use this for initialization
    void Start () {
        Vector3 objPosition = GameObject.Find(name).transform.position;
        position = new Vector2(objPosition.x, objPosition.y);
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 playerPosition = GameObject.Find("Player").transform.position;
        Vector2 playerPos = new Vector2(playerPosition.x, playerPosition.y);

        Vector3 objPosition = GameObject.Find(name).transform.position;
        position = new Vector2(objPosition.x, objPosition.y);

        float distance = (playerPos - position).magnitude;
        float angle = Mathf.Acos(Vector2.Dot((playerPos - position), direction) / ((playerPos - position).magnitude * direction.magnitude));

        if(distance < radius && angle < coneAngle)
        {
            playerDetected = true;
        }
        else
        {
            playerDetected = false;
        }

        if (playerDetected)
        {
            chasePlayer(playerPos);
        }
    }

    void chasePlayer(Vector2 playerPos)
    {
        Vector2 movDirection = playerPos - position;
        Vector3 pos = new Vector3(position.x, position.y);
        GameObject.Find(name).transform.Translate(-pos);
        GameObject.Find(name).transform.Rotate(0, 0, Mathf.Acos(Vector2.Dot(movDirection, direction) / (movDirection.magnitude * direction.magnitude)));
        GameObject.Find(name).transform.Translate(pos);
        direction = movDirection.normalized;

        GameObject.Find(name).transform.Translate(new Vector3(direction.x, direction.y, 0.0f) * speed);
    }


}
