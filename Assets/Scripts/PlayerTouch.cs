using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerTouch : PlayerController {

	private Switch activeSwitch;

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
		if (activeSwitch) {
			activeSwitch.Toggle();
		}
	}

	void OnTriggerEnter( Collider other ) {
		if (other.gameObject.name == "Switch" ) {
			activeSwitch = other.GetComponent<Switch>();
		}
	}

	void OnTriggerExit( Collider other ) {
		if (other.gameObject.name == "Switch" ) {
			activeSwitch = null;
		}
	}
}
