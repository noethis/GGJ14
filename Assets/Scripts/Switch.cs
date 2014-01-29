using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour {
	private const float TOGGLE_TIME = 1f;
	public Door door;
	[HideInInspector] public bool switchOn;
	private bool isToggling = false;
	
	// Use this for initialization
	void Start () {
		switchOn = false;
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void OnCollisionEnter (Collision col)
	{
//		print ("COL");
//		if (col.collider.CompareTag ("Player")) {
//			Toggle();
//		}
	}

	public void Toggle() {
		if (isToggling) {
			return;
		}
		StartCoroutine(Toggle_Internal());
		audio.Play();
		switchOn = !switchOn;
		if ( switchOn ) {
			ToggleOn();
		}
		else {
			ToggleOff();
		}
	}

	IEnumerator Toggle_Internal() {
		isToggling = true;
		yield return new WaitForSeconds(TOGGLE_TIME);
		isToggling = false;
	}

	void ToggleOn() {
		light.color = Color.green;
		StartCoroutine( OpenDoor() );
	}

	IEnumerator OpenDoor() {
		yield return new WaitForSeconds( 1f );
		door.Open ();
	}

	void ToggleOff() {
		light.color = Color.red;
		StartCoroutine( CloseDoor() );
	}

	IEnumerator CloseDoor() {
		yield return new WaitForSeconds( 1f );
		door.Close ();
	}
}
