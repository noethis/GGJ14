using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPatrol : MonoBehaviour {
	public List<EnemyWaypoint> targets;
	[Range(0f, 10f)]
	public float waypointWaitTime;
	private NavMeshAgent agent;
	private EnemyWaypoint currTarget;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		SetNextWaypoint();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetNextWaypoint() {
		if (currTarget == null) {
			if (targets == null || targets.Count == 0) {
				currTarget = null;
			}
			currTarget = targets [Random.Range(0, targets.Count)];
		} 
		else {
			currTarget = currTarget.GetTarget();
		}

		if (currTarget != null) {
			agent.ResetPath();
			agent.SetDestination(currTarget.transform.position);
		}
	}

	void DebugComputerMovement() {
		if ( agent.path != null ) {
			Vector3 prevCorner = Vector3.zero;
			foreach (Vector3 corner in agent.path.corners ) {
				if ( prevCorner == Vector3.zero ) {
					prevCorner = transform.position;
				}
				Util.DrawLine( prevCorner + new Vector3(0,1,0), corner + new Vector3(0,1,0), Color.blue );
				prevCorner = corner;
			}
		}
	}

	void OnTriggerEnter( Collider other ) {
		if (other.CompareTag("Waypoint")) {
			EnemyWaypoint waypoint = other.GetComponent<EnemyWaypoint>();
			if ( waypoint == currTarget ) {
				Invoke( "SetNextWaypoint", waypointWaitTime );
			}
		}
	}
}
