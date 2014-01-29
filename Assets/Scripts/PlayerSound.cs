using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSound : PlayerController {
	public Light playerLight;
	private Transform earTransform;

	// Use this for initialization
	protected virtual void Start () {
		base.Start ();
		earTransform = transform.FindChild( "Ear" );
		InvokeRepeating ("FootSteps", 0f, .6f);
	}

	void FootSteps () {
		if ( dead ) {
			return;
		}
		if ( isMoving ) {
			audio.volume = 0.66f;
			audio.Play ();
			GetComponent<SoundRayGenerator>().generate(transform.position, transform.forward, 12, Random.Range ( 6, 8 ), .15f, 360);
			StartCoroutine( Bounce() );
		}
	}

	IEnumerator Bounce () {
		float growTime = 0.2f;
		while ( growTime > 0f ) {
			growTime -= Time.deltaTime;
			earTransform.localScale = Vector3.Lerp( Vector3.one * 0.95f, Vector3.one * 1.1f, Mathf.Clamp01( growTime / 0.2f ) );
			yield return 0;
		}
		float shrinkTime = 0.4f;
		while ( shrinkTime > 0f ) {
			shrinkTime -= Time.deltaTime;
			earTransform.localScale = Vector3.Lerp( Vector3.one * 1.1f, Vector3.one, Mathf.Clamp01( shrinkTime / 0.4f ) );
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

	public void OnActive() {
		GameState.Instance.LightsOff ();
		GameState.Instance.SoundOn ();
		playerLight.enabled = true;
		Music.Instance.audio.volume = 0.1f;
		if ( !GameState.selectedEar ) {
			GameState.selectedEar = true;
			GameState.Instance.ShowPrompt( "Press E for Sonic Ray", 4f );
		}
		//show sound rays
		Camera.main.cullingMask = Camera.main.cullingMask | (1<<10);
	}
	
	public void OnInactive() {
		GameState.Instance.SoundOff ();
		playerLight.enabled = false;
		Music.Instance.audio.volume = 1f;
		//hide sound rays
		Camera.main.cullingMask = Camera.main.cullingMask & ~(1<<10);
	}

	public void Action() {
		GetComponent<SoundRayGenerator>().generate(transform.position, transform.forward, 20, 20, 3f, 360);
	}
}
