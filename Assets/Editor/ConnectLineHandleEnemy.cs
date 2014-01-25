using UnityEngine;
using System.Collections;
using UnityEditor;

// Draw lines to the connected game objects that a script has.
// if the target object doesnt have any game objects attached
// then it draws a line from the object to 0,0,0.

[CustomEditor(typeof(EnemyPatrol))] 
public class ConnectLineHandleEnemy : Editor {
	void OnSceneGUI () {
		EnemyPatrol enemy = (EnemyPatrol)target;
		int index = 0;
		foreach (EnemyWaypoint t in enemy.targets) {
			Vector3 offset = Vector3.up * 0.1f * index;
			Handles.DrawLine (enemy.transform.position + offset, t.transform.position + offset );
			index++;
		}
	}
}