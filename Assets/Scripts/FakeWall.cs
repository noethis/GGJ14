using UnityEngine;
using System.Collections;

public class FakeWall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Punch() {
		StartCoroutine(Punch_Internal());
	}

	IEnumerator Punch_Internal() {
		foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>()) {
			mesh.enabled = false;
		}
		foreach (BoxCollider box in GetComponentsInChildren<BoxCollider>()) {
			box.enabled = false;
		}

		ParticleSystem ps = GetComponentInChildren<ParticleSystem> ();
		ps.Play();
		yield return new WaitForSeconds(4f);
		Destroy(gameObject);
	}
}
