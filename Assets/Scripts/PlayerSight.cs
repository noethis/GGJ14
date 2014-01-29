using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSight : PlayerController {

	public Light playerLight;
	public SpriteRenderer viewCone;
	public Transform eye, iris, white;
	private Vector3 irisLocalPos, whiteLocalPos;

	protected virtual void Awake() {
		base.Awake ();
	}

	// Use this for initialization
	protected virtual void Start () {
		base.Start ();
		irisLocalPos = iris.localPosition;
		whiteLocalPos = white.localPosition;
		InvokeRepeating ("FootSteps", 0f, .6f);
	}

	void FootSteps () {
		if ( dead ) {
			return;
		}
		if ( isMoving ) {
			audio.Play ();
			StartCoroutine( Bounce() );
		}
	}

	IEnumerator Bounce () {
		float growTime = 0.2f;
		while ( growTime > 0f ) {
			growTime -= Time.deltaTime;
			eye.localScale = Vector3.Lerp( Vector3.one * 0.95f, Vector3.one * 1.07f, Mathf.Clamp01( growTime / 0.2f ) );
			eye.localScale = new Vector3( eye.localScale.x, 1f, 1f );
			iris.localScale = white.localScale = eye.localScale;
			yield return 0;
		}
		float shrinkTime = 0.4f;
		while ( shrinkTime > 0f ) {
			shrinkTime -= Time.deltaTime;
			eye.localScale = Vector3.Lerp( Vector3.one * 1.07f, Vector3.one, Mathf.Clamp01( shrinkTime / 0.4f ) );
			eye.localScale = new Vector3( eye.localScale.x, 1f, 1f );
			iris.localScale = white.localScale = eye.localScale;
			yield return 0;
		}
		yield break;
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
