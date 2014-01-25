using UnityEngine;
using System.Collections;
using UnityEditor;

// Draw lines to the connected game objects that a script has.
// if the target object doesnt have any game objects attached
// then it draws a line from the object to 0,0,0.

[CustomEditor(typeof(EnemyWaypoint))] 
public class ConnectLineHandle : Editor {
	void OnSceneGUI () {
		EnemyWaypoint waypoint = (EnemyWaypoint)target;
		int index = 0;
		foreach (EnemyWaypoint t in waypoint.targets) {
			Vector3 offset = Vector3.up * 0.1f * index;
			Handles.DrawLine (waypoint.transform.position + offset, t.transform.position + offset );
			index++;
		}
	}
}