using UnityEngine;
using System.Collections;

public class HintTrigger : MonoBehaviour {
	public string hintString;
	public int count = 1;

	void OnTriggerEnter( Collider other ) {
		if ( other.CompareTag( "Player" ) ) {
			if ( count > 0 ) {
				count--;
				GameState.Instance.ShowPrompt( hintString, 4f );
			}
		}
	}
}
