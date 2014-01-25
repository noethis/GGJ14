using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pickup : MonoBehaviour {
	//CONSTS
	public const float MAX_WEIGHT = 100f;

	//UNITY
	public AudioClip pickupClip;

	//VARS
	protected PlayerController player;
	protected float initialDelayTime = 0f;
	protected float respawnTime = 10f;
	public bool pickedUp = false;
	protected Vector3 primarySpawnPos, spawnPos;
	[HideInInspector] public float weight = 1f;


	protected virtual void Start () {
//		GameState.Instance.pickups.Add( this );
		primarySpawnPos = transform.position;
		Reset ();
	}

	IEnumerator WaitToSpawn() {
		renderer.material.color = Util.SetAlpha( renderer.material.color, 0f );
		yield return new WaitForSeconds( initialDelayTime );
		Respawn();
	}

	void Update() {
	}

	protected virtual bool PickupItem() {
		if ( player == null || player.dead ) {
			return false;
		}
		AudioSource.PlayClipAtPoint( pickupClip, transform.position, 0.5f );
		renderer.material.color = Util.SetAlpha( renderer.material.color, .1f );
		pickedUp = true;
		StartCoroutine( WaitToRespawn() );
		return true;
	}

	IEnumerator WaitToRespawn() {
		yield return new WaitForSeconds( respawnTime );
		Respawn();
	}

	public void Reset() {
		StopAllCoroutines();
		pickedUp = true;
		PickSpawnPos();
		StartCoroutine( WaitToSpawn() );
	}

	void PickSpawnPos() {
		transform.position = primarySpawnPos;
		List<Vector3> spawns = new List<Vector3>();
		spawns.Add( transform.position );
		foreach( Transform t in transform ) {
			spawns.Add( t.position );
		}
		spawnPos = spawns[ Random.Range ( 0, spawns.Count ) ];
	}

	public void Respawn() {
		StopAllCoroutines();
		transform.position = spawnPos;
		renderer.material.color = Util.SetAlpha( renderer.material.color, 1f );
		pickedUp = false;
	}                

	void OnTriggerStay( Collider other ) {
		if ( pickedUp ) {
			return;
		}
		if ( other.gameObject.CompareTag( "Player" ) ) {
			player = other.gameObject.GetComponent<PlayerController>();
			PickupItem();
		}
	}
}
