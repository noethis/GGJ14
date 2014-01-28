using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSight : PlayerController {

	public Light playerLight;
	public SpriteRenderer viewCone;
	public Transform iris, white;
	private Vector3 irisLocalPos, whiteLocalPos;

	protected virtual void Awake() {
		base.Awake ();
	}

	// Use this for initialization
	protected virtual void Start () {
		base.Start ();
		irisLocalPos = iris.localPosition;
		whiteLocalPos = white.localPosition;
	}

	public void Action() {

	}

	public void OnActive() {
//		viewCone.enabled = true;
		GameState.Instance.LightsOn ();
		playerLight.enabled = true;
	}

	public void OnInactive() {
//		viewCone.enabled = false;
		playerLight.enabled = false;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		base.Update ();
		iris.localPosition = irisLocalPos + rigidbody.velocity / 6f * 0.1f;
//		white.localPosition = whiteLocalPos + rigidbody.velocity / 6f * 0.1f;
	}

	// Update is called once per physics frame
	protected virtual void FixedUpdate () {
		base.FixedUpdate ();
	}

	protected virtual void PreDie() {
		base.PreDie ();
		viewCone.enabled = false;
	}
}
