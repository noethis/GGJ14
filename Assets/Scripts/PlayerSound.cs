using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSound : PlayerController {

	public SoundRay soundRay;

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
		checkInput ();
	}

	void checkInput() {
		if (GameState.Instance.activePlayer && Input.GetMouseButton (0)) {

		}
	}
}
