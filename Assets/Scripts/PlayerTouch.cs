using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerTouch : PlayerController {

	private Switch activeSwitch;
	private FakeWall activeFakeWall;

	// Use this for initialization
	protected virtual void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		base.Update ();
	}

	// Update is called once per physics frame
	protected virtual void FixedUpdate () {
		base.FixedUpdate ();
	}

	public void Action() {
		if (activeSwitch != null ) {
			activeSwitch.Toggle ();
		} else if (activeFakeWall != null) {
			activeFakeWall.Punch();
		}
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.collider.name == "FakeWall" ) {
			activeFakeWall = col.collider.GetComponent<FakeWall>();
			GameState.Instance.ShowPrompt( "Press E to punch through fake wall" );
		}
	}

	void OnCollisionExit (Collision col)
	{
		if (col.collider.name == "FakeWall" ) {
			activeFakeWall = null;
			GameState.Instance.HidePrompt();
		}
	}

	void OnTriggerEnter( Collider other ) {
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
