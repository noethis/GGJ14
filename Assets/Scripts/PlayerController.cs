using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	//CONSTS

	//UNITY
	public float moveSpeed, followSpeed, followDist;
	public AudioClip fallDeathClip;

	//VARS
	protected float cameraDiff;
	[HideInInspector] public bool dead = false;

	protected virtual void Awake() {
	}

	protected virtual void Start() {
		cameraDiff = Camera.main.transform.position.y - transform.position.y;
	}

//	public abstract void OnActive() {
//
//	}
//
//	public abstract void OnInactive() {
//
//	}

	// Update is called once per frame
	protected virtual void Update () {
		//INACTIVE
		if (GameState.Instance.activePlayer != this) {

        } 
		//ACTIVE PLAYER
        else {
            RunAiming();
        }
	}	

	protected virtual void FixedUpdate () {
		//INACTIVE
		if ( GameState.Instance.activePlayer != this) {
			Follow();
		}
		//ACTIVE PLAYER
		else {
			RunMovement();
		}
	}

	void Follow() {
		//rotate to look at the player
		transform.LookAt( GameState.Instance.activePlayer.transform.position );
		
		//move towards the player
		float distToActivePlayer = Vector3.Distance(transform.position, GameState.Instance.activePlayer.transform.position);
		if (distToActivePlayer > followDist) {
			rigidbody.AddForce( transform.forward * followSpeed * Time.deltaTime );
		}
	}

	void RunMovement() {
		float v = Input.GetAxisRaw("Vertical"  );	
		float h = Input.GetAxisRaw("Horizontal" );

		Vector3 movement = new Vector3( h, 0, v );

		rigidbody.AddForce( movement * moveSpeed * Time.deltaTime );
	}

	void RunAiming() {
		Quaternion oldRotation = transform.rotation;
		
		Vector3 worldPos = Camera.main.ScreenToWorldPoint( new Vector3( Input.mousePosition.x, Input.mousePosition.y, cameraDiff ) );
		Vector3 lookat =  new Vector3 ( worldPos.x, transform.position.y, worldPos.z);
		transform.LookAt(lookat);
	}

	void OnTriggerEnter( Collider other ) {
		if (other.gameObject.name == "Pit" ) {
			StartCoroutine( FallDeath() );
		}
		else if (other.gameObject.name == "LevelGoal" ) {
			GameState.Instance.WinLevel();
		}
	}

	IEnumerator FallDeath() {
		PreDie ();
		rigidbody.drag = 20f;//Mathf.Max ( 3.5f, rigidbody.velocity.magnitude / 1f );
		AudioSource.PlayClipAtPoint( fallDeathClip, transform.position, 1.0f );
		float val = 0.1f;
		float startVal = 100f;
		while( startVal > 0f ) {
			val += 0.025f;
			startVal -= val;
			transform.localScale = new Vector3( startVal, startVal, startVal ) / 100f;
			yield return 0;
		}
		StartCoroutine( Die( 2f ) );
	}

	protected virtual void PreDie() {
		GameState.Instance.LightsOn ();
		PlayerSight playerSight = GameState.Instance.players [0] as PlayerSight;
		playerSight.viewCone.enabled = false;

	}
	IEnumerator Die( float time = 0f ) {
		dead = true;
		yield return new WaitForSeconds (time);
		GameState.Instance.LoseLevel();
	}

	public void PendulumDeath() {
		StartCoroutine( Die (0f) );
	}
}	