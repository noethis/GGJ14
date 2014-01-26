using UnityEngine;
using System.Collections;

public class GravityWall : MonoBehaviour {

	public float force = 10;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerStay (Collider other)
	{
		other.rigidbody.AddForce (Vector3.forward * force, ForceMode.Impulse);
	}
}
