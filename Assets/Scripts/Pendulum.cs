using UnityEngine;
using System.Collections;

public class Pendulum : MonoBehaviour {
	public float ARC = 30f;
	public float  TIME = 1f;

	private int direction = 1;
	private Vector3 startAngle, startPos;
	public float startTime = 0f;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
		startAngle = transform.rotation.eulerAngles;
//		startTime = Random.Range( 0f, 4f );
		InvokeRepeating ("Sound", 2f + startTime, TIME * 2f );
		InvokeRepeating ("SoundGen", startTime, .25f);
	}
	
	// Update is called once per frame
	void Update () {
		if ( startTime > 0f ) {
			startTime -= Time.deltaTime;
			return;
		}
		transform.RotateAround ( startPos + new Vector3( 0f, 24f, 0f ), transform.forward, direction * ARC * Time.deltaTime / TIME);
		float val = Mathf.Abs( (startAngle.z - transform.rotation.eulerAngles.z) );
		if (val >= 180f) {
			val = 360f - val;
		}
		print(val);
		if ( direction == 1 && val >= ARC ) {
			direction = -1;
		}
		else if ( direction == -1 && -val <= -ARC ) {
			direction = 1;
		}
	}

	void OnTriggerEnter( Collider other ) {
		if (other.CompareTag ("Player")) {
			PlayerController player = other.gameObject.GetComponent<PlayerController>();
			if ( player == GameState.Instance.activePlayer ) {
				player.PendulumDeath();
			}
		}
	}


	private void Sound() {
		audio.PlayOneShot( audio.clip );
	}

	private void SoundGen() {
		GetComponent<SoundRayGenerator>().generatePendulum(transform.position, direction * transform.forward);
	}
}
