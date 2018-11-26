using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AStarAI : MonoBehaviour {
	public Transform targetPosition;
	public bool reachedEndOfPath;
	public float speed = 2f;
	public float nextWaypointDistance = 3;
	public Path path;

	private int currentWaypoint = 0;
	private Vector3 prevTargetPosition;
	private Seeker seeker;
	private bool DEBUG = false;

	// Use this for initialization
	void Start () {
		seeker = GetComponent<Seeker>();

		// start calculating a path to the target
		seeker.StartPath(transform.position, targetPosition.position,
										 OnPathComplete);

		prevTargetPosition = targetPosition.position;
	}

	public void Update () {
		// update the path if the target position has changed
		if (targetPosition.position != prevTargetPosition) {
			prevTargetPosition = targetPosition.position;
			seeker.StartPath(transform.position, targetPosition.position,
										 	 OnPathComplete);
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
				} else {
					// Set a status variable to indicate that the agent has reached the end of the path.
					// You can use this to trigger some special code if your game requires that.
					reachedEndOfPath = true;
					break;
				}
			} else {
				break;
			}
		}

		// slow down smoothly upon approaching the last waypoint of the path
		var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint/nextWaypointDistance) : 1f;

		// normalize direction to the next waypoint so that it has a length of 1 world unit
		Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
		// multiply the direction by our desired speed to get a velocity
		Vector3 velocity = dir * speed * speedFactor;

		// move the game object to the target
		transform.position += velocity * Time.deltaTime;
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
}
