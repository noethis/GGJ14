using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	private const float LIP = 0.05f;
	public Transform leftDoor, rightDoor;

	public bool isOpen = false;

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

	}

	IEnumerator Open_Internal() {

	}
}
