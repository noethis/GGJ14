using UnityEngine;
using System.Collections;

public class SoundRay : MonoBehaviour {

	private const float LIFETIME_VARIANCE = 0.1f; //percentage
	private float speed;
	private Vector3 direction;
	private float lifetime;
	private float initTime;

	public void init (Vector3 dir, float speed, float lifetime, AudioClip overrideClip = null ) {
		this.direction = dir.normalized;
		this.speed = speed;
		this.lifetime = lifetime + Random.Range( lifetime - lifetime * LIFETIME_VARIANCE, lifetime + lifetime * LIFETIME_VARIANCE );
//		if ( overrideClip != null ) {
			audio.clip = overrideClip;
//		}
	}

	void Start () {
		rigidbody.velocity = direction * speed;
		initTime = Time.time;
		Destroy (gameObject, lifetime);
	}

	void LateUpdate() {
		rigidbody.velocity = speed * (rigidbody.velocity.normalized);
//		GetComponent<TrailRenderer>().material.color = Util.SetAlpha (renderer.material.color, 1f - Mathf.Clamp01 ((Time.time - initTime) / lifetime));
	}

	void OnCollisionEnter (Collision collision) {
		if ( collision.collider.name == "FakeWall" ) {
			return;
		}
		ContactPoint contact = collision.contacts[0];
		rigidbody.velocity = Vector3.Reflect(direction, contact.normal);
		direction = rigidbody.velocity.normalized;
		audio.PlayOneShot( audio.clip );
		audio.pitch = Mathf.Lerp( 1.5f, .66f, ( Time.time - initTime ) / lifetime );
	}
}
