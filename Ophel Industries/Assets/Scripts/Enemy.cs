using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public string name;
    private Vector2 position;
    public Vector2 direction = new Vector2(-1.0f, 0.0f);
    public float coneAngle = Mathf.PI * (15 / 180);
    public float radius = 2.0f;
    private bool playerDetected = false;
    private float speed = 0.05f;

    // Use this for initialization
    void Start() {
        Vector3 objPosition = GameObject.Find(name).transform.position;
        position = new Vector2(objPosition.x, objPosition.y);
    }

    // Update is called once per frame
    void Update() {
        GameObject player = GameObject.Find("Player");
        Vector2 playerPosition = player.transform.position;

        float distance = (playerPosition - position).magnitude;
        float angle = Mathf.Acos(Vector2.Dot((playerPosition - position), direction) / ((playerPosition - position).magnitude * direction.magnitude));
        if (distance < radius && angle < coneAngle) {
            Collider2D collider = player.GetComponent<Collider2D>();
            float x = collider.bounds.extents.x;
            float y = collider.bounds.extents.y;
            Vector2 upperLeft = position + new Vector2(-x, y);
            Vector2 lowerLeft = position - (Vector2)collider.bounds.extents;
            Vector2 upperRight = position + (Vector2)collider.bounds.extents;
            Vector2 lowerRight = position + new Vector2(x, -y);
            RaycastHit2D raycastHit = Physics2D.Raycast(position, upperLeft - position);
            GameObject enemy = GameObject.Find(name);
            Collider2D col = enemy.GetComponent<Collider2D>();
            if (raycastHit.collider.name.Equals("Player")) {
                playerDetected = true;
            }
            else { //since enemy is a collidable object, when rays are cast from the center of the enemy they hit the boundaries of the enemy first
                   // so we need to cast a ray from a circle outside of square (the square is inscribed in circle)
                   // casting 5 rays from the "center" of the enemy to corners and the center point of the player
                raycastHit = Physics2D.Raycast(position + (lowerLeft - position).normalized * col.bounds.extents.magnitude, lowerLeft - position);
                if (raycastHit.collider.name.Equals("Player")) {
                    playerDetected = true;
                }
                else {
                    raycastHit = Physics2D.Raycast(position + (upperRight - position).normalized * col.bounds.extents.magnitude, upperRight - position);
                    if (raycastHit.collider.name.Equals("Player")) {
                        playerDetected = true;
                    }
                    else {
                        raycastHit = Physics2D.Raycast(position + (lowerRight - position).normalized * col.bounds.extents.magnitude, lowerRight - position);
                        if (raycastHit.collider.name.Equals("Player")) {
                            playerDetected = true;
                        }
                        else {
                            raycastHit = Physics2D.Raycast(position + (playerPosition - position).normalized * col.bounds.extents.magnitude, playerPosition - position);
                            if (raycastHit.collider.name.Equals("Player")) {
                                playerDetected = true;
                            }
                            else {
                                playerDetected = false;
                            }
                        }
                    }
                }
            }
        }
        else {
            playerDetected = false;
        }
        if (playerDetected) {
            chasePlayer(playerPosition);
        }
    }

    void chasePlayer(Vector2 playerPos) {
        Vector2 movDirection = playerPos - position;
        //Vector3 pos = new Vector3(position.x, position.y);
        //GameObject.Find(name).transform.Translate(-pos);
        //GameObject.Find(name).transform.Rotate(0, 0, Mathf.Acos(Vector2.Dot(movDirection, direction) / (movDirection.magnitude * direction.magnitude)));
        //GameObject.Find(name).transform.Translate(pos);
        Debug.Log(movDirection);
        Debug.Log(playerPos);
        direction = movDirection.normalized;
        GameObject.Find(name).transform.Translate(new Vector3(direction.x, direction.y, 0.0f) * speed);
        position = GameObject.Find(name).transform.position;
    }

}
