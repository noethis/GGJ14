using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	//CONSTS

	//UNITY
	public float moveSpeed, followSpeed, followDist;
	public AudioClip fallDeathClip, deathClip;
	public SpriteRenderer bloodSplat;
	public ParticleSystem bloodPuff;

	//VARS
	protected float cameraDiff;
	[HideInInspector] public bool dead = false;
	protected bool isMoving = false;
	protected string deathString = "YOU LOSE!";

	protected virtual void Awake() {
	}

	protected virtual void Start() {
		cameraDiff = Camera.main.transform.position.y - transform.position.y;
//		StartCoroutine( TestMovement() );
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
		if ( dead ) {
			return;
		}

		//INACTIVE
		if (GameState.Instance.activePlayer != this) {

        } 
		//ACTIVE PLAYER
        else {
            RunAiming();
        }
	}	

	protected virtual void FixedUpdate () {
		if ( dead ) {
			return;
		}
		isMoving = false;
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

		if ( h != 0 || v != 0 ) {
			isMoving = true;
		}

		rigidbody.AddForce( movement * moveSpeed * Time.deltaTime );
	}

//	IEnumerator TestMovement() {
//		while ( true ) {
//			float v = Input.GetAxisRaw("Vertical"  );	
//			float h = Input.GetAxisRaw("Horizontal" );
//
//			if ( v != 0f || h != 0f ) {
//				Vector3 movement = new Vector3( h, 0, v );
//				rigidbody.AddForce( movement * moveSpeed * 0.4f );
//				yield return new WaitForSeconds( 0.5f );
//			}
//			else {
//				yield return 0;
//			}
//		}
//	}

	void RunAiming() {
		Quaternion oldRotation = transform.rotation;
		
		Vector3 worldPos = Camera.main.ScreenToWorldPoint( new Vector3( Input.mousePosition.x, Input.mousePosition.y, cameraDiff ) );
		Vector3 lookat =  new Vector3 ( worldPos.x, transform.position.y, worldPos.z);
		transform.LookAt(lookat);
	}

	protected virtual void OnTriggerEnter( Collider other ) {
		if (other.gameObject.name == "Pit" ) {
			if ( this == GameState.Instance.activePlayer ) {
				StartCoroutine( FallDeath() );
			}
		}
		else if (other.gameObject.name == "LevelGoal" ) {
			if ( this == GameState.Instance.activePlayer ) {
				GameState.Instance.WinLevel();
			}
		}
	}

	IEnumerator FallDeath() {
		PreDie ();
		rigidbody.drag = 20f;//Mathf.Max ( 3.5f, rigidbody.velocity.magnitude / 1f );
		AudioSource.PlayClipAtPoint( fallDeathClip, transform.position, 0.75f );
		float val = 0.1f;
		float startVal = 100f;
		while( startVal > 0f ) {
			val += 0.025f;
			startVal -= val;
			transform.localScale = new Vector3( startVal, startVal, startVal ) / 100f;
			yield return 0;
		}
		deathString = "YOU FELL IN A PIT!";
		StartCoroutine( Die( 1f ) );
	}

	protected virtual void PreDie() {
		GameState.Instance.LightsOn ();
		PlayerSight playerSight = GameState.Instance.players [0] as PlayerSight;
		playerSight.viewCone.enabled = false;

	}
	IEnumerator Die( float time = 0f ) {
		if ( dead ) {
			yield break;
		}
		dead = true;
		yield return new WaitForSeconds (time);
		GameState.Instance.LoseLevel( deathString );
	}

	public void PendulumDeath() {
		if ( dead ) {
			return;
		}
		audio.PlayOneShot( deathClip );
		StartCoroutine( SpawnBlood() );
		foreach( SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>() ) {
			sprite.enabled = false;
		}
//		deathString = "YOU FELL IN A PIT!";
		StartCoroutine( Die (0f) );
	}

	IEnumerator SpawnBlood() {
		float radius = 2f;
		int num = Random.Range( 3, 6 );
		Instantiate( bloodPuff, transform.position, transform.rotation );
		Destroy ( bloodPuff.gameObject, 3f );
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