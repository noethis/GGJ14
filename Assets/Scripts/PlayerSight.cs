using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSight : PlayerController {

	public SpriteRenderer viewCone;

	protected virtual void Awake() {
		base.Awake ();
	}

	// Use this for initialization
	protected virtual void Start () {
		base.Start ();
	}

	public void OnActive() {
		viewCone.enabled = true;
	}

	public void OnInactive() {
		viewCone.enabled = false;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		base.Update ();
	}

	// Update is called once per physics frame
	protected virtual void FixedUpdate () {
		base.FixedUpdate ();
	}
}
