using UnityEngine;
using System.Collections;

public class SoundRayGenerator : MonoBehaviour {

	public SoundRay soundRay;
	public Transform soundRayEnemy, soundRayPendulum;
	public AudioClip bouncingSoundOverride;

	public void generate(Vector3 pos, Vector3 dir, float speed, int numRays, float lifeTime = 3, float spread = 60) {
//		audio.PlayOneShot( bouncingSound );
		if (numRays == 1) {
			SoundRay sr = Instantiate(soundRay, new Vector3(pos.x, 0.5f, pos.z), Quaternion.identity) as SoundRay;
			sr.init(dir, speed, lifeTime, bouncingSoundOverride);
		} else {
			for (int i = 0; i < numRays; ++i) {
				float angle = spread / numRays;
				SoundRay sr = Instantiate(soundRay, new Vector3(pos.x, 0.5f, pos.z), Quaternion.identity) as SoundRay;
				sr.init(Rotate (dir, i * angle), speed, lifeTime, bouncingSoundOverride);
			}
		}
	}

	public void generateEnemy( Vector3 pos, Vector3 dir ) {
		for (int i = 0; i < 8; ++i) {
			float angle = 360 / 8;
			Transform srTransform = Instantiate(soundRayEnemy, new Vector3(pos.x, 0.5f, pos.z), Quaternion.identity) as Transform;
			SoundRay sr = srTransform.GetComponent<SoundRay>();
			sr.init(Rotate (dir, i * angle), 10, .15f, null);
		}
	}

	public void generatePendulum( Vector3 pos, Vector3 dir ) {
		for (int i = 0; i < 6; ++i) {
			float angle = 180 / 6;
			Transform srTransform = Instantiate(soundRayPendulum, new Vector3(pos.x, 0.5f, pos.z), Quaternion.identity) as Transform;
			SoundRay sr = srTransform.GetComponent<SoundRay>();
			sr.init(Rotate (dir, i * angle), 20, .1f, null);
		}
	}
	
	private Vector3 Rotate(Vector3 vec, float angle) {
		return Quaternion.Euler(0, angle, 0) * vec;
	}
}
