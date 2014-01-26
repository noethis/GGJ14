using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPatrol : MonoBehaviour {
	public List<EnemyWaypoint> targets;
	[Range(0f, 10f)]
	public float waypointWaitTime;
	private NavMeshAgent agent;
	private EnemyWaypoint currTarget;
	private bool playerInSight = false;
	public float fieldOfViewAngle = 45f;
	public float viewLength = 2;
	private SphereCollider col;
	public AudioClip footsteps;

	void Awake() {
		col = GetComponent<SphereCollider>();
	}

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		SetNextWaypoint();
		InvokeRepeating ("Footsteps", 1.0f, 1.0f);
	}

	private void Footsteps() {
		if (agent.hasPath)
		{
//			AudioSource.PlayClipAtPoint( footsteps, transform.position, 1.0f );
		}
	}

	// Update is called once per frame
	void Update () {
		foreach (PlayerController player in GameState.Instance.players) {
			Vector3 toPlayer = player.transform.position - transform.position;
			if (toPlayer.magnitude >= viewLength) {
				float angle = Vector3.Angle(toPlayer, transform.forward);
				if (angle < fieldOfViewAngle * 0.5f) {
					RaycastHit hit;
					if(Physics.Raycast(transform.position, toPlayer.normalized, out hit, 8)) {
						if(hit.collider.gameObject.CompareTag("Player")) {
							playerInSight = true;
						}
					}
				}
			}
		}
	}

	public void SetNextWaypoint() {
		if (currTarget == null) {
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
