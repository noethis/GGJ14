using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerTouch : PlayerController {

	private Switch activeSwitch;
	private FakeWall activeFakeWall;
	public EnemyPatrol activeEnemy;
	public AudioClip punchSnd;
	public Light playerLight;
	public Transform hand;

	// Use this for initialization
	protected virtual void Start () {
		base.Start ();
		InvokeRepeating ("FootSteps", 0f, .6f);
	}

	void FootSteps () {
		if ( dead ) {
			return;
		}
		if ( isMoving ) {
			if ( !audio.isPlaying ) {
				audio.Play ();
			}
			StartCoroutine( Move() );
		}
		else {
			audio.Stop();
		}
	}
	
	IEnumerator Move () {
		float wiggleTime = 0.2f;
		while ( wiggleTime > 0f ) {
			float startAngle = hand.rotation.eulerAngles.y;
			wiggleTime -= Time.deltaTime;
			float angle = Mathf.LerpAngle( startAngle - 5f, startAngle + 5f, Mathf.Clamp01( wiggleTime / 0.2f ) );
			hand.Rotate ( Vector3.forward, angle );
			yield return 0;
		}
		wiggleTime = 0.2f;
		while ( wiggleTime > 0f ) {
			float startAngle = hand.rotation.eulerAngles.y;
			wiggleTime -= Time.deltaTime;
			float angle = Mathf.LerpAngle( startAngle + 5f, startAngle - 5f, Mathf.Clamp01( wiggleTime / 0.2f ) );
			hand.Rotate ( Vector3.forward, angle );
			yield return 0;
		}
		yield break;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		base.Update ();
	}

	// Update is called once per physics frame
	protected virtual void FixedUpdate () {
		base.FixedUpdate ();
	}

	public void UpdateActiveEnemy( EnemyPatrol enemy ) {
		activeEnemy = enemy;
		GameState.Instance.ShowPrompt( "Press E to take down enemy" );
	}

	public void ClearActiveEnemy() {
		activeEnemy = null;
		GameState.Instance.HidePrompt();
	}

	public void OnActive() {
		GameState.Instance.LightsOff ();
		playerLight.enabled = true;
	}

	
	public void OnInactive() {
		playerLight.enabled = false;
	}

	public void Action() {
		if (activeSwitch != null ) {
			activeSwitch.Toggle ();
		} else if (activeFakeWall != null) {
			audio.PlayOneShot( punchSnd );
			activeFakeWall.Punch();
			GameState.Instance.HidePrompt();
		}
		else if (activeEnemy != null) {
			activeEnemy.Die();
			GameState.Instance.HidePrompt();
		}
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.collider.name == "FakeWall" ) {
			activeFakeWall = col.collider.GetComponent<FakeWall>();
			GameState.Instance.ShowPrompt( "Press E to punch through fake wall" );
		}
//		if (col.collider.name == "EnemyPatrol" ) {
//			activeEnemy = col.collider.GetComponent<EnemyPatrol>();
//			if ( activeEnemy.playerInSight ) {
//				activeEnemy = null;
//			}
//			else {
//				GameState.Instance.ShowPrompt( "Press E to take down enemy" );
//			}
//		}
	}

	void OnCollisionExit (Collision col)
	{
		if (col.collider.name == "FakeWall" ) {
			activeFakeWall = null;
			GameState.Instance.HidePrompt();
		}
//		if (col.collider.name == "EnemyPatrol" ) {
//			activeEnemy = null;
//			GameState.Instance.HidePrompt();
//		}
	}

	protected virtual void OnTriggerEnter( Collider other ) {
		base.OnTriggerEnter( other );
		if (other.gameObject.name == "Switch" ) {
			activeSwitch = other.GetComponent<Switch>();
			GameState.Instance.ShowPrompt( "Press E to pull lever" );
		}
	}
		
	void OnTriggerExit( Collider other ) {
		if (other.gameObject.name == "Switch" ) {
			activeSwitch = null;
			GameState.Instance.HidePrompt();
		}
	}
}
