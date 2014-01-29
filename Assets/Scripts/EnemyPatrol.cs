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
	public float fieldOfViewAngle;
	public float viewLength = 2;
	private SphereCollider col;
	public AudioClip[] leftFootsteps, rightFootsteps;
	public AudioClip gaspClip, painClip;
	private bool isLeft = true;
	public SpriteRenderer leftFoot, rightFoot;
	public SpriteRenderer bloodSplat;
	public ParticleSystem bloodPuff;
	private bool dead = false;
	public bool invisible = false;
	private const float PUNCH_DIST = 2.5f;

	void Awake() {
		col = GetComponent<SphereCollider>();
	}

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
//		StartCoroutine( SetNextWaypoint() );
		if ( targets.Count > 0 ) {
			currTarget = targets[ 0 ];
			agent.SetDestination(currTarget.transform.position);
		}
		else {
			rigidbody.isKinematic = true;
		}
		InvokeRepeating ("Footsteps", 0f, 0.5f);
	}

	private void Footsteps() {
		if ( dead ) {
			return;
		}
		if (agent.hasPath)
		{
			GetComponent<SoundRayGenerator>().generateEnemy(transform.position, transform.forward );

			if ( isLeft ) {
				isLeft = false;
				audio.PlayOneShot( leftFootsteps[ Random.Range( 0, leftFootsteps.Length ) ] );
				SpawnLeft();
			}
			else {
				isLeft = true;
				audio.PlayOneShot( rightFootsteps[ Random.Range( 0, rightFootsteps.Length ) ] );
				SpawnRight();
			}
		}
		else {
			SpawnLeft();
			SpawnRight();
		}
	}

	private void SpawnLeft() {
		if ( invisible ) {
			return;
		}
		SpriteRenderer left = Instantiate( leftFoot ) as SpriteRenderer;
		left.transform.position = transform.position + transform.right * -1f * .5f;
		Vector3 pos = left.transform.position;
		pos.y = 0.01f;
		left.transform.position = pos;
		Vector3 angle = left.transform.eulerAngles;
		angle.y = transform.eulerAngles.y;
		left.transform.eulerAngles = angle;
		StartCoroutine( Util.FadeOut( left, 1f, 1f ) );
	}

	private void SpawnRight() {
		if ( invisible ) {
			return;
		}
		SpriteRenderer right = Instantiate( rightFoot ) as SpriteRenderer;
		right.transform.position = transform.position + transform.right * .5f;
		Vector3 pos = right.transform.position;
		pos.y = 0.01f;
		right.transform.position = pos;
		Vector3 angle = right.transform.eulerAngles;
		angle.y = transform.eulerAngles.y;
		right.transform.eulerAngles = angle;
		StartCoroutine( Util.FadeOut( right, 1f, 1f ) );
	}

	// Update is called once per frame
	void Update () {
		if ( playerInSight || dead ) {
			return;
		}

		Vector3 toPlayer = GameState.Instance.activePlayer.transform.position - transform.position;
		if (toPlayer.magnitude <= viewLength) {
			float angle = Vector3.Angle(toPlayer, transform.forward);
			print ( angle );
			if (angle < fieldOfViewAngle * 0.5f) {
				RaycastHit hit;
//				Util.DrawLine( transform.position, transform.position + toPlayer.normalized * viewLength, Color.red );
				if(Physics.Raycast(transform.position, toPlayer.normalized, out hit, viewLength)) {
					if(hit.collider.gameObject.CompareTag("Player")) {
						StartCoroutine( Alerted() );						
					}
				}
			}
		}

		if ( GameState.Instance.activePlayer is PlayerTouch ) {
			PlayerTouch playerTouch = GameState.Instance.activePlayer as PlayerTouch;
			if (toPlayer.magnitude <= PUNCH_DIST ) {
				playerTouch.UpdateActiveEnemy( this );
			}
			else if ( this == playerTouch.activeEnemy ) {
				playerTouch.ClearActiveEnemy();
			}
		}
	}

	IEnumerator Alerted() {
		audio.PlayOneShot( gaspClip );
		playerInSight = true;
		yield return new WaitForSeconds( 1f );
		GameState.Instance.LoseLevel( "YOU WERE SEEN!" );
	}
	
	IEnumerator SetNextWaypoint() {
		agent.Stop();
		agent.ResetPath();
		rigidbody.isKinematic = true;

		if (currTarget == null) {
			currTarget = targets [Random.Range(0, targets.Count)];
		} 
		else {
			currTarget = currTarget.GetTarget();
		}

		if (currTarget != null) {
			Vector3 vec = currTarget.transform.position - transform.position;
			Quaternion q = Quaternion.LookRotation( vec.normalized );
			for( float i = 0f; i < waypointWaitTime; i += Time.deltaTime ) {
				transform.rotation = Quaternion.Slerp (transform.rotation, q, Time.deltaTime);
				yield return 0;
			}
			rigidbody.isKinematic = false;
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
				StartCoroutine( SetNextWaypoint() );
			}
		}
	}

	public void Die() {
		dead = true;
		agent.Stop();
		audio.PlayOneShot( painClip );
		StartCoroutine( SpawnBlood() );
		this.enabled = false;
		if ( GameState.Instance.activePlayer is PlayerTouch ) {
			PlayerTouch playerTouch = GameState.Instance.activePlayer as PlayerTouch;
			if ( this == playerTouch.activeEnemy ) {
				playerTouch.ClearActiveEnemy();
			}
		}
		Destroy (gameObject, 2f);
	}

	IEnumerator SpawnBlood() {
		float radius = 2f;
		int num = Random.Range( 3, 6 );
		GameObject g = Instantiate( bloodPuff, transform.position, transform.rotation ) as GameObject;
		Destroy ( g, 3f );
		for( int i = 0; i < num; i++ ) {
			SpriteRenderer blood = Instantiate( bloodSplat ) as SpriteRenderer;
			blood.transform.position = transform.position + transform.right * Random.Range ( -radius, radius ) + transform.forward * Random.Range ( -radius, radius );
			Vector3 pos = blood.transform.position;
			pos.y = 0.01f;
			blood.transform.position = pos;
			Vector3 angle = blood.transform.eulerAngles;
			angle.y = Random.Range( 0f, 180f );
			blood.transform.eulerAngles = angle;
			StartCoroutine( Util.FadeOut( blood, 3f, 3f ) );
			yield return new WaitForSeconds( Random.Range( 0.05f, 0.15f ) );
		}
	}
}
