using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPatrol : MonoBehaviour {
	public List<EnemyWaypoint> targets;
	[Range(0f, 10f)]
	public float waypointWaitTime;
	private NavMeshAgent agent;
	private EnemyWaypoint currTarget;
	public bool playerInSight = false;
	public float fieldOfViewAngle = 20f;
	public float viewLength = 2;
	private SphereCollider col;
	public AudioClip leftFootstep, rightFootstep;
	private bool isLeft = true;
	public SpriteRenderer leftFoot, rightFoot;

	void Awake() {
		col = GetComponent<SphereCollider>();
	}

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		SetNextWaypoint();
		InvokeRepeating ("Footsteps", 0f, 0.5f);
	}

	private void Footsteps() {
		if (agent.hasPath)
		{
			GetComponent<SoundRayGenerator>().generate(transform.position, transform.forward, 20, 10, 1f, 360);

//			AudioSource.PlayClipAtPoint( footsteps, transform.position, 1.0f );
			if ( isLeft ) {
				isLeft = false;
				audio.PlayOneShot( leftFootstep );
//				leftFoot.enabled = true;
//				rightFoot.enabled = false;
			}
			else {
				isLeft = true;
				audio.PlayOneShot( rightFootstep );
//				leftFoot.enabled = false;
//				rightFoot.enabled = true;
			}
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
							GameState.Instance.LoseLevel( "YOU WERE SEEN!" );
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

	public void Die() {
		Destroy (gameObject);
	}
}
