using UnityEngine;
using System.Collections;

public class CubeCollision : MonoBehaviour {

	public AudioClip audioColission;
	public AudioClip audioExplosion;
	AudioSource audio;
	AudioSource audioEx;

	int lives = 4;
	bool alive;

	private CreateWorld parent;
	
	public CreateWorld Parent {
		get { return parent; }
		set { parent = value; }
	}

	// Use this for initialization
	void Start () {
		initSound();
		alive = true;
	}
	
	void initSound() {
		audio = transform.gameObject.AddComponent<AudioSource>();
		audio.clip = audioColission;
		audio.loop = false;
		audio.playOnAwake = false;
		audio.volume = 0.8f;

		audioEx = transform.gameObject.AddComponent<AudioSource>();
		audioEx.clip = audioExplosion;
		audioEx.loop = false;
		audioEx.playOnAwake = false;
		audioEx.volume = 0.8f;
	}
	
	// Update is called once per frame
	void Update () {
		if (!alive && !audioEx.isPlaying)
			Destroy(transform.gameObject);
	}

	//Detect when a bullet collision with him. 
	//Substract one live and when arrive to 0 the cube notify to manager for his destruction.
	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == "Bullet") {
			if (audio == null) initSound();
			audio.Play();
			lives--;

			if (lives==0) { 
				audioEx.Play();
				alive = false;
				parent.destroyCube(transform.parent.gameObject);
				transform.localScale = new Vector3(0, 0, 0);
			}
		}
	}
}
