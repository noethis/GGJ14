using UnityEngine;
using System.Collections;

public class Pendulum : MonoBehaviour {
	private const float ARC = 10f;

	private int direction = 1;
	private Vector3 startAngle, startPos;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
		startAngle = transform.rotation.eulerAngles;
		InvokeRepeating ("Sound", 0f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround ( startPos + new Vector3( 0f, 24f, 0f ), Vector3.forward, direction * ARC * Time.deltaTime / 1f);
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
			player.PendulumDeath();
		}
	}


	private void Sound() {
		GetComponent<SoundRayGenerator>().generate(transform.position, transform.forward, 20, 10, 1f, 360);
	}
}
