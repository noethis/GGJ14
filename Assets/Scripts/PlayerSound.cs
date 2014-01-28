using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSound : PlayerController {
	public Light playerLight;

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

	public void OnActive() {
		GameState.Instance.LightsOff ();
		playerLight.enabled = true;
	}
	
	public void OnInactive() {
		playerLight.enabled = false;
	}

	public void Action() {
		GetComponent<SoundRayGenerator>().generate(transform.position, transform.forward, 20, 20, 3f, 360);
	}
}
