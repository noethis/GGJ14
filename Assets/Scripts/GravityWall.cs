using UnityEngine;
using System.Collections;

public class GravityWall : MonoBehaviour {

	public float force = 10;
	public AudioClip gravityWall;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerStay (Collider other)
	{
		if (other.CompareTag("Player")) 
		{
			AudioSource.PlayClipAtPoint( gravityWall, transform.position, 0.5f );
			other.rigidbody.AddForce (Vector3.forward * force, ForceMode.Impulse);
		}
	}
}
