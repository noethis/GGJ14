using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSound : PlayerController {
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
		if (this == GameState.Instance.activePlayer && Input.GetMouseButtonUp (0)) {
			GetComponent<SoundRayGenerator>().generate(transform.localPosition, transform.forward, 20, 20);
		}
	}
}
