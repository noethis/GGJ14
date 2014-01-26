using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	private const float LIP = 0.25f;
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
//		float dist = 0f;
//		float totalDist = 2.5f - LIP;
//		while (dist < totalDist) {
//			leftDoor.position = leftStart - dist;
//			rightDoor.position = rightStart + dist;
//		}
		yield break;
	}
}
