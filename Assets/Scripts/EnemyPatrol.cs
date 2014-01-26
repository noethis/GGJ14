using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPatrol : MonoBehaviour {
	public List<EnemyWaypoint> targets;
	[Range(0f, 10f)]
	public float waypointWaitTime;
	private NavMeshAgent agent;
	private EnemyWaypoint currTarget;
	public bool playerInSight;
	public float fieldOfViewAngle = 110f;
	private SphereCollider col;

	void Awake() 
	{
		col = GetComponent<SphereCollider>();
	}

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

		// If the player has entered the trigger sphere...
		if (other.CompareTag("Player"))
		{
			// By default the player is not in sight.
			playerInSight = false;

			// Create a vector from the enemy to the player and store the angle between it and forward.
			Vector3 direction = other.transform.position - transform.position;
			float angle = Vector3.Angle(direction, transform.forward);
			
			// If the angle between forward and where the player is, is less than half the angle of view...
			if(angle < fieldOfViewAngle * 0.5f)
			{
				RaycastHit hit;
				// ... and if a raycast towards the player hits something...
				//if(Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, 30))
				if(Physics.Raycast(transform.position, direction.normalized, out hit, 8))
				{
					Debug.DrawLine (other.transform.position, transform.position);
					// ... and if the raycast hits the player...
					if(hit.collider.gameObject.CompareTag("Player"))
					{
						// ... the player is in sight.
						playerInSight = true;
						print ("found player");
					}
				}
			}
		}
	}

	/*
	bool CanSeePlayer() {
		//if LOS to player
		Vector3 dir = opponent.transform.position - myTransform.position;
		Vector3 pos = myTransform.position;
		//pos.y = SHOOT_POS_Y;
		Ray ray = new Ray( pos, dir.normalized );
		RaycastHit hit;
		int layerMask = ~(1<<10); //see through clip
		if ( Physics.Raycast( ray, out hit, Mathf.Infinity, layerMask ) && hit.collider != null && hit.collider.CompareTag( "Player" ) ) {
			return true;
		}
		return false;
	}
	*/

	void OnTriggerExit (Collider other)
	{
		// If the player leaves the trigger zone...
		if(other.CompareTag("Player"))
			// ... the player is not in sight.
			playerInSight = false;
			print ("lost player");
	}
}
