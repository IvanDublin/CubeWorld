using UnityEngine;
using System.Collections;

public class MovementBullet : MonoBehaviour {

	public float init_time;
	public float BULLET_LIVE = 3f;
	public float speed = 6f;

	bool alive = true;


	// Use this for initialization
	void Start () {
		transform.Rotate(Vector3.right,90);
		init_time = Time.time;
	}

	
	// Update is called once per frame
	void Update () {
		if (Time.time - init_time < BULLET_LIVE) 
			transform.position += transform.up * speed *  Time.deltaTime;
		else
			Destroy(transform.gameObject);
	}

	//Detect when the bullet collsion for first time with any object exept the player.
	//When this happens, the bullet death start. Added a rigidbody with gravity to give more reality.
	void OnCollisionEnter(Collision col) {
		if (alive && col.gameObject.tag!="Player") {
			alive = false;
			speed = 0;
			init_time = Time.time;
			BULLET_LIVE = BULLET_LIVE*2;
			Rigidbody rb = transform.gameObject.GetComponent<Rigidbody>();
			rb.useGravity = true;
		}
	}
}
