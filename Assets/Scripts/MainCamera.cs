using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {
	private Vector3 cameraOffset;
	
	// Use this for initialization
	void Start () {
		cameraOffset = camera.transform.position - GameState.Instance.activePlayer.transform.position;
	}
	
	// Update is called once per frame
	void Update () {		
		transform.position = GameState.Instance.activePlayer.transform.position + cameraOffset + new Vector3( 0, 0, 0 );
	}
}
