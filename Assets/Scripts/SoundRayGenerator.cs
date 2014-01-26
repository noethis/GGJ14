using UnityEngine;
using System.Collections;

public class SoundRayGenerator : MonoBehaviour {

	public SoundRay soundRay;

	public void generate(Vector3 pos, Vector3 dir, float speed, int numRays, float lifeTime = 3, float spread = 60) {
		if (numRays == 1) {
			SoundRay sr = Instantiate(soundRay, new Vector3(pos.x, 0.5f, pos.z), Quaternion.identity) as SoundRay;
			sr.init(dir, speed, lifeTime);
		} else {
			for (int i = 0; i < numRays; ++i) {
				float angle = spread / numRays;
				SoundRay sr = Instantiate(soundRay, new Vector3(pos.x, 0.5f, pos.z), Quaternion.identity) as SoundRay;
				sr.init(Rotate (dir, i * angle), speed, lifeTime);
			}
		}
	}

	private Vector3 Rotate(Vector3 vec, float angle) {
		return Quaternion.Euler(0, angle, 0) * vec;
	}
}
