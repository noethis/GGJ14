using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	private const float MOVE_TIME = 3f, MOVE_DIST = 1f;
	public Transform leftDoor, rightDoor;
	public AudioClip closeClip, openClip;
	
	public bool isOpen = false;
	private bool isMoving = false;
	
	private Vector3 leftStart, rightStart;
	
	// Use this for initialization
	void Start () {
		leftStart = leftDoor.position;
		rightStart = rightDoor.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void Open() {
		if (isMoving) {
			return;
		}
		StartCoroutine (Open_Internal ());
	}
	
	IEnumerator Open_Internal() {
		audio.PlayOneShot( openClip );
		isMoving = true;
		float dist = 0f;
		float totalDist = MOVE_DIST;
		while (dist < totalDist) {
			dist += ( MOVE_TIME * Time.deltaTime ) / totalDist;
			leftDoor.position = leftStart - transform.right * dist;
			rightDoor.position = rightStart + transform.right * dist;
			yield return 0;
		}
		isMoving = false;
		yield break;
	}

	public void Close() {
		if (isMoving) {
			return;
		}
		StartCoroutine (Close_Internal ());
	}
	
	IEnumerator Close_Internal() {
		audio.PlayOneShot( closeClip );
		isMoving = true;
		float dist = 0f;
		float totalDist = MOVE_DIST;
		Vector3 leftEnd = leftDoor.position;
		Vector3 rightEnd = rightDoor.position;
		while (dist < totalDist) {
			dist += ( MOVE_TIME * Time.deltaTime ) / totalDist;
			leftDoor.position = leftEnd + transform.right * dist;
			rightDoor.position = rightEnd - transform.right * dist;
			yield return 0;
		}
		leftDoor.position = leftStart;
		rightDoor.position = rightStart;
		isMoving = false;
		yield break;
	}
}
