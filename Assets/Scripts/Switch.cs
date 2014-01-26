using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour {
	
	public bool switchOn;
	Behaviour halo;
	
	// Use this for initialization
	void Start () {
		switchOn = false;
		halo = (Behaviour)GetComponent("Halo");
	}
	
	// Update is called once per frame
	void Update () {
		if (switchOn) 
		{
			halo.enabled = true;
		}
		else {
			halo.enabled = false;
		}
	}
	
	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.CompareTag ("Player")) {
			switchOn = !switchOn;
		}
	}
	
}
