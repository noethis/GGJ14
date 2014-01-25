using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	//CONSTS

	//UNITY
	public float moveSpeed, followSpeed, followDist;

	//VARS
	protected float cameraDiff;
	[HideInInspector] public bool dead = false;

	protected virtual void Awake() {
	}

	protected virtual void Start() {
		cameraDiff = Camera.main.transform.position.y - transform.position.y;
	}

	// Update is called once per frame
	protected virtual void Update () {
		//INACTIVE
		if (GameState.Instance.activePlayer != this) {

        } 
		//ACTIVE PLAYER
        else {
            RunAiming();
        }
	}	

	protected virtual void FixedUpdate () {
		//INACTIVE
		if ( GameState.Instance.activePlayer != this) {
			Follow();
		}
		//ACTIVE PLAYER
		else {
			RunMovement();
		}
	}

	void Follow() {
		//rotate to look at the player
		transform.LookAt( GameState.Instance.activePlayer.transform.position );
		
		//move towards the player
		float distToActivePlayer = Vector3.Distance(transform.position, GameState.Instance.activePlayer.transform.position);
		if (distToActivePlayer > followDist) {
			rigidbody.AddForce( transform.forward * followSpeed * Time.deltaTime );
		}
	}

	void RunMovement() {
		float v = Input.GetAxisRaw("Vertical"  );	
		float h = Input.GetAxisRaw("Horizontal" );

		Vector3 movement = new Vector3( h, 0, v );

		rigidbody.AddForce( movement * moveSpeed * Time.deltaTime );
	}

	void RunAiming() {
		Quaternion oldRotation = transform.rotation;
		
		Vector3 worldPos = Camera.main.ScreenToWorldPoint( new Vector3( Input.mousePosition.x, Input.mousePosition.y, cameraDiff ) );
		Vector3 lookat =  new Vector3 ( worldPos.x, transform.position.y, worldPos.z);
		transform.LookAt(lookat);
	}
}	