using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;

public class AStarAI : MonoBehaviour {
	public Transform targetPosition;
	public bool reachedEndOfPath;
	public float speed = 2f;
	public float nextWaypointDistance = 1;
	public Path path;
    public Text detectionText;

    static public bool caughtPlayer;
    
    public float radius;
    public float coneAngle;

	private int currentWaypoint = 0;
	private Vector3 prevTargetPosition;
	private Seeker seeker;
	private bool DEBUG = false;

    private bool wallRotation;
    private bool endOfPathRotation, nextRotation;
    private bool rotated;

    private DetectionMeter detectionMeter;
    // Use this for initialization
    void Start () {
        caughtPlayer = false;
		seeker = GetComponent<Seeker>();

        if (isPlayerVisible()) {
            // start calculating a path to the target
            seeker.StartPath(transform.position, targetPosition.position,
                                         OnPathComplete);
        }

		prevTargetPosition = targetPosition.position;
        rotated = false;
        wallRotation = false;
        endOfPathRotation = false;
        nextRotation = false;
        detectionMeter = new DetectionMeter(detectionText, this.gameObject);
	}

	public void Update () {
        Debug.DrawRay(this.transform.position, this.transform.up, Color.magenta, 0.0f, false);
        detectionMeter.Update();
        //Debug.DrawRay(this.transform.position, direction, Color.blue, 10.0f, false);
        // If player is visible, chase him
        if (isPlayerVisible()) {
            // update the path if the target transform.position has changed
            if (targetPosition.position != prevTargetPosition) {
                prevTargetPosition = targetPosition.position;
                seeker.StartPath(transform.position, targetPosition.position,
                                                  OnPathComplete);
                reachedEndOfPath = false;
                nextRotation = false;
                rotated = false;
                Debug.Log("Started new path");
            }

            Vector3 newDirection = (targetPosition.position - transform.position).normalized; // looking at player
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, newDirection);
            transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
            if(detectionMeter.detectionRate != 100) {
                detectionMeter.detectionRate++;
            }
            Debug.Log(detectionMeter.detectionRate);
        }
        if (path == null) {
            // We have no path to follow yet, so don't do anything
            return;
        }

        // Check in a loop if we are close enough to the current waypoint to switch to the next one.
        // We do this in a loop because many waypoints might be close to each other and we may reach
        // several of them in the same frame.
        reachedEndOfPath = false;
        // distance to the next waypoint in the path
        float distanceToWaypoint;

        while (true) {
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            // next waypoint distance determines how soon the game object will slow down
            if (distanceToWaypoint < nextWaypointDistance) {
                // Check if there is another waypoint or if we have reached the end of the path
                if (currentWaypoint + 1 < path.vectorPath.Count) {
                    currentWaypoint++;
                }
                else {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    reachedEndOfPath = true;
                    if (isPlayerVisible()) {
                        reachedEndOfPath = false;
                    }

                    break;
                }
            }
            else {
                break;
            }
        }

        // slow down smoothly upon approaching the last waypoint of the path
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

        // normalize direction to the next waypoint so that it has a length of 1 world unit
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;

        // multiply the direction by our desired speed to get a velocity
        Vector3 velocity = dir * speed * speedFactor;

        // move the game object to the target
        transform.position += velocity * Time.deltaTime;

        if (reachedEndOfPath) {
            //start rotation function (that is not a function its a coroutine)
            if (isCloseToWall() && !endOfPathRotation && !wallRotation) {
                StartCoroutine(Rotate(Vector3.forward, Mathf.Rad2Deg * coneAngle * 2.0f, 0.25f, true));
                rotated = true;
            }
            else if(!wallRotation && !endOfPathRotation && !nextRotation && !rotated){
                nextRotation = true;
                StartCoroutine(Rotate(Vector3.forward, 90, 0.25f));
                rotated = true;
            }
            if(nextRotation && !endOfPathRotation && !wallRotation) {
                StartCoroutine(Rotate(Vector3.forward, -180, 0.50f));
                nextRotation = false;
            }
        }
    }

    public void OnPathComplete(Path p) {
		if (DEBUG) {	
			Debug.Log("Path complete");
			Debug.Log("Vector path: " + p.vectorPath.Count);
		}
		
		if (!p.error) {
			path = p;
			currentWaypoint = 0;
		}
		else {
			if (DEBUG) {
				Debug.Log("Possible error: " + p.error);
				Debug.Log("Vector path: " + p.vectorPath[0]);
				Debug.Log("Vector path: " + p.vectorPath.Count);
			}
		}
	}
  
    //NOT FINISHED ADDING VISION DETECTION TO PATH FINDING 
    //CHANGE THE VARIABLE NAMES
    public bool isPlayerVisible() {
        bool playerDetected = false;
        float distance = (targetPosition.position - transform.position).magnitude;
        float angle = Mathf.Acos(Vector2.Dot((targetPosition.position - transform.position), transform.up)
                                 / ((targetPosition.position - transform.position).magnitude * transform.up.magnitude)
                                 );
        if (distance < radius && angle < coneAngle) {
            GameObject player = GameObject.Find("Player");
            Collider2D collider = player.GetComponent<Collider2D>();
            float x = collider.bounds.extents.x;
            float y = collider.bounds.extents.y;
            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            Vector2 upperLeft = pos + new Vector2(-x, y);
            Vector2 lowerLeft = pos - (Vector2)collider.bounds.extents;
            Vector2 upperRight = pos + (Vector2)collider.bounds.extents;
            Vector2 lowerRight = pos + new Vector2(x, -y);
            RaycastHit2D raycastHit = Physics2D.Raycast(pos, upperLeft - pos);
            GameObject enemy = GameObject.Find(name);
            Collider2D col = enemy.GetComponent<Collider2D>();
            if (raycastHit == true && raycastHit.collider.name.Equals("Player")) {
                playerDetected = true;
            }
            else { //since enemy is a collidable object, when rays are cast from the center of the enemy they hit the boundaries of the enemy first
                   // so we need to cast a ray from a circle outside of square (the square is inscribed in circle)
                   // casting 5 rays from the "center" of the enemy to corners and the center point of the player
                raycastHit = Physics2D.Raycast(pos + (lowerLeft - pos).normalized * col.bounds.extents.magnitude, lowerLeft - pos);
                if (raycastHit == true && raycastHit.collider.name.Equals("Player")) {
                    playerDetected = true;
                }
                else {
                    raycastHit = Physics2D.Raycast(pos + (upperRight - pos).normalized * col.bounds.extents.magnitude, upperRight - pos);
                    if (raycastHit == true && raycastHit.collider.name.Equals("Player")) {
                        playerDetected = true;
                    }
                    else {
                        raycastHit = Physics2D.Raycast(pos + (lowerRight - pos).normalized * col.bounds.extents.magnitude, lowerRight - pos);
                        if (raycastHit == true && raycastHit.collider.name.Equals("Player")) {
                            playerDetected = true;
                        }
                        else {
                            raycastHit = Physics2D.Raycast(transform.position + (targetPosition.position - transform.position).normalized * col.bounds.extents.magnitude, targetPosition.position - transform.position);
                            if (raycastHit == true && raycastHit.collider.name.Equals("Player")) {
                                playerDetected = true;
                            }
                            else {
                                raycastHit = Physics2D.Raycast(pos + (upperLeft - pos).normalized * col.bounds.extents.magnitude, upperLeft - pos);
                                if (raycastHit == true && raycastHit.collider.name.Equals("Player")) {
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
        }
        else {
            playerDetected = false;
        }
        return playerDetected;
    }

    //checks if enemy's cone of vision hits the damn wall
    bool isCloseToWall() {
        Collider2D collider  = this.GetComponent<Collider2D>();
        Vector3 raycastDir = transform.up; 
        Vector3 pos = this.transform.position;
        RaycastHit2D raycastHit = Physics2D.Raycast(pos + collider.bounds.extents.magnitude * raycastDir, raycastDir); //casts a ray to enemy's front direction
        if(raycastHit != false && raycastHit.collider.tag.Equals("Wall") && raycastHit.distance < radius) { //if wall is hit and inside of vision radius
            return true;
        }
        return false;
    }

    /**
     * rotates the enemy
     */
    IEnumerator Rotate(Vector3 axis, float angle, float duration = 1.0f, bool isWall = false) {
        if (isWall) {
            wallRotation = true;
        }
        else {
            endOfPathRotation = true;
        }
        // quaternion that saves the code from self destruction
        Quaternion from = transform.rotation; //initializing
        Quaternion to = transform.rotation; 
        //rotation that we want to do
        to *= Quaternion.Euler(axis * angle);

        float elapsed = 0.0f; // counter
        while (elapsed < duration) { 
            transform.rotation = Quaternion.Slerp(from, to, elapsed / duration); //linear interpolation
            elapsed += Time.deltaTime; //update counter
            yield return null;
        }
        transform.rotation = to; //final rotation
        if (isWall) {
            wallRotation = false;
        }
        else {
            endOfPathRotation = false;
        }
    }

    //private void OnCollision2DEnter(Collision collision) {
    //    if(collision.gameObject.name == "Player") {
    //        Debug.Log("Game Over!");
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Player") {
            caughtPlayer = true;
        }
    }
}
