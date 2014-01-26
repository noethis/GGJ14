﻿using UnityEngine;
using System.Collections;

public class SoundRay : MonoBehaviour {

	private float speed;
	private Vector3 direction;
	private float lifetime;
	private float initTime;

	public void init (Vector3 dir, float speed, float lifetime) {
		this.direction = dir.normalized;
		this.speed = speed;
		this.lifetime = lifetime;
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
		ContactPoint contact = collision.contacts[0];

		rigidbody.velocity = Vector3.Reflect(direction, contact.normal);
		direction = rigidbody.velocity.normalized;
	}
}
