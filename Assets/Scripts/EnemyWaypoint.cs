using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyWaypoint : MonoBehaviour {
	public List<EnemyWaypoint> targets;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public EnemyWaypoint GetTarget() {
		if (targets == null || targets.Count == 0) {
			return null;
		}
		return targets [Random.Range (0, targets.Count)];
	}
}
