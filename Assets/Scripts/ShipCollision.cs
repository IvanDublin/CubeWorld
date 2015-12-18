using UnityEngine;
using System.Collections;

public class ShipCollision : MonoBehaviour {
	public CreateWorld manager;

	public AudioClip audioExplosion;
	AudioSource audio;
	
	bool alive = true;
	float time = 0;
	
	public float TIME_DEATH = 5f;
	
	// Use this for initialization
	void Start () {
		initSound();
	}
	
	void initSound() {
		audio = transform.gameObject.AddComponent<AudioSource>();
		audio.clip = audioExplosion;
		audio.loop = false;
		audio.playOnAwake = false;
		audio.volume = 0.8f;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!alive) {
			if (Time.time - time >= TIME_DEATH) {
				manager.gameOver();
			}
		}
	}

	//Detect any collision and start the death of the ship. Delete the controller to not moe the ship.
	//Also a rigidbody with gravity to give more reality
	void OnCollisionEnter(Collision col) {
		Debug.Log("Destruction");
		if (audio == null) initSound();
		audio.Play();
		alive = false;
		time = Time.time;

		ShipController sc = transform.parent.GetComponent<ShipController>();
		Destroy(sc);		
		Rigidbody rb = transform.GetComponent<Rigidbody>();
		rb.useGravity = true;

	}
}
