using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if ( GameState.currLevel == GameState.NUM_LEVELS ) {
			GameState.Instance.ShowMessage ("Final Level", 1.5f );
		}
		else {
			GameState.Instance.ShowMessage ("Level " + GameState.currLevel, 1.5f );
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
